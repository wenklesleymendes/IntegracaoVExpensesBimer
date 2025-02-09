public class AuthRequestDto
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public string GrantType { get; set; } = "password";
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Nonce { get; set; }
}
