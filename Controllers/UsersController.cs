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
        private readonly IUserService _userService;

        public UsersController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
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
                PhoneNumber = $"{userProfile.AreaCode} {userProfile.PhoneNumber}".Trim(),
                Speciality = userProfile.Speciality ?? string.Empty
            };

            return Ok(dto);
        }

        /// <summary>
        /// Actualiza el perfil del usuario autenticado (para pacientes o roles sin perfil específico).
        /// </summary>
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProfile = await _userService.UpdateCurrentUserProfileAsync(updateDto);

            return Ok(updatedProfile);
        }

        /// <summary>
        /// Actualiza el perfil del usuario autenticado con rol de Médico.
        /// </summary>
        [HttpPut("me/medic-profile")]
        [Authorize(Roles = "Médico")]
        public async Task<IActionResult> UpdateMyMedicProfile([FromBody] MedicUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProfile = await _userService.UpdateCurrentMedicProfileAsync(updateDto);

            return Ok(updatedProfile);
        }
    }
}
