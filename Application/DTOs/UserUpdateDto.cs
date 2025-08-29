using System.ComponentModel.DataAnnotations;

namespace TurneroAPI.Application.DTOs
{
    /// <summary>
    /// DTO para actualizar la información del perfil de un usuario.
    /// Contiene los campos que el usuario puede modificar.
    /// </summary>
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(20, ErrorMessage = "El DNI no puede tener más de 20 caracteres.")]
        public string Dni { get; set; }

        [StringLength(10, ErrorMessage = "El código de área no puede tener más de 10 caracteres.")]
        public string? AreaCode { get; set; }

        [Phone(ErrorMessage = "El formato del número de teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El número de teléfono no puede tener más de 20 caracteres.")]
        public string? PhoneNumber { get; set; }
    }
}
