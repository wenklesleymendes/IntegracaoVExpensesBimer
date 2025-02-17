using PARS.Inhouse.Systems.Domain.Entities.vexpense;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces
{
    public interface IVExpensesApi
    {
        Task<List<Report>> BuscarRelatorioPorStatusAsync(string status, string filtros, string token, string uri, bool statusPago);
    }
}