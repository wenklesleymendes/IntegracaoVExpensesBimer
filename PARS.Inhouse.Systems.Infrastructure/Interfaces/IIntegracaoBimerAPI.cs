namespace PARS.Inhouse.Systems.Infrastructure.Interfaces
{
    public interface IIntegracaoBimerAPI
    {
        Task<string> CriarTituloAPagar(string bimerRequest, string uri, string token);
        Task<string> AuthenticateAsync(FormUrlEncodedContent content, string uri);
        Task<string> ReauthenticateAsync(FormUrlEncodedContent content, string uri);
    }
}
