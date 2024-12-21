using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string Surname { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        public string PhoneNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Bir salon seçmeniz gerekiyor")]
        [Display(Name = "Salon")]
        [ForeignKey(nameof(Salon))]
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur")]
        [ForeignKey(nameof(ExpertService))]
        public int ExpertiseId { get; set; }
        public Service? ExpertService { get; set; }

        [Required(ErrorMessage = "En az bir çalışma günü seçmeniz gerekiyor")]
        public List<string> WorkDays { get; set; } = new List<string>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}