using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Salon))]
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [ForeignKey(nameof(Service))]
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        [Required]
        public DateTime RandevuZamani { get; set; }

        [StringLength(50)]
        public string? CustomerName { get; set; }

        public decimal? Price { get; set; }

        public int? Duration { get; set; }


        [Phone]
        [StringLength(20)]
        public string? CustomerPhone { get; set; }

        public bool IsConfirmed { get; set; }

    }
}