using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // Müşteri adı

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Müşteri e-posta adresi

        public string PhoneNumber { get; set; } // Telefon numarası
    }
}
