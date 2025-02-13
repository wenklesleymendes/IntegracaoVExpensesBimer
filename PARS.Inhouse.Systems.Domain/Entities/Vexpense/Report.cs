using PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response;
using PARS.Inhouse.Systems.Shared.Enums;

namespace PARS.Inhouse.Systems.Domain.Entities.vexpense
{
    public class Report
    {
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public int? UserId { get; set; }
        public int? DeviceId { get; set; }
        public string Description { get; set; }
        public ReportStatus Status { get; set; }
        public int? ApprovalStageId { get; set; }
        public int? ApprovalUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? Observation { get; set; }
        public int? PayingCompanyId { get; set; }
        public bool On { get; set; }
        public string Justification { get; set; }
        public string PdfLink { get; set; }
        public string ExcelLink { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ExpenseContainerResponse Expenses { get; set; }

        public Report(string description)
        {
            SetDescription(description);
            Status = ReportStatus.ENVIADO;
        }

        public static Report Create(int? id, string? external_id, int? userId, int? deviceId, string? description, ReportStatus status, int? approvalStageId, 
            int? approvalUserId, DateTime? approvalDate, DateTime? paymentDate, int? paymentMethodId, string? observation, int? payingCompanyId, bool on, 
            string? justification, string? pdfLink, string? excelLink, DateTime? createdAt, DateTime? updatedAt, ExpenseContainerResponse? expenses)
        {
            var report = new Report(description ?? "Relatório Sem Nome")
            {
                Id = id ?? 0,
                ExternalId = external_id,
                UserId = userId,
                DeviceId = deviceId,
                Description = description,
                Status = status,
                ApprovalStageId = approvalStageId,
                ApprovalUserId = approvalUserId,
                ApprovalDate = approvalDate,
                PaymentDate = paymentDate,
                PaymentMethodId = paymentMethodId,
                Observation = observation,
                PayingCompanyId = payingCompanyId,
                On = on,
                Justification = justification,
                PdfLink = pdfLink,
                ExcelLink = excelLink,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                Expenses = expenses
            };

            report.SetPdfLink(pdfLink);
            report.SetExcelLink(excelLink);
            return report;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição do relatório não pode estar vazia.");

            this.Description = description;
        }

        public void SetPdfLink(string? pdfLink)
        {
            if (pdfLink == null)
            {
                PdfLink = null;
                return;
            }

            if (!Uri.TryCreate(pdfLink, UriKind.Absolute, out Uri? validUri))
                throw new ArgumentException("O link do PDF não é válido.");

            PdfLink = validUri.ToString();
        }

        public void SetExcelLink(string? excelLink)
        {
            if (excelLink == null)
            {
                ExcelLink = null;
                return;
            }

            if (!Uri.TryCreate(excelLink, UriKind.Absolute, out Uri? validUri))
                throw new ArgumentException("O link do Excel não é válido.");

            ExcelLink = validUri.ToString();
        }
    }
}
