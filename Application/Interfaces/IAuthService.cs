using TurneroAPI.Domain.Entities;

namespace TurneroAPI.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de autenticación que maneja la lógica de usuario con proveedores de identidad externos.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Obtiene el usuario de la base de datos local que corresponde al usuario autenticado.
        /// Si el usuario no existe en la base de datos local, lo crea.
        /// </summary>
        /// <returns>La entidad User de la base de datos local.</returns>
        Task<User> GetOrCreateUserAsync();


    }
}
