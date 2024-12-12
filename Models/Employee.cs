using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(Salon))]
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        [Required]
        [StringLength(50)]
        public string Expertise { get; set; }

        // Çalışma günlerini string olarak tutacağız
        [Required]
        [StringLength(100)]
        public string WorkDays { get; set; } // "Pazartesi,Salı,Çarşamba" gibi

        // İlişkili koleksiyonlar
        public ICollection<Service> ExpertService { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}