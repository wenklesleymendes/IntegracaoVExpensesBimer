namespace PARS.Inhouse.Systems.Infrastructure.Interfaces
{
    public interface IIntegracaoBimerAPI
    {
        Task<string> CriarTituloAPagar(string bimerRequest, string uri, string token);
    }
}
