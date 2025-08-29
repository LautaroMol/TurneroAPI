using System.ComponentModel.DataAnnotations;

namespace TurneroAPI.Application.DTOs
{
    /// <summary>
    /// DTO para actualizar la información del perfil de un usuario con el rol de Médico.
    /// </summary>
    public class MedicUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(20)]
        public string Dni { get; set; }

        [StringLength(10)]
        public string? AreaCode { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria para los médicos.")]
        [StringLength(100)]
        public string Speciality { get; set; }
    }
}
