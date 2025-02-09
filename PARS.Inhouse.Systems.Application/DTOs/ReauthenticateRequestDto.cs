namespace PARS.Inhouse.Systems.Application.DTOs
{
    public class ReauthenticateRequestDto
    {
        public required string ClientId { get; set; }
        public string GrantType { get; set; } = "refresh_token";
        public required string RefreshToken { get; set; }
    }
}
