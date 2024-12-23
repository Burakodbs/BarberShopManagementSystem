using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace BarberShopManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
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

        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Address");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");

            // Bugünün tarihini ViewBag ile gönderiyoruz
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");

            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            string userEmail = User.Identity.Name;
            var customer = _context.Customers.FirstOrDefault(c => c.Email == userEmail);
            if (customer == null)
            {
                return Unauthorized();
            }

            // Validate RandevuZamani
            if (appointment.RandevuZamani == null || appointment.RandevuZamani <= DateTime.Now)
            {
                ModelState.AddModelError("RandevuZamani", "You cannot select a past date or time.");
                ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Address", appointment.SalonId);
                ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
                ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", appointment.EmployeeId);
                return View(appointment);
            }

            if (ModelState.IsValid)
            {
                // Check for conflicting appointment
                var conflictingAppointment = await _context.Appointments
                    .Where(a => a.EmployeeId == appointment.EmployeeId
                                && a.RandevuZamani == appointment.RandevuZamani)
                    .FirstOrDefaultAsync();

                if (conflictingAppointment != null)
                {
                    ModelState.AddModelError("RandevuZamani", "The selected time slot is not available. Please choose another time.");
                    ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Address", appointment.SalonId);
                    ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
                    ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", appointment.EmployeeId);
                    return View(appointment);
                }
                else
                {
                    // If no errors, process the appointment
                    appointment.CustomerName = customer.FirstName;
                    appointment.CustomerPhone = customer.PhoneNumber;
                    appointment.IsConfirmed = false;

                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Return view with error messages
            ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Address", appointment.SalonId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", appointment.EmployeeId);
            return View(appointment);
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            // Get logged-in user name
            string userEmail = User.Identity?.Name;
            var customer = _context.Customers.FirstOrDefault(c => c.Email == userEmail);
            if (customer == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            // Fetch appointments for the logged-in user
            var userAppointments = _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Where(a => a.CustomerName == customer.FirstName);

            return View(await userAppointments.ToListAsync());
        }



        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
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
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTimes(int salonId, DateTime date)
        {
            var salon = await _context.Salons.FirstOrDefaultAsync(s => s.Id == salonId);
            if (salon == null)
            {
                return BadRequest("Invalid Salon");
            }

            var openingTime = salon.OpeningTime;
            var closingTime = salon.ClosingTime;

            var times = new List<string>();
            for (var time = openingTime; time < closingTime; time = time.Add(TimeSpan.FromMinutes(30)))
            {
                times.Add(time.ToString(@"hh\:mm"));
            }

            var bookedTimes = await _context.Appointments
                .Where(a => a.SalonId == salonId && a.RandevuZamani.Date == date.Date)
                .Select(a => a.RandevuZamani.TimeOfDay)
                .ToListAsync();

            times = times.Where(t => !bookedTimes.Contains(TimeSpan.Parse(t))).ToList();

            return Json(times);
        }

    }
}
