﻿using System;
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
                    appointment.CustomerName = customer.FirstName;
                    appointment.CustomerPhone = customer.PhoneNumber;
                    appointment.IsConfirmed = false;
                    appointment.Employee = await _context.Employees.Where(e => e.Id == appointment.EmployeeId).FirstOrDefaultAsync();
                    appointment.Service = await _context.Services.Where(s => s.Id == appointment.ServiceId).FirstOrDefaultAsync();
                    if (appointment.Service == appointment.Employee.ExpertService)
                    {
                        appointment.Price = appointment.Service.Price + 100;

                    }
                    else { appointment.Price = appointment.Service.Price; }
                    appointment.Duration = appointment.Service.Duration;

                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Address", appointment.SalonId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", appointment.EmployeeId);
            return View(appointment);
        }

        public async Task<IActionResult> Index()
        {
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

            var userAppointments = _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Where(a => a.CustomerName == customer.FirstName);

            return View(await userAppointments.ToListAsync());
        }


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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllAppointments(string statusFilter = "All", string sortOrder = "Date")
        {
            var appointments = _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .AsQueryable();

            // Filter by status
            if (statusFilter == "Confirmed")
            {
                appointments = appointments.Where(a => a.IsConfirmed);
            }
            else if (statusFilter == "Pending")
            {
                appointments = appointments.Where(a => !a.IsConfirmed);
            }

            // Sort by chosen order
            appointments = sortOrder switch
            {
                "Date" => appointments.OrderBy(a => a.RandevuZamani),
                "Customer" => appointments.OrderBy(a => a.CustomerName),
                _ => appointments.OrderBy(a => a.RandevuZamani)
            };

            var result = await appointments.ToListAsync();
            ViewBag.StatusFilter = statusFilter;
            ViewBag.SortOrder = sortOrder;
            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ConfirmAppointment(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);

            if (appointment != null)
            {
                appointment.IsConfirmed = true;
                _context.SaveChanges();
            }

            return RedirectToAction("AllAppointments");
        }

        
    }
}