public class AuthRequestDto
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string GrantType { get; set; } = "password";
    public string Username { get; set; }
    public string Password { get; set; }
}