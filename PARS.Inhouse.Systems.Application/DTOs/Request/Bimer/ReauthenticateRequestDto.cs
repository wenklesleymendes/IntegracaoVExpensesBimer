namespace PARS.Inhouse.Systems.Application.DTOs.Request.Bimer
{
    public class ReauthenticateRequestDto
    {
        public required string ClientId { get; set; }
        public string GrantType { get; set; } = "refresh_token";
        public required string RefreshToken { get; set; }
    }
}
