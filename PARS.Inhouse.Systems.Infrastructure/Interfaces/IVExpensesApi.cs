using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Vexpense;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface para comunicação com a API VExpenses.
    /// </summary>
    public interface IVExpensesApi
    {
        Task<string> AlterarRelatorio(string uri, string token, AtualizaStatusDto requestDto);

        /// <summary>
        /// Busca relatórios filtrados pelo status e filtros adicionais.
        /// </summary>
        /// <param name="status">status do relatório.</param>
        /// <param name="filtros">Objeto contendo os filtros de pesquisa.</param>
        /// <returns>Lista de relatórios encontrados.</returns>
        Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusAsync(string status, string uri, string token, FiltrosDto filtros);
        Task<IReadOnlyList<ReportDto>>  BuscarRelatorioPorStatusPagoAsync(string status, string uri, string token);
    }
}
