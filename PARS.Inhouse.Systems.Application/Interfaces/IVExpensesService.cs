using PARS.Inhouse.Systems.Application.DTOs.Response.Vexpense;

namespace PARS.Inhouse.Systems.Application.Interfaces
{
    public interface IVExpensesService
    {
        Task<List<ReportDto>> GetReportsByStatusAsync(string status, FiltrosDto filtrosDto);
    }
}
