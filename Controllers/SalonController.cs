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
    }
}
