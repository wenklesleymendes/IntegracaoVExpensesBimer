using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Vexpense;

namespace PARS.Inhouse.Systems.Application.Interfaces
{
    public interface IVExpensesService
    {
        Task<string> AlterarStatus(int id, AtualizaStatusDto requestDto);
        Task<List<ReportDto>> BuscarRelatorioPorStatusAsync(string status, FiltrosDto filtrosDto);
    }
}
