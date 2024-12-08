
namespace BarberShopManagementSystem.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WorkingDays { get; set; } = "Monday-Saturday";
        public TimeSpan StartHour { get; set; } = new TimeSpan(8, 0, 0);
        public TimeSpan EndHour { get; set; } = new TimeSpan(20, 0, 0);

        public ICollection<Service> Services { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}