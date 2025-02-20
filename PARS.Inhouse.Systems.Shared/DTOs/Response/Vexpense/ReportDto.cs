using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PARS.Inhouse.Systems.Shared.DTOs.Response.Vexpense
{
    public class ReportDto
    {
        public int? id { get; set; }
        public string? external_id { get; set; }
        public int? user_id { get; set; }
        public int? device_id { get; set; }
        public string description { get; set; } = string.Empty;
        public ReportStatus status { get; set; }
        public int? approval_stage_id { get; set; }
        public int? approval_user_id { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? approval_date { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? payment_date { get; set; }
        public int? payment_method_id { get; set; }
        public string? observation { get; set; }
        public int? paying_company_id { get; set; }
        public bool on { get; set; }
        public string? justification { get; set; }
        public string? pdf_link { get; set; }
        public string? excel_link { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? created_at { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? updated_at { get; set; }
        public ExpenseContainerDto? expenses { get; set; }

        public ReportDto()
        {
        }

        public static ReportDto Create(int? id, string? externalId, int? userId, int? deviceId, string? description, ReportStatus status,
            int? approvalStageId, int? approvalUserId, DateTime? approvalDate, DateTime? paymentDate, int? paymentMethodId,
            string? observation, int? payingCompanyId, bool on, string? justification, string? pdfLink, string? excelLink,
            DateTime? createdAt, DateTime? updatedAt, ExpenseContainerDto? expenses)
        {
            var report = new ReportDto
            {
                id = id ?? 0,
                external_id = externalId,
                user_id = userId,
                device_id = deviceId,
                description = description,
                status = status,
                approval_stage_id = approvalStageId,
                approval_user_id = approvalUserId,
                approval_date = approvalDate,
                payment_date = paymentDate,
                payment_method_id = paymentMethodId,
                observation = observation,
                paying_company_id = payingCompanyId,
                on = on,
                justification = justification,
                created_at = createdAt,
                updated_at = updatedAt,
                expenses = expenses ?? new ExpenseContainerDto() // Evita nulos
            };

            report.SetPdfLink(pdfLink);
            report.SetExcelLink(excelLink);

            return report;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição do relatório não pode estar vazia.");

            this.description = description;
        }

        public void SetPdfLink(string? pdfLink)
        {
            if (!string.IsNullOrEmpty(pdfLink) && !Uri.TryCreate(pdfLink, UriKind.Absolute, out _))
                throw new ArgumentException("O link do PDF não é válido.");

            pdf_link = pdfLink;
        }

        public void SetExcelLink(string? excelLink)
        {
            if (!string.IsNullOrEmpty(excelLink) && !Uri.TryCreate(excelLink, UriKind.Absolute, out _))
                throw new ArgumentException("O link do Excel não é válido.");

            excel_link = excelLink;
        }

        public void UpdateStatus(ReportStatus newStatus)
        {
            if (newStatus == status)
                throw new InvalidOperationException("O novo status deve ser diferente do status atual.");

            status = newStatus;
        }
    }

    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String &&
                DateTime.TryParse(reader.GetString(), out DateTime date))
            {
                return date;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                writer.WriteNullValue();
        }
    }
}
