
namespace BarberShopManagementSystem.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string ServiceName { get; set; }
        public int Duration { get; set; } // in minutes
        public decimal Price { get; set; }

        public Salon Salon { get; set; }
    }
}