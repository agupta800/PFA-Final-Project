using System.ComponentModel.DataAnnotations;

namespace PFA.ViewModel
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; } = default!;
        [EmailAddress]
        [Required(ErrorMessage = "Please Enter Email")]

        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage ="Please Enter Confirm Password")]

        [Compare("Password",ErrorMessage = "Password and Confirm Password not matched.")]
        public string ConfirmPassword { get; set; }
    }
}
