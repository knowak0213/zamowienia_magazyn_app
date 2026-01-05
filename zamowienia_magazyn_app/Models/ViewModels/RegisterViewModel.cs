using System.ComponentModel.DataAnnotations;

namespace zamowienia_magazyn_app.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Niepoprawny numer telefonu.")]
        [Display(Name = "Numer telefonu")]
        public string? PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Adres nie może być dłuższy niż 200 znaków.")]
        [Display(Name = "Miasto/Adres")]
        public string? Address { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
