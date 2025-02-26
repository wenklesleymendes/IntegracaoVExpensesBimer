using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Vexpense;
using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;

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
            _tokenApiKey = tokenApiKey?.Value?.Token ?? throw new ArgumentNullException(nameof(tokenApiKey));
        }

        public async Task<List<ReportDto>> BuscarRelatorioPorStatusAsync(string status, FiltrosDto filtrosDto)
        {
            var statusPago = status.ToUpper() == "PAGO";
            var token = _tokenApiKey;
            var filtrosDtoPadrao = AplicarFiltrosPadrao(filtrosDto);
            var uri = _options.VExpenseReport.Replace("{status}", status);

            IReadOnlyList<ReportDto>? reports;
            if (statusPago)
            {
                reports = await _vExpensesApi.BuscarRelatorioPorStatusPagoAsync(status, uri, token);
            }
            else
            {
                reports = await _vExpensesApi.BuscarRelatorioPorStatusAsync(status, uri, token, filtrosDtoPadrao);
            }

            List<ReportDto> reportsList = reports.Select(r => new ReportDto
            {
                id = r.id,
                external_id = r.external_id,
                user_id = r.user_id,
                device_id = r.device_id,
                description = r.description,
                status = r.status,
                approval_stage_id = r.approval_stage_id,
                approval_user_id = r.approval_user_id,
                approval_date = r.approval_date,
                payment_date = r.payment_date,
                payment_method_id = r.payment_method_id,
                observation = r.observation,
                paying_company_id = r.paying_company_id,
                on = r.on,
                justification = r.justification,
                pdf_link = r.pdf_link,
                excel_link = r.excel_link,
                created_at = r.created_at,
                updated_at = r.updated_at,
                expenses = MapearDtoResponse(r.expenses)
            }).ToList();

            await SaveReports(reportsList);

            return reportsList;
        }

        private FiltrosDto AplicarFiltrosPadrao(FiltrosDto filtrosDto)
        {
            return new FiltrosDto
            {
                SearchField = filtrosDto.SearchField != default ? filtrosDto.SearchField : FiltroSearchField.approval_date_between,
                Search = !string.IsNullOrEmpty(filtrosDto.Search) ? filtrosDto.Search : "",
                SearchJoin = filtrosDto.SearchJoin != default ? filtrosDto.SearchJoin : FiltroSearchJoin.and
            };
        }

        private ExpenseContainerDto MapearDtoResponse(ExpenseContainerDto expenseContainer)
        {
            return new ExpenseContainerDto
            {
                data = expenseContainer?.data?.Select(exp => new ExpenseDto
                {
                    id = exp.id,
                    user_id = exp.user_id,
                    expense_id = exp.expense_id,
                    device_id = exp.device_id,
                    integration_id = exp.integration_id,
                    external_id = exp.external_id,
                    mileage = exp.mileage,
                    date = exp.date,
                    expense_type_id = exp.expense_type_id,
                    payment_method_id = exp.payment_method_id,
                    paying_company_id = exp.paying_company_id,
                    course_id = exp.course_id,
                    reicept_url = exp.reicept_url,
                    value = exp.value,
                    title = exp.title,
                    validate = exp.validate,
                    reimbursable = exp.reimbursable,
                    observation = exp.observation,
                    rejected = exp.rejected,
                    on = exp.on,
                    mileage_value = exp.mileage_value,
                    original_currency_iso = exp.original_currency_iso,
                    exchange_rate = exp.exchange_rate,
                    converted_value = exp.converted_value,
                    converted_currency_iso = exp.converted_currency_iso,
                    created_at = exp.created_at,
                    updated_at = exp.updated_at
                }).ToList() ?? new List<ExpenseDto>()
            };
        }

        private async Task SaveReports(List<ReportDto> reportsList)
        {
            await _vExpensesApi.SaveChanges(reportsList);
        }

    }
}
