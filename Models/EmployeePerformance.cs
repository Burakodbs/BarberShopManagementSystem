namespace BarberShopManagementSystem.Models
{
    public class EmployeePerformance
    {
        public string EmployeeName { get; set; }
        public int AppointmentCount { get; set; }
        public decimal TotalEarnings { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}
