using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var salons = _context.Salons.ToList();
            return View(salons);
        }

        // Yeni çalışan ekleme sayfası
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Yeni çalışan ekleme işlemi
        [HttpPost]
        public IActionResult Create(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Salons.Add(salon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salon);
        }
    }
}
