using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } // Randevu tarihi

        [Required]
        public int EmployeeId { get; set; } // Hangi çalışanla

        public Employee Employee { get; set; } // Navigation property

        [Required]
        public string Service { get; set; } // Yapılacak işlem (ör: Saç Kesimi)

        [Required]
        public decimal Price { get; set; } // Ücret

        [Required]
        public int CustomerId { get; set; } // Randevuyu alan müşteri

        public Customer Customer { get; set; } // Navigation property

    }
}
