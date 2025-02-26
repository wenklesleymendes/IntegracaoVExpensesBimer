namespace PARS.Inhouse.Systems.Shared.DTOs.Request.Bimer
{
    public class ReauthenticateRequestDto
    {
        public required string client_id { get; set; }
        public string grant_type { get; set; } = "refresh_token";
        public required string refresh_token { get; set; }
    }
}
