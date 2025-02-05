public class AuthResponseDto
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
    public string username { get; set; }
    public string error { get; set; }
    public string errorDescription { get; set; }
}