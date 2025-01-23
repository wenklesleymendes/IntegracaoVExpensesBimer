using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Infrastructure.APIs;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class VExpensesService : IVExpensesService
    {
        private readonly IVExpensesApi _vExpensesApi;

        public VExpensesService(IVExpensesApi vExpensesApi)
        {
            _vExpensesApi = vExpensesApi;
        }

        public async Task<List<ReportDto>> GetReportsByStatusAsync(string status, string token)
        {
            var reports = await _vExpensesApi.GetReportsByStatusAsync(status, token);
            return reports.Select(r => new ReportDto
            {
                Id = r.Id,
                Description = r.Description,
                Status = r.Status,
                ApprovalDate = r.ApprovalDate,
                PdfLink = r.PdfLink,
                ExcelLink = r.ExcelLink
            }).ToList();
        }
    }
}
