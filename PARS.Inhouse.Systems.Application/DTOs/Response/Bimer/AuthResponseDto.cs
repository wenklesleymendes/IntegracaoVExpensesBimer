public class AuthResponseDto
{
    public required string AccessToken { get; set; }
    public required string TokenType { get; set; }
    public required int ExpiresIn { get; set; }
    public required string RefreshToken { get; set; }
    public required string Username { get; set; }
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
}
