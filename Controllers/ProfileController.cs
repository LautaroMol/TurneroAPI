using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroAPI.Application.Interfaces;

namespace TurneroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // <-- Atributo clave: Solo usuarios autenticados pueden acceder.
    public class ProfileController : ControllerBase
    {
        private readonly IAuthService _authService;

        public ProfileController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Devuelve el perfil del usuario autenticado desde la base de datos local.
        /// Si el usuario no existe, lo crea a partir de la información del token de Auth0.
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                // Esta llamada ejecuta la lógica principal de nuestro servicio.
                var userProfile = await _authService.GetOrCreateUserAsync();

                // Devolvemos el perfil del usuario (para la prueba está bien, en producción
                // podrías usar un DTO para no exponer la entidad del dominio).
                return Ok(userProfile);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Captura de otros posibles errores
                return StatusCode(500, new { message = "Ocurrió un error en el servidor.", details = ex.Message });
            }
        }
    }
}
