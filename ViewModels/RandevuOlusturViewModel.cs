using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.ViewModels
{
    public class RandevuOlusturViewModel
    {
        [Required(ErrorMessage = "Hizmet seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir hizmet seçiniz.")]
        public int HizmetId { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir personel seçiniz.")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [FutureDate(ErrorMessage = "Randevu tarihi gelecekte olmalıdır.")]
        public DateTime RandevuTarihi { get; set; }
    }
}
