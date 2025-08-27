using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TurneroAPI.Application.Interfaces;
using TurneroAPI.Domain.Entities;

namespace TurneroAPI.Infrastructure.Services
{
    // Este es el servicio de autenticación que interactúa con Auth0.
    // Su responsabilidad principal es asegurar que exista un usuario en tu base de datos local
    // que corresponda al usuario que ha iniciado sesión a través de Auth0.
    public class AuthService : IAuthService
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Obtiene el usuario de la base de datos local que corresponde al usuario autenticado por Auth0.
        /// Si el usuario no existe en la base de datos local, lo crea.
        /// </summary>
        /// <returns>La entidad User de la base de datos local.</returns>
        public async Task<User> GetOrCreateUserAsync()
        {
            // 1. OBTENER EL ID DE AUTH0 DEL TOKEN
            // Cuando un usuario inicia sesión en el frontend con Auth0, Auth0 emite un JWT (JSON Web Token).
            // El frontend envía este token en el encabezado de autorización de cada solicitud a tu API.
            // El middleware de autenticación de ASP.NET Core valida el token y extrae la información (claims).
            // El 'NameIdentifier' es el 'sub' (subject) de Auth0, que es el ID de usuario único.
            var auth0Id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(auth0Id))
            {
                // Esto no debería ocurrir si el endpoint está protegido con [Authorize]
                throw new UnauthorizedAccessException("No se pudo encontrar el identificador del usuario en el token.");
            }

            // 2. BUSCAR AL USUARIO EN TU BASE DE DATOS
            // Usamos el ID de Auth0 para ver si ya tenemos un registro para este usuario.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityId == auth0Id);

            // 3. SI EL USUARIO NO EXISTE, CREARLO
            if (user == null)
            {
                // Extraemos información adicional del token para crear un perfil de usuario básico.
                var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
                var firstName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
                var lastName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

                // Creamos la nueva entidad User.
                user = new User
                {
                    IdentityId = auth0Id, // Este es el vínculo crucial con Auth0.
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    // Puedes asignar roles por defecto o dejarlos vacíos para asignarlos después.
                    Roles = "Paciente",
                    // El DNI y otros campos obligatorios deberán ser completados por el usuario más tarde.
                    // Aquí los inicializamos para que la entidad sea válida.
                    Dni = "DNI Pendiente",
                    AreaCode = string.Empty
                };

                // Guardamos el nuevo usuario en la base de datos.
                _context.Users.Add(user);
                await _context.SaveChangesAsync(new CancellationToken());
            }

            // 4. DEVOLVER EL USUARIO (EXISTENTE O RECIÉN CREADO)
            return user;
        }
    }
}