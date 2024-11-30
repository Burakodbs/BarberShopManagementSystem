namespace BarberShopManagementSystem.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public DateTime RandevuTarihi { get; set; }

        public int PersonelId { get; set; }
        public Personel Personel { get; set; }

        // Identity kullanıyorsanız string olacak
        public string MusteriId { get; set; }
        public ApplicationUser Musteri { get; set; }

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }

        public RandevuDurumu Durum { get; set; }
    }
}

namespace BarberShopManagementSystem
{
    public enum RandevuDurumu
    {
        Beklemede = 1,
        Onaylandı = 2,
        Reddedildi = 3,
        Tamamlandı = 4,
        Iptal = 5
    }
}