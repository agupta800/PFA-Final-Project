using Microsoft.AspNetCore.Identity;

namespace PFA.Models
{
    public class UserViewModel
    {
        public string IdentityId { get; set; } // Corrected property name
        public IdentityUser User { get; set; } = default!;
        public IdentityUserRole<string> UserRole { get; set; } = default!;// Updated property for IdentityUserRole
        public string RoleName { get; set; } = default!;
    }
}
