using Microsoft.AspNetCore.Mvc;
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

        // Randevuları listele
        [Authorize]
        public IActionResult Index()
        {
            var appointments = _context.Appointments
                .OrderBy(a => a.Date)
                .ToList();
            return View(appointments);
        }

        // Yeni randevu oluşturma sayfası
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Customers = _context.Customers.ToList();
            return View();
        }


        // Yeni randevu oluşturma işlemi
        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Çakışma kontrolü
                bool isConflict = _context.Appointments
                    .Any(a => a.EmployeeId == appointment.EmployeeId &&
                              a.Date == appointment.Date);

                if (isConflict)
                {
                    ModelState.AddModelError("", "Bu çalışanın belirtilen tarihte başka bir randevusu var.");
                    ViewBag.Employees = _context.Employees.ToList();
                    return View(appointment);
                }

                // Çakışma yoksa randevuyu kaydet
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Employees = _context.Employees.ToList();
            return View(appointment);
        }

    }
}
