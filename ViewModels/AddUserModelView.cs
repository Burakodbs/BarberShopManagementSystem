namespace BarberShopManagementSystem.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddUserViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "İsim")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }
    }

}
