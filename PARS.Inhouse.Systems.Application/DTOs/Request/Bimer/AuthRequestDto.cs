public class AuthRequestDto
{
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string grant_type { get; set; } = "password";
    public string username { get; set; }
    public string password { get; set; }
    public string nonce { get; set; }
}
