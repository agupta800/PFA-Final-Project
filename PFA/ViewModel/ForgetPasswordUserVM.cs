using System.ComponentModel.DataAnnotations;

namespace PFA.ViewModel
{
    public class ForgetPasswordUserVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }

        public string Password { get; set; }
        [Compare(nameof(Password))]

        public string ConfirmPassword { get; set; }





    }
}
