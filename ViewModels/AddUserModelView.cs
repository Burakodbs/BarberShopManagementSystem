namespace BarberShopManagementSystem.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

}
