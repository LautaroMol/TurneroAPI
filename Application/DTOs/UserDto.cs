namespace TurneroAPI.Application.DTOs;

/// <summary>
/// Data Transfer Object for a User.
/// Used to transfer user data between layers, especially for API responses.
/// </summary>
public class UserDto
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Roles { get; set; } = string.Empty;
}

