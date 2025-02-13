public class AuthResponseDto
{
    public required string access_token { get; set; }
    public required string token_type { get; set; }
    public required int expires_in { get; set; }
    public required string refresh_token { get; set; }
    public required string username { get; set; }
    public string? error { get; set; }
    public string? error_description { get; set; }
}
