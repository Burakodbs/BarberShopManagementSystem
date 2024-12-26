using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context,UserManager<IdentityUser> userManager) {
            _context = context;
            _userManager = userManager;

        }

        public IActionResult Dashboard() {
            return View();
        }

        public IActionResult ManageUsers() {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId) {
            var user = await _userManager.FindByIdAsync(userId);

            if(user != null) {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("ManageUsers");
        }

        public IActionResult AddUser() {
            return View(new AddUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model) {
            if(ModelState.IsValid) {
                var user = new IdentityUser { UserName = model.Email,Email = model.Email };
                var result = await _userManager.CreateAsync(user,model.Password);

                if(result.Succeeded) {
                    return RedirectToAction("ManageUsers");
                }

                foreach(var error in result.Errors) {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> PendingAppointments() {
            var pendingAppointments = await _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Where(a => !a.IsConfirmed)
                .ToListAsync();

            return View(pendingAppointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAppointment(int id) {
            var appointment = await _context.Appointments.FindAsync(id);
            if(appointment == null) {
                return NotFound();
            }

            appointment.IsConfirmed = true;
            _context.Update(appointment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Appointment has been confirmed successfully.";
            return RedirectToAction(nameof(PendingAppointments));
        }

    }
}
