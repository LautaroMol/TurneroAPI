using TurneroAPI.Application.DTOs;

namespace TurneroAPI.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio que maneja la lógica de negocio de usuarios.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Actualiza el perfil del usuario actualmente autenticado.
        /// </summary>
        /// <param name="updateDto">Datos para actualizar.</param>
        /// <returns>El perfil del usuario actualizado.</returns>
        Task<UserProfileDto> UpdateCurrentUserProfileAsync(UserUpdateDto updateDto);

        /// <summary>
        /// Actualiza el perfil del médico actualmente autenticado.
        /// </summary>
        /// <param name="updateDto">Datos para actualizar el perfil del médico.</param>
        /// <returns>El perfil del médico actualizado.</returns>
        Task<UserProfileDto> UpdateCurrentMedicProfileAsync(MedicUpdateDto updateDto);


    }
}
