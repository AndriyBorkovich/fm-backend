namespace FootballManager.Application.Models.Identity;

public class AuthResponse
{
    public bool IsAuthSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
}
