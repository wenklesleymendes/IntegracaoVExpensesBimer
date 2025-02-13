using PARS.Inhouse.Systems.Shared.Enums;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response
{
    public class VExpenseResponse
    {
        public int? id { get; set; }
        public int? external_id { get; set; }
        public int? user_id { get; set; }
        public int? device_id { get; set; }
        public string description { get; set; } = string.Empty;
        public ReportStatus status { get; set; }
        public int? approval_stage_id { get; set; }
        public int? approval_user_id { get; set; }
        public string? approval_date { get; set; }
        public DateTime? payment_date { get; set; }
        public int? payment_method_id { get; set; }
        public string? observation { get; set; }
        public int? paying_company_id { get; set; }
        public bool on { get; set; }
        public string? justification { get; set; }
        public string? pdf_link { get; set; }
        public string? excel_link { get; set; }
        public string? created_at { get; set; }
        public string? updated_at { get; set; }
        public ExpenseContainerResponse? expenses { get; set; }

        public VExpenseResponse() { }

        public DateTime? GetPaymentDate()
        {
            if (!string.IsNullOrWhiteSpace(payment_date.ToString()) && DateTime.TryParse(payment_date.ToString(), out var parsedDate))
            {
                return parsedDate;
            }
            return null;
        }


        public void SetPdfLink(string? pdfLink)
        {
            if (pdfLink == null)
            {
                pdfLink = null;
                return;
            }

            if (!Uri.TryCreate(pdfLink, UriKind.Absolute, out Uri? validUri))
                throw new ArgumentException("O link do PDF não é válido.");

            pdfLink = validUri.ToString();
        }

        public void SetExcelLink(string? excelLink)
        {
            if (excelLink == null)
            {
                excelLink = null;
                return;
            }

            if (!Uri.TryCreate(excelLink, UriKind.Absolute, out Uri? validUri))
                throw new ArgumentException("O link do Excel não é válido.");

            excelLink = validUri.ToString();
        }
    }
}