using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Infrastructure.APIs;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class VExpensesService : IVExpensesService
    {
        private readonly IVExpensesApi _vExpensesApi;
        private readonly OpcoesUrls _options;

        public VExpensesService(IVExpensesApi vExpensesApi, IOptions<OpcoesUrls> options)
        {
            _vExpensesApi = vExpensesApi;
            _options = options?.Value;
        }

        public async Task<List<ReportDto>> GetReportsByStatusAsync(string status, string token)
        {
            var uri = _options.VExpenseReport.Replace("{status}", $"{status}");

            var reports = await _vExpensesApi.GetReportsByStatusAsync(status, token, uri);
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
