using Microsoft.AspNetCore.Identity;

namespace TurneroAPI.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Dni { get; set; } 
        public string AreaCode { get; set; }
        public string? Speciality { get; set; }
    }
}
