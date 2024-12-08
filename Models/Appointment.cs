namespace BarberShopManagementSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public string CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
        public bool IsApproved { get; set; }

        public Salon Salon { get; set; }
        public Employee Employee { get; set; }
        public Service Service { get; set; }
    }

}
