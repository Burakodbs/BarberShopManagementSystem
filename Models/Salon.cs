namespace BarberShopManagementSystem.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public TimeSpan AcilisSaati { get; set; }
        public TimeSpan KapanisSaati { get; set; }
        public List<Personel> Personeller { get; set; }
        public List<Hizmet> SunulanHizmetler { get; set; }
    }
}
