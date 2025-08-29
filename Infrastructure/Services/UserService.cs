using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TurneroAPI.Application.DTOs;
using TurneroAPI.Application.Interfaces;
using TurneroAPI.Domain.Entities;

namespace TurneroAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserProfileDto> UpdateCurrentUserProfileAsync(UserUpdateDto updateDto)
        {
            var user = await GetCurrentUserAsync();

            // Actualizar los campos del usuario
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Dni = updateDto.Dni;
            user.AreaCode = updateDto.AreaCode ?? user.AreaCode;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;

            await _context.SaveChangesAsync(CancellationToken.None);

            return MapUserToProfileDto(user);
        }

        public async Task<UserProfileDto> UpdateCurrentMedicProfileAsync(MedicUpdateDto updateDto)
        {
            var user = await GetCurrentUserAsync();

            // Doble verificación: aunque el endpoint está protegido por rol, el servicio también debería verificarlo.
            if (!user.Roles.Contains("Médico"))
            {
                throw new UnauthorizedAccessException("Este usuario no tiene el rol de Médico.");
            }

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Dni = updateDto.Dni;
            user.AreaCode = updateDto.AreaCode ?? user.AreaCode;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
            user.Speciality = updateDto.Speciality;

            await _context.SaveChangesAsync(CancellationToken.None);

            return MapUserToProfileDto(user);
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new UnauthorizedAccessException("No se pudo acceder al contexto HTTP.");
            var identityId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("No se encontró el identificador del usuario (sub).");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityId == identityId);
            return user ?? throw new KeyNotFoundException("Usuario no encontrado.");
        }

        private UserProfileDto MapUserToProfileDto(User user)
        {
            return new UserProfileDto
            {
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = user.Roles,
                Dni = user.Dni,
                PhoneNumber = $"{user.AreaCode} {user.PhoneNumber}".Trim(),
                Speciality = user.Speciality ?? string.Empty
            };
        }
    }
}
