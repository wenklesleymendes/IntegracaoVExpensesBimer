using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.DTOs.Response.Vexpense
{

    public class ExpenseContainerDto
    {
        [JsonPropertyName("data")]
        public List<ExpenseDto> Data { get; set; } = new();
    }

    public class ExpenseDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }

        [JsonPropertyName("expense_id")]
        public int? ExpenseId { get; set; }

        [JsonPropertyName("device_id")]
        public int? DeviceId { get; set; }

        [JsonPropertyName("integration_id")]
        public int? IntegrationId { get; set; }

        [JsonPropertyName("external_id")]
        public int? ExternalId { get; set; }

        [JsonPropertyName("mileage")]
        public decimal? Mileage { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("expense_type_id")]
        public int? ExpenseTypeId { get; set; }

        [JsonPropertyName("payment_method_id")]
        public int? PaymentMethodId { get; set; }

        [JsonPropertyName("paying_company_id")]
        public int? PayingCompanyId { get; set; }

        [JsonPropertyName("course_id")]
        public int? CourseId { get; set; }

        [JsonPropertyName("receipt_url")]
        public string? ReceiptUrl { get; set; }

        [JsonPropertyName("value")]
        public int? Value { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; } = string.Empty;

        [JsonPropertyName("validate")]
        public string? Validate { get; set; } = string.Empty;

        [JsonPropertyName("reimbursable")]
        public bool? Reimbursable { get; set; }

        [JsonPropertyName("observation")]
        public string? Observation { get; set; } = string.Empty;

        [JsonPropertyName("rejected")]
        public int? Rejected { get; set; }

        [JsonPropertyName("on")]
        public bool? On { get; set; }

        [JsonPropertyName("mileage_value")]
        public decimal? MileageValue { get; set; }

        [JsonPropertyName("original_currency_iso")]
        public string? OriginalCurrencyIso { get; set; } = "BRL";

        [JsonPropertyName("exchange_rate")]
        public decimal? ExchangeRate { get; set; } = 1;

        [JsonPropertyName("converted_value")]
        public decimal? ConvertedValue { get; set; }

        [JsonPropertyName("converted_currency_iso")]
        public string? ConvertedCurrencyIso { get; set; } = "BRL";

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ExpenseDto()
        {
        }

        public static ExpenseDto Create(int? id, int? userId, int? expenseId, int value, string title, int expenseTypeId, int paymentMethodId,
                                     int payingCompanyId, bool reimbursable, DateTime? date, string? observation, string? receiptUrl,
                                     decimal exchangeRate, string originalCurrencyIso, string convertedCurrencyIso)
        {
            var expense = new ExpenseDto()
            {
                Id = id,
                UserId = userId,
                ExpenseId = expenseId,
                Value = value,
                Title = title,
                ExpenseTypeId = expenseTypeId,
                PaymentMethodId = paymentMethodId,
                PayingCompanyId = payingCompanyId,
                Reimbursable = reimbursable,
                Date = date ?? DateTime.UtcNow,
                Observation = observation,
                ReceiptUrl = receiptUrl,
                ExchangeRate = exchangeRate,
                OriginalCurrencyIso = originalCurrencyIso,
                ConvertedCurrencyIso = convertedCurrencyIso
            };


            return expense;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título da despesa não pode estar vazio.");
            Title = title;
        }

        public void SetValue(int? value)
        {
            if (value <= 0)
                throw new ArgumentException("O valor da despesa deve ser maior que zero.");
            Value = value;
        }

        public void SetObservation(string? observation)
        {
            Observation = observation ?? string.Empty;
        }

        public void SetReceiptUrl(string? url)
        {
            if (url != null && !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new ArgumentException("A URL do recibo não é válida.");
            ReceiptUrl = url;
        }

        public void SetExchangeRate(decimal exchangeRate, string originalCurrency, string convertedCurrency)
        {
            if (exchangeRate <= 0)
                throw new ArgumentException("A taxa de câmbio deve ser maior que zero.");

            ExchangeRate = exchangeRate;
            OriginalCurrencyIso = originalCurrency;
            ConvertedCurrencyIso = convertedCurrency;
            ConvertedValue = Value * exchangeRate;
        }

        public static ExpenseDto Create(int? id, int? userId, int? expenseId, int? deviceId,
            int? integrationId, int? externalId, decimal? mileage, DateTime? date,
            int? expenseTypeId, int? paymentMethodId, int? payingCompanyId,
            int? courseId, string? receiptUrl, int? value, string? title, string? validate,
            bool? reimbursable, string? observation, int? rejected, bool? on,
            decimal? mileageValue, string? originalCurrencyIso, decimal? exchangeRate,
            decimal? convertedValue, string? convertedCurrencyIso, DateTime? createdAt,
            DateTime? updatedAt)
        {
            var expense = new ExpenseDto()
            {
                Id = id,
                UserId = userId,
                ExpenseId = expenseId,
                Value = value,
                Title = title,
                ExpenseTypeId = expenseTypeId,
                PaymentMethodId = paymentMethodId,
                PayingCompanyId = payingCompanyId,
                Reimbursable = reimbursable,
                Date = date ?? DateTime.UtcNow,
                Observation = observation,
                ReceiptUrl = receiptUrl,
                ExchangeRate = exchangeRate,
                OriginalCurrencyIso = originalCurrencyIso,
                ConvertedCurrencyIso = convertedCurrencyIso,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };


            return expense;
        }
    }
}
