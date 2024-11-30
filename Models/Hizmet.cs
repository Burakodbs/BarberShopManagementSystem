namespace BarberShopManagementSystem.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public decimal Ucret { get; set; }
        public int Sure { get; set; } // Dakika
    }
}