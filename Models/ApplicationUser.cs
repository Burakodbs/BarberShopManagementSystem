using Microsoft.AspNetCore.Identity;

namespace BarberShopManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Ek özellikler eklenebilir
        public string Ad { get; set; }
        public string Soyad { get; set; }
    }
}