using System.ComponentModel.DataAnnotations;

namespace PFA.Models
{
    public class FileVM


    {

        [Key]
        public int Id { get; set; }


        public string? Image { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(10, ErrorMessage = "First Name cannot exceed 10 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(10, ErrorMessage = "Last Name cannot exceed 10 characters")]
        public string LastName { get; set; }
       
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }


        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Invalid mobile number.")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]

        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Invalid mobile number.")]
        public string Phone { get; set; }



        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        public string City { get; set; }

        public string Qualification { get; set; }

        [Required(ErrorMessage = "Job Category is required")]
        public string JobCategory { get; set; }

        public string Position { get; set; }

        public string KeySkills { get; set; }




        public bool HasExperience { get; set; }



        public string? TrackingId { get; set; } // New property for tracking ID

        public bool IsApproved { get; set; } // New property for approval status
        public bool IsDisApproved { get; set; } // New property for Disapproval status






    }

}
