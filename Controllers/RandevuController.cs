using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;
using BarberShopManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Controllers
{
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RandevuController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RandevuOlustur(RandevuOlusturViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Hizmet ve salon listelerini tekrar doldur
                return View(model);
            }

            // Giriş yapmış kullanıcıyı al
            var musteri = await _userManager.GetUserAsync(User);
            if (musteri == null)
            {
                return Challenge(); // Giriş sayfasına yönlendir
            }

            // Çakışan randevu kontrolü
            var cakisanRandevu = await _context.Randevular
                .AnyAsync(r => r.PersonelId == model.PersonelId &&
                                r.RandevuTarihi == model.RandevuTarihi);

            if (cakisanRandevu)
            {
                ModelState.AddModelError("", "Seçilen zamanda randevu dolu.");
                return View(model);
            }

            var randevu = new Randevu
            {
                PersonelId = model.PersonelId,
                RandevuTarihi = model.RandevuTarihi,
                HizmetId = model.HizmetId,
                MusteriId = musteri.Id, // Giriş yapmış kullanıcının ID'si
                Durum = RandevuDurumu.Beklemede
            };

            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction("RandevuOnay", new { id = randevu.Id });
        }

        // Kullanıcının kendi randevularını listeleme
        [Authorize]
        public async Task<IActionResult> Randevularim()
        {
            var musteri = await _userManager.GetUserAsync(User);

            var randevular = await _context.Randevular
                .Include(r => r.Personel)
                .Include(r => r.Hizmet)
                .Where(r => r.MusteriId == musteri.Id)
                .ToListAsync();

            return View(randevular);
        }
    }
}
