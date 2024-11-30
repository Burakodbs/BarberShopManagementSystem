
using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.ViewModels
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime randevuTarihi)
            {
                // Şu andan en az 1 saat sonrası için randevu alınabilir
                if (randevuTarihi > DateTime.Now.AddHours(1))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage ?? "Randevu tarihi geçerli değil.");
        }
    }
}