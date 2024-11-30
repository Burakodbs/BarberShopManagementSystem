using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        // Yeni müşteri ekleme sayfası
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Appointments(int customerId)
        {
            var appointments = _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .OrderBy(a => a.Date)
                .ToList();

            var customer = _context.Customers.Find(customerId);

            ViewBag.CustomerName = customer != null ? customer.Name : "Bilinmiyor";
            return View(appointments);
        }


        // Yeni müşteri ekleme işlemi
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
    }
}
