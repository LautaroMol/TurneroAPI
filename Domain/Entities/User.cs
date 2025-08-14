using System.ComponentModel.DataAnnotations;

namespace TurneroAPI.Domain.Entities
{
    public class User : EntityBase
    {
        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public string Dni { get; set; } 

        public string AreaCode { get; set; }

        /// <summary>
        /// Email único del usuario.
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña hasheada (nunca guardes la contraseña en texto plano).
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Roles del usuario (ej. "Paciente", "Médico", "Admin").
        /// Guardar como lista para soportar múltiples roles.
        /// </summary>
        public ICollection<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Especialidad del médico (si aplica).
        /// </summary>
        public string? Speciality { get; set; }

        /// <summary>
        /// Teléfono de contacto.
        /// </summary>
        public string? PhoneNumber { get; set; }

        // Navegación inversa
        public virtual ICollection<Turno> TurnosComoPaciente { get; set; } = new List<Turno>();
        public virtual ICollection<Turno> TurnosComoMedico { get; set; } = new List<Turno>();
    }
}