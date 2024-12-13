using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Salons.ToListAsync());
        }

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

        public IActionResult Create()
        {
            ViewBag.ServiceList = _context.Services.Select(s => new
            {
                s.Id,
                s.Name
            }).ToList();
            return View();
        }

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
