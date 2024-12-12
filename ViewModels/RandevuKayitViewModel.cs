using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.ViewModels
{
    public class RandevuKayitViewModel
    {
        [Required]
        public int SalonId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public int HizmetId { get; set; }

        [Required]
        public DateTime RandevuZamani { get; set; }

        [Required]
        [StringLength(50)]
        public string MusteriAdi { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string MusteriTelefon { get; set; }
    }
}
