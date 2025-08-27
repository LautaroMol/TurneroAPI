using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroAPI.Application.DTOs;
using TurneroAPI.Application.Interfaces;

namespace TurneroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
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
            var userProfile = await _authService.GetOrCreateUserAsync();

            var dto = new UserProfileDto
            {
                Email = userProfile.Email,
                FullName = $"{userProfile.FirstName} {userProfile.LastName}",
                Roles = userProfile.Roles,
                Dni = userProfile.Dni,
                PhoneNumber = userProfile.PhoneNumber ?? string.Empty,
                Speciality = userProfile.Speciality ?? string.Empty
            };

            return Ok(dto);
        }
    }
}
