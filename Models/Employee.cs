namespace BarberShopManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SalonId { get; set; }
        public TimeSpan AvailableFrom { get; set; }
        public TimeSpan AvailableTo { get; set; }

        public ICollection<EmployeeSpecialty> Specialties { get; set; }
        public Salon Salon { get; set; }
    }

}