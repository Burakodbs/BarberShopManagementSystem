namespace BarberShopManagementSystem.Models
{
    public class Personel
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
        public List<Hizmet> UzmanlikAlanlari { get; set; }
        public List<Randevu> Randevular { get; set; }
    }
}