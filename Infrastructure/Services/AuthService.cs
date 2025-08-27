using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TurneroAPI.Application.DTOs;
using TurneroAPI.Application.Interfaces;
using TurneroAPI.Domain.Entities;

namespace TurneroAPI.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<User> GetOrCreateUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new UnauthorizedAccessException("No se pudo acceder al contexto HTTP.");

            var auth0Id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("No se encontró el identificador del usuario (sub).");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityId == auth0Id);

            // Si el usuario no existe, o si existe pero está incompleto, lo creamos/actualizamos.
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                var userInfo = await GetUserInfoFromAuth0Async();

                if (user == null)
                {
                    // Caso 1: El usuario no existe, lo creamos.
                    user = new User
                    {
                        IdentityId = auth0Id,
                        Email = userInfo.Email,
                        FirstName = userInfo.GivenName,
                        LastName = userInfo.FamilyName,
                        Roles = "Paciente", // rol por defecto
                        Dni = "Pendiente",
                        AreaCode = string.Empty
                    };
                    _context.Users.Add(user);
                }
                else
                {
                    // Caso 2: El usuario existe pero está incompleto, lo actualizamos.
                    user.Email = userInfo.Email;
                    user.FirstName = userInfo.GivenName;
                    user.LastName = userInfo.FamilyName;
                    _context.Users.Update(user);
                }

                await _context.SaveChangesAsync(CancellationToken.None);
            }

            return user;
        }

        private async Task<Auth0UserDto> GetUserInfoFromAuth0Async()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var accessToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new UnauthorizedAccessException("No se proporcionó un token de acceso.");
            }

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://dev-dt0bwdavp7b00fif.us.auth0.com/userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("No se pudo obtener la información del usuario desde Auth0.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<Auth0UserDto>(content);

            return userInfo ?? throw new Exception("La información del usuario recibida de Auth0 es inválida.");
        }
    }
}
