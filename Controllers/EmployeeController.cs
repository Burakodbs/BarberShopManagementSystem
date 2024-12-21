using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;

namespace BarberShopManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employees.Include(e => e.Salon).Include(e=>e.ExpertService);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services, "Id", "Name");
            ViewBag.Salons = _context.Salons.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            })
        .ToList();
            ViewBag.WorkDays = new List<string>
            {
                "Pazartesi", "Salı", "Çarşamba", "Perşembe",
                "Cuma", "Cumartesi", "Pazar"
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, List<string> selectedWorkDays)
        {
            if (ModelState.IsValid)
            {
                employee.WorkDays = selectedWorkDays;
                var salon = await _context.Salons.FindAsync(employee.SalonId);
                if (salon != null)
                {
                    salon.Employees.Add(employee);
                    employee.Salon = salon;  // Ensure the Employee has a reference to the Salon
                }
                var service=await _context.Services.FindAsync(employee.ExpertiseId);
                if (service != null)
                {
                    employee.ExpertService=service;
                }
                
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services, "Id", "Name");
            ViewBag.Salons = _context.Salons.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            })
        .ToList();
            ViewBag.WorkDays = new List<string>
            {
                "Pazartesi", "Salı", "Çarşamba", "Perşembe",
                "Cuma", "Cumartesi", "Pazar"
            };
            return View(employee);
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employees.FindAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Address", employee.SalonId);
        //    return View(employee);
        //}

        //// POST: Employee/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,PhoneNumber,SalonId,Expertise,WorkDays")] Employee employee)
        //{
        //    if (id != employee.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(employee);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EmployeeExists(employee.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SalonId"] = new SelectList(_context.Salons, "Id", "Address", employee.SalonId);
        //    return View(employee);
        //}

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
