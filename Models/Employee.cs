using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Expertise { get; set; } // Uzmanlık alanı (Saç kesimi, sakal traşı vb.)

        public bool IsAvailable { get; set; } // Müsaitlik durumu

        [Required]
        public int SalonId { get; set; } // Hangi salonda çalıştığını belirtir

        public Salon Salon { get; set; } // Navigation property
    }
}
