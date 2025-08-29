using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TurneroAPI.Application.Interfaces;
using TurneroAPI.Domain.Entities;

namespace TurneroAPI.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private const string Namespace = "https://api.turnero.com/";
        private const string RolesClaimType = Namespace + "roles";
        private const string DesiredRoleClaimType = Namespace + "desired_role"; // El nuevo claim
        private const string EmailClaimType = Namespace + "email";
        private const string GivenNameClaimType = Namespace + "given_name";
        private const string FamilyNameClaimType = Namespace + "family_name";

        public AuthService(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetOrCreateUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new UnauthorizedAccessException("No se pudo acceder al contexto HTTP.");
            var claims = httpContext.User.Claims;

            var auth0Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("No se encontró el identificador del usuario (sub).");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityId == auth0Id);

            if (user == null)
            {
                // El usuario no existe, lo creamos.
                var desiredRole = claims.FirstOrDefault(c => c.Type == DesiredRoleClaimType)?.Value;

                user = new User
                {
                    IdentityId = auth0Id,
                    Email = claims.FirstOrDefault(c => c.Type == EmailClaimType)?.Value ?? string.Empty,
                    FirstName = claims.FirstOrDefault(c => c.Type == GivenNameClaimType)?.Value ?? "Usuario",
                    LastName = claims.FirstOrDefault(c => c.Type == FamilyNameClaimType)?.Value ?? string.Empty,
                    // Si hay un rol deseado en el token, lo usamos. Si no, 'Paciente' por defecto.
                    Roles = desiredRole ?? "Paciente", 
                    Dni = "Pendiente",
                    AreaCode = string.Empty
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                // El usuario ya existe, sincronizamos los roles por si cambiaron en Auth0 (ej. un admin lo ascendió).
                var rolesFromToken = claims.Where(c => c.Type == RolesClaimType).Select(c => c.Value);
                var rolesString = rolesFromToken.Any() ? string.Join(",", rolesFromToken) : user.Roles; // No sobrescribir con vacío

                if (user.Roles != rolesString)
                {
                    user.Roles = rolesString;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync(CancellationToken.None);
                }
            }

            return user;
        }
    }
}

