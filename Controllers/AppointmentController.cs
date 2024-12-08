using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace BarberShopManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AppointmentController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Appointment.Include(a => a.Employee).Include(a => a.Salon).Include(a => a.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var startHour = new TimeSpan(8, 0, 0); // 08:00
            var endHour = new TimeSpan(19, 0, 0); // 19:00 (07:00 PM)

            var timeSlots = new List<SelectListItem>();
            var baseDate = DateTime.Today; // Bugünün tarihi

            for (var time = startHour; time <= endHour; time = time.Add(new TimeSpan(1, 0, 0)))
            {
                var timeAsDateTime = baseDate.Add(time);

                timeSlots.Add(new SelectListItem
                {
                    Value = time.ToString(@"hh\:mm"),
                    Text = timeAsDateTime.ToString("hh:mm tt")
                });
            }

            // ViewData ile ServiceId'yi de gönderiyoruz
            ViewData["TimeSlots"] = timeSlots; // Zaman dilimlerini ViewData'ya ekle
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Name");
            ViewData["ServiceId"] = new SelectList(_context.SalonServices, "Id", "ServiceName"); // ServiceId için SelectList

            return View();
        }


        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SalonId,EmployeeId,ServiceId,CustomerId,StartTime,IsApproved")] Appointment appointment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // If the user is not logged in, redirect them to the login page
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Ensure the appointment start time is at the full hour (e.g., 10:00, 11:00)
                appointment.StartTime = new DateTime(appointment.StartTime.Year, appointment.StartTime.Month, appointment.StartTime.Day, appointment.StartTime.Hour, 0, 0);
                appointment.EndTime = appointment.StartTime.AddMinutes(appointment.Service.Duration);



                // Validate that the employee is available at the selected time
                var employee = await _context.Employees.FindAsync(appointment.EmployeeId);
                if (appointment.StartTime.TimeOfDay < employee.AvailableFrom || appointment.EndTime.TimeOfDay > employee.AvailableTo)
                {
                    ModelState.AddModelError("StartTime", "Employee is not available at this time.");
                    return View(appointment);
                }

                // Check if the employee is already booked at the requested time
                var existingAppointments = await _context.Appointment
                    .Where(a => a.EmployeeId == appointment.EmployeeId &&
                                a.StartTime < appointment.EndTime &&
                                a.EndTime > appointment.StartTime)
                    .ToListAsync();

                if (existingAppointments.Any())
                {
                    ModelState.AddModelError("StartTime", "The employee already has an appointment at this time.");
                    return View(appointment);
                }

                // Seçilen hizmetin fiyatını hesaplayalım
                var service = await _context.SalonServices.FindAsync(appointment.ServiceId);
                if (service != null)
                {
                    // Fiyatı hizmetin fiyatına göre ayarlıyoruz
                    appointment.Price = service.Price;
                }

                // Set the current user as the CustomerId for the appointment
                appointment.CustomerId = _userManager.GetUserId(User); // Assuming CustomerId is the logged-in username

                // Save the appointment
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var startHour = new TimeSpan(8, 0, 0); // 08:00
            var endHour = new TimeSpan(19, 0, 0); // 19:00 (07:00 PM)

            var timeSlots = new List<SelectListItem>();
            var baseDate = DateTime.Today; // Bugünün tarihi

            for (var time = startHour; time <= endHour; time = time.Add(new TimeSpan(1, 0, 0)))
            {
                var timeAsDateTime = baseDate.Add(time);

                timeSlots.Add(new SelectListItem
                {
                    Value = time.ToString(@"hh\:mm"),
                    Text = timeAsDateTime.ToString("hh:mm tt")
                });
            }

            // ViewData ile ServiceId'yi de gönderiyoruz
            ViewData["TimeSlots"] = timeSlots; // Zaman dilimlerini ViewData'ya ekle
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", appointment.EmployeeId);
            ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Name", appointment.SalonId);
            ViewData["ServiceId"] = new SelectList(_context.SalonServices, "Id", "ServiceName", appointment.ServiceId);
            return View(appointment);
        }



        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", appointment.EmployeeId);
            ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Id", appointment.SalonId);
            ViewData["ServiceId"] = new SelectList(_context.SalonServices, "Id", "Id", appointment.ServiceId);
            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SalonId,EmployeeId,ServiceId,CustomerId,StartTime,EndTime,Price,IsApproved")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                // Validate that StartTime is before EndTime
                if (appointment.StartTime >= appointment.EndTime)
                {
                    ModelState.AddModelError("StartTime", "Start time must be earlier than End time.");
                    return View(appointment);
                }

                // Validate that the appointment is within the working hours of the salon
                var salon = await _context.Salons.FindAsync(appointment.SalonId);
                if (appointment.StartTime.TimeOfDay < salon.StartHour || appointment.EndTime.TimeOfDay > salon.EndHour)
                {
                    ModelState.AddModelError("StartTime", "Appointment must be within the salon's working hours.");
                    return View(appointment);
                }

                // Validate that the employee is available at the selected time
                var employee = await _context.Employees.FindAsync(appointment.EmployeeId);
                if (appointment.StartTime.TimeOfDay < employee.AvailableFrom || appointment.EndTime.TimeOfDay > employee.AvailableTo)
                {
                    ModelState.AddModelError("StartTime", "Employee is not available at this time.");
                    return View(appointment);
                }

                // Check if the employee is already booked at the requested time
                var existingAppointments = await _context.Appointment
                    .Where(a => a.EmployeeId == appointment.EmployeeId &&
                                a.StartTime < appointment.EndTime &&
                                a.EndTime > appointment.StartTime &&
                                a.Id != appointment.Id) // Exclude the current appointment if we're editing
                    .ToListAsync();

                if (existingAppointments.Any())
                {
                    ModelState.AddModelError("StartTime", "The employee already has an appointment at this time.");
                    return View(appointment);
                }

                // All validation passed, now update the appointment
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If validation failed, populate drop-down lists and return the view with errors
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", appointment.EmployeeId);
            ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Name", appointment.SalonId);
            ViewData["ServiceId"] = new SelectList(_context.SalonServices, "Id", "ServiceName", appointment.ServiceId);
            return View(appointment);
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
