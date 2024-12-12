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
    public class SalonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Salon
        public async Task<IActionResult> Index()
        {
            return View(await _context.Salons.ToListAsync());
        }

        // GET: Salon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        // GET: Salon/Create
        public IActionResult Create()
        {
            ViewBag.ServiceList = _context.Services.Select(s => new
            {
                s.Id,
                s.Name
            }).ToList();
            return View();
        }

        // POST: Salon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salon salon, int[] selectedServices)
        {
            if (ModelState.IsValid)
            {
                // Add selected services
                salon.Services = _context.Services.Where(s => selectedServices.Contains(s.Id)).ToList();
                _context.Add(salon);
                await _context.SaveChangesAsync();

                // Add the new Salon to the selected Services
                var services = _context.Services.Where(s => selectedServices.Contains(s.Id)).ToList();
                foreach (var service in services)
                {
                    service.Salons.Add(salon);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ServiceList = _context.Services.Select(s => new
            {
                s.Id,
                s.Name
            }).ToList();
            return View(salon);
        }
        // GET: Salon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        // POST: Salon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salon = await _context.Salons.FindAsync(id);
            if (salon != null)
            {
                _context.Salons.Remove(salon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalonExists(int id)
        {
            return _context.Salons.Any(e => e.Id == id);
        }
    }
}
