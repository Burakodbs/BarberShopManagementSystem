using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class EmployeeStatsController : Controller {
        private readonly ApplicationDbContext _context;

        public EmployeeStatsController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var stats = await _context.Employees
                .GroupJoin(
                    _context.Appointments,
                    e => e.Id,
                    a => a.EmployeeId,
                    (employee,appointments) => new EmployeeAppointmentStatsViewModel {
                        EmployeeId = employee.Id,
                        EmployeeName = employee.Name,
                        TotalAppointments = appointments.Count(),
                        TotalEarnings = (decimal)appointments.Sum(a => a.Price),
                        CompletedAppointments = appointments.Count(a => a.IsConfirmed == true),
                        AverageAppointmentPrice = (decimal)(appointments.Any() ? appointments.Average(a => a.Price) : 0),
                    })
                .ToListAsync();

            return View(stats);
        }

        public async Task<JsonResult> GetChartData() {
            var data = await _context.Employees
                .GroupJoin(
                    _context.Appointments,
                    e => e.Id,
                    a => a.EmployeeId,
                    (employee,appointments) => new {
                        employeeName = employee.Name,
                        totalAppointments = appointments.Count(),
                        completedAppointments = appointments.Count(a => a.IsConfirmed == true),
                        totalEarnings = appointments.Sum(a => a.Price)
                    })
                .ToListAsync();

            return Json(data);
        }
    }
}

