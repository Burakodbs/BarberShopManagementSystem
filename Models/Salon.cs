namespace BarberShopManagementSystem.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
    }
}

