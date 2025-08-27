using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TurneroAPI.Application.Interfaces;
using TurneroAPI.Domain.Entities;

namespace TurneroAPI.Infrastructure.Services
{
    /// <summary>
    /// Este servicio se encarga de la sincronización de usuarios entre Auth0 y la base de datos local.
    /// Su propósito es asegurar que exista un registro de usuario local para cada usuario que se autentica a través de Auth0.
    /// </summary>
    public class UserSyncService
    {
        private readonly IApplicationDbContext _context;

        public UserSyncService(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Sincroniza un usuario a partir de los claims obtenidos del token de Auth0.
        /// Este método se llamaría típicamente después de que un usuario inicie sesión por primera vez
        /// o en cada inicio de sesión para mantener los datos actualizados.
        /// </summary>
        /// <param name="claims">Los claims del principal de usuario (HttpContext.User.Claims) después de una autenticación exitosa.</param>
        /// <returns>El usuario local (ya sea existente o recién creado).</returns>
        public async Task<User> SyncUserAsync(IEnumerable<Claim> claims)
        {
            // El claim "sub" es el identificador de usuario único y estable de Auth0. Es el más importante.
            var identityId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(identityId))
            {
                throw new InvalidOperationException("El token del usuario no contiene el claim 'sub' (NameIdentifier).");
            }

            // Busca al usuario en nuestra base de datos usando el ID de Auth0.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityId == identityId);

            // Si el usuario no existe en nuestra BD, lo creamos.
            if (user == null)
            {
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
                
                // Verificamos si ya existe un usuario con ese email pero con otro IdentityId (caso poco común pero posible).
                var existingByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingByEmail != null)
                {
                    // Aquí podrías manejar una lógica de fusión de cuentas o lanzar un error.
                    // Por ahora, lanzamos una excepción para señalar el conflicto.
                    throw new InvalidOperationException($"Un usuario con el email {email} ya existe pero con un IdentityId diferente.");
                }

                user = new User
                {
                    IdentityId = identityId,
                    Email = email,
                    FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "Usuario",
                    LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? string.Empty,
                    // Asignar un rol por defecto. En el futuro, los roles podrían venir como un claim desde Auth0.
                    Roles = "Paciente"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(new CancellationToken());
            }
            
            // Si el usuario ya existía, podrías agregar lógica aquí para actualizar sus datos
            // (ej. si cambió su nombre en el perfil de Auth0).

            return user;
        }
    }
}
