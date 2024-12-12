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
        public Salon Salon { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey(nameof(Service))]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Required]
        public DateTime RandevuZamani { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string CustomerPhone { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

    }
    public enum AppointmentStatus
    {
        Waiting = 1,
        Approved = 2,
        Completed = 3,
        Canceled = 4
    }
}