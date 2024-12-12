using BarberShopManagementSystem.Models;

namespace BarberShopManagementSystem.ViewModels
{
    
    public class RandevuViewModel
    {
        public int SalonId { get; set; }
        public int PersonelId { get; set; }
        public int HizmetId { get; set; }
        public DateTime RandevuZamani { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriTelefon { get; set; }

        public List<Employee> Employees { get; set; }
        public List<Service> Services { get; set; }
    }
}
