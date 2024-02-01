using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Security.Policy;

namespace PFA.JobModel
{
    public class JobPostModel
    {
        [Key]

        [Column("JobID")]
        public int JobID { get; set; }



        [Display(Name = "Please Enter The Company Name")]
        [Required(ErrorMessage = "Company Name is Required")]
        public string companyname { get; set; } = default!;

       
        [DisplayName("Enter the Job Title")]
        [Required(ErrorMessage = "Job Title is Required")]

        public string jobTitle { get; set; } = default!;

        
        [DisplayName("Enter the ApplyLink")]
        [Required(ErrorMessage = "ApplyLink is Required")]

        public string applylink { get; set; } = default!;

        
        [DisplayName("Enter the Description in Text")]
        [Required(ErrorMessage = "Description is Required")]

        public string txtDescription { get; set; } = default!;

        
        [DisplayName("Enter the Date")]
        [Required(ErrorMessage = "Date is Required")]

        public DateTime inputDate { get; set; } = default!;

        //companyImageUrl
        
        [DisplayName("Enter the Image Url")]
        [Required(ErrorMessage = "Company Image is Required")]

        public string companyImageUrl { get; set; } = default!;

    }
}
