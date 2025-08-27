namespace TurneroAPI.Application.DTOs
{
    public class UserProfileDto
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }
}
