using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Policy;

namespace PFA.JobModel
{
    public class JobPostModel
    {
        [Key] 
        [Required]
       
        public int JobID { get; set; }


        [Required]
        [DisplayName("Enter the Company Name")]
        public string companyname { get; set; } = default!;

        [Required]
        [DisplayName("Enter the Job Title")]
        public string JobTitle { get; set; } = default!;

        [Required]
        [DisplayName("Enter the ApplyLink")]
        public string Applylink { get; set; }= default!;

        [Required]
        [DisplayName("Enter the Description in Text")]
        public string TxtDescription { get; set; } = default!;

        [Required]
        [DisplayName("Enter the Date")]
        public DateTime inputDate { get; set; } = default!;

        //companyImageUrl
        [Required]
        [DisplayName("Enter the Image Url")]
        public string companyImageUrl { get; set; } = default!;

    }
}
