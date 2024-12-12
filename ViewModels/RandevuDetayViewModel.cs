using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarberShopManagementSystem.ViewModels
{
    public class RandevuDetayViewModel
    {
        public int SalonId { get; set; }
        public string SalonAdi { get; set; }
        public List<SelectListItem> MusaitPersoneller { get; set; }
        public List<SelectListItem> MusaitHizmetler { get; set; }
    }
}
