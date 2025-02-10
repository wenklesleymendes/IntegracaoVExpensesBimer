using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs.Response.Vexpense;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using PARS.Inhouse.Systems.Shared.Enums;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class VExpensesService : IVExpensesService
    {
        private readonly IVExpensesApi _vExpensesApi;
        private readonly OpcoesUrls _options;
        private readonly string _tokenApiKey;

        public VExpensesService(IVExpensesApi vExpensesApi, IOptions<OpcoesUrls> options, IOptions<VexpenseTokenApiKeyConfig> tokenApiKey)
        {
            _vExpensesApi = vExpensesApi ?? throw new ArgumentNullException(nameof(vExpensesApi));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _tokenApiKey = tokenApiKey.Value.Token;
        }

        public async Task<List<ReportDto>> GetReportsByStatusAsync(string status, FiltrosDto filtrosDto, string token)
        {
            var filtrosDtoPadrao = AplicarFiltrosPadrao(filtrosDto);

            status = status.ToUpper();
            var uri = _options.VExpenseReport.Replace("{status}", status);

            var reports = await _vExpensesApi.GetReportsByStatusAsync(status, filtrosDtoPadrao, token, uri);
            return reports.Select(r => new ReportDto
            {
                Id = r.Id,
                Description = r.Description,
                Status = r.Status.ToString(),
                ApprovalDate = r.ApprovalDate,
                PdfLink = r.PdfLink,
                ExcelLink = r.ExcelLink
            }).ToList();
        }

        private string AplicarFiltrosPadrao(FiltrosDto filtrosDto)
        {
            var filtros = new FiltrosDto
            {
                Include = filtrosDto.Include != default ? filtrosDto.Include : FiltroInclude.expenses,
                Search = !string.IsNullOrEmpty(filtrosDto.Search) ? filtrosDto.Search : "",
                SearchField = filtrosDto.SearchField != default ? filtrosDto.SearchField : FiltroSearchField.approval_date_between,
                SearchJoin = filtrosDto.SearchJoin != default ? filtrosDto.SearchJoin : FiltroSearchJoin.and
            };

            return $"include={filtros.Include.ToString().ToLower()}&search={Uri.EscapeDataString(filtros.Search)}&" +
                   $"searchFields={FormatarCampo(filtros.SearchField)}&searchJoin={FormatarCampo(filtros.SearchJoin)}";
        }

        private string FormatarCampo<T>(T campo) where T : Enum
        {
            return campo.ToString().ToLower().Replace("_", ":");
        }
    }
}
