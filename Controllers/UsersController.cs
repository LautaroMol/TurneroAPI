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
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // TODO: Re-implement with ASP.NET Identity
        /*
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
        */
    }
}
