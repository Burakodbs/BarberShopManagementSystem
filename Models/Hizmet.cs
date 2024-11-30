using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShopManagementSystem.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string Ad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Ucret { get; set; }
        public int Sure { get; set; } // Dakika
    }
}