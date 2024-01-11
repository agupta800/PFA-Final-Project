using System.ComponentModel.DataAnnotations;

namespace PFA.ViewModel
{
    public class LoginVM
    {

        [EmailAddress]
        [Required(ErrorMessage = "Please Enter Email")]

        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}