using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Specialty { get; set; } // Uzmanlık alanı (ör. Saç Kesimi, Sakal Traşı)

        public bool IsAvailable { get; set; } // Müsaitlik durumu

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } // Çalışma başlangıç saati

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; } // Çalışma bitiş saati
    }
}
