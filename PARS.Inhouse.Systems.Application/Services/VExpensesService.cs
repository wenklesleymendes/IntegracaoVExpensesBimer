using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response;
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
            var statusEnum = statusPago ? ReportStatus.Aprovado : ReportStatus.Pago;

            var reports = await _vExpensesApi.BuscarRelatorioPorStatusAsync(statusEnum, filtrosDtoPadrao);

            return reports.Select(r => new ReportDto
            {
                Id = r.Id,
                ExternalId = r.ExternalId,
                UserId = r.UserId,
                DeviceId = r.DeviceId,
                Description = r.Description,
                Status = r.Status,
                ApprovalStageId = r.ApprovalStageId,
                ApprovalUserId = r.ApprovalUserId,
                ApprovalDate = r.ApprovalDate,
                PaymentDate = r.PaymentDate,
                PaymentMethodId = r.PaymentMethodId,
                Observation = r.Observation,
                PayingCompanyId = r.PayingCompanyId,
                On = r.On,
                Justification = r.Justification,
                PdfLink = r.PdfLink,
                ExcelLink = r.ExcelLink,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                expenses = MapearDtoResponse(r.Expenses)
            }).ToList();
        }

        private FiltrosDto AplicarFiltrosPadrao(FiltrosDto filtrosDto)
        {
            return new FiltrosDto
            {
                SearchField = filtrosDto.SearchField != default ? filtrosDto.SearchField : FiltroSearchField.ApprovalDateBetween,
                Search = !string.IsNullOrEmpty(filtrosDto.Search) ? filtrosDto.Search : "",
                SearchJoin = filtrosDto.SearchJoin != default ? filtrosDto.SearchJoin : FiltroSearchJoin.And
            };
        }

        private ExpenseContainerDto MapearDtoResponse(ExpenseContainerResponse expenseContainer)
        {
            return new ExpenseContainerDto
            {
                Data = expenseContainer?.data?.Select(exp => new ExpenseDto
                {
                    Id = exp.id,
                    UserId = exp.user_id,
                    ExpenseId = exp.expense_id,
                    DeviceId = exp.device_id,
                    IntegrationId = exp.integration_id,
                    ExternalId = exp.external_id,
                    Mileage = exp.mileage,
                    Date = exp.date,
                    ExpenseTypeId = exp.expense_type_id,
                    PaymentMethodId = exp.payment_method_id,
                    PayingCompanyId = exp.paying_company_id,
                    CourseId = exp.course_id,
                    ReceiptUrl = exp.reicept_url,
                    Value = exp.value,
                    Title = exp.title,
                    Validate = exp.validate,
                    Reimbursable = exp.reimbursable,
                    Observation = exp.observation,
                    Rejected = exp.rejected,
                    On = exp.on,
                    MileageValue = exp.mileage_value,
                    OriginalCurrencyIso = exp.original_currency_iso,
                    ExchangeRate = exp.exchange_rate,
                    ConvertedValue = exp.converted_value,
                    ConvertedCurrencyIso = exp.converted_currency_iso,
                    CreatedAt = exp.created_at,
                    UpdatedAt = exp.updated_at
                }).ToList() ?? new List<ExpenseDto>()
            };
        }

        private string FormatarCampo<T>(T campo) where T : Enum
        {
            return campo.ToString().ToLower().Replace("_", ":");
        }
    }
}
