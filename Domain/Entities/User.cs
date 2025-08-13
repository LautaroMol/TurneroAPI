using Microsoft.EntityFrameworkCore;

namespace TurneroAPI.Domain.Entities
{
    /// <summary>
    /// Represents a user in the system, linked to an Auth0 identity.
    /// A user can be a patient, a doctor, or both, determined by their roles.
    /// </summary>
    [Index(nameof(Auth0Id), IsUnique = true)]
    public class User : EntityBase
    {
        /// <summary>
        /// The unique identifier from Auth0. This is the primary link to the identity provider.
        /// </summary>
        public string Auth0Id { get; set; }

        /// <summary>
        /// The user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The user's email address. Used for notifications and login.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// URL for the user's profile picture.
        /// </summary>
        public string? ProfileImageUrl { get; set; }

        /// <summary>
        /// Area code for the phone number.
        /// </summary>
        public string? AreaCode { get; set; }

        /// <summary>
        /// The user's phone number (without area code). Used for future SMS reminders.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// List of roles assigned to the user (e.g., "Patient", "Doctor", "Admin").
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        // --- Doctor-Specific Fields ---

        /// <summary>
        /// The medical specialty of the user, if they have the "Doctor" role.
        /// </summary>
        public string? Specialty { get; set; }

        /// <summary>
        /// The doctor's medical license number for verification purposes.
        /// </summary>
        public string? MedicalLicense { get; set; }

        // --- Patient-Specific Fields ---

        /// <summary>
        /// The patient's date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
    }
}