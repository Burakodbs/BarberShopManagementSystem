using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.Employees.Include(e => e.Salon).Include(e => e.ExpertService);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {
            if(id == null) {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Appointments)
                    .ThenInclude(a => a.Service)
                .Include(e => e.Salon)
                .Include(e => e.ExpertService)
                .FirstOrDefaultAsync(e => e.Id == id);
            if(employee == null) {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create() {
            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services,"Id","Name");
            ViewBag.Salons = _context.Salons.Select(s => new SelectListItem {
                Value = s.Id.ToString(),
                Text = s.Name
            })
        .ToList();
            ViewBag.WorkDays = new List<string>
            {
                "Monday", "Tuesday", "Wednesday", "Thursday",
                "Friday", "Saturday"
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee,List<string> selectedWorkDays) {
            if(ModelState.IsValid) {
                employee.WorkDays = selectedWorkDays;
                var salon = await _context.Salons.FindAsync(employee.SalonId);
                if(salon != null) {
                    salon.Employees.Add(employee);
                    employee.Salon = salon;
                }
                var service = await _context.Services.FindAsync(employee.ExpertiseId);
                if(service != null) {
                    employee.ExpertService = service;
                }

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services,"Id","Name");
            ViewBag.Salons = _context.Salons.Select(s => new SelectListItem {
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

        public async Task<IActionResult> Edit(int? id) {
            if(id == null) {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Salon)
                .Include(e => e.ExpertService)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(employee == null) {
                return NotFound();
            }

            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services,"Id","Name",employee.ExpertiseId);
            ViewBag.Salons = _context.Salons.Select(s => new SelectListItem {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = s.Id == employee.SalonId
            }).ToList();

            ViewBag.WorkDays = new List<string>
    {
        "Monday", "Tuesday", "Wednesday", "Thursday",
        "Friday", "Saturday"
    };

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Employee employee,List<string> selectedWorkDays) {
            if(id != employee.Id) {
                return NotFound();
            }

            if(ModelState.IsValid) {
                try {
                    var existingEmployee = await _context.Employees
                        .Include(e => e.Salon)
                        .Include(e => e.ExpertService)
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if(existingEmployee == null) {
                        return NotFound();
                    }

                    existingEmployee.WorkDays = selectedWorkDays;
                    existingEmployee.IsOnVacation = employee.IsOnVacation;
                    existingEmployee.Name = employee.Name;
                    existingEmployee.SalonId = employee.SalonId;
                    existingEmployee.ExpertiseId = employee.ExpertiseId;

                    _context.Update(existingEmployee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateConcurrencyException) {
                    if(!EmployeeExists(employee.Id)) {
                        return NotFound();
                    }
                    throw;
                }
            }

            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services,"Id","Name",employee.ExpertiseId);
            ViewBag.Salons = _context.Salons.Select(s => new SelectListItem {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = s.Id == employee.SalonId
            }).ToList();

            ViewBag.WorkDays = new List<string>
    {
        "Pazartesi", "Salı", "Çarşamba", "Perşembe",
        "Cuma", "Cumartesi", "Pazar"
    };

            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id) {
            if(id == null) {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(employee == null) {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var employee = await _context.Employees.FindAsync(id);
            if(employee != null) {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id) {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
