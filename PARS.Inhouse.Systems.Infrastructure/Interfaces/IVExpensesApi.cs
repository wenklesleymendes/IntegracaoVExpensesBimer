using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Domain.Entities.vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Vexpense;
using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface para comunicação com a API VExpenses.
    /// </summary>
    public interface IVExpensesApi
    {
        /// <summary>
        /// Busca relatórios filtrados pelo status e filtros adicionais.
        /// </summary>
        /// <param name="status">status do relatório.</param>
        /// <param name="filtros">Objeto contendo os filtros de pesquisa.</param>
        /// <returns>Lista de relatórios encontrados.</returns>
        Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusAsync(string status, string uri, string token, FiltrosDto filtros);
        Task<IReadOnlyList<ReportDto>>  BuscarRelatorioPorStatusPagoAsync(string uri, string token);
    }
}
