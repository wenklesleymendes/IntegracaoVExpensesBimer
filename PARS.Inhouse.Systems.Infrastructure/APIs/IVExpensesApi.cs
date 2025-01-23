using PARS.Inhouse.Systems.Domain.Entities;

namespace PARS.Inhouse.Systems.Infrastructure.APIs
{
    public interface IVExpensesApi
    {
        Task<List<Report>> GetReportsByStatusAsync(string status, string token);
    }
}
