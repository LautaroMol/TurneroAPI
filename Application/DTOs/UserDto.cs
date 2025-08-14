namespace TurneroAPI.Application.DTOs;

/// <summary>
/// Data Transfer Object for a User.
/// Used to transfer user data between layers, especially for API responses.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Unique identifier for the User.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
