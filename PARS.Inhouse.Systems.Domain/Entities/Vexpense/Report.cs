using PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response;
using PARS.Inhouse.Systems.Shared.Enums;

namespace PARS.Inhouse.Systems.Domain.Entities.vexpense
{
    public class Report
    {
        public int Id { get; private set; }
        public int? ExternalId { get; set; }
        public string Description { get; private set; } = string.Empty;
        public ReportStatus Status { get; private set; }
        public string? ApprovalDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public string? PdfLink { get; private set; }
        public string? ExcelLink { get; private set; }
        public int? UserId { get; private set; }
        public int? DeviceId { get; private set; }
        public int? ApprovalStageId { get; private set; }
        public int? ApprovalUserId { get; private set; }
        public int? PaymentMethodId { get; private set; }
        public string? Observation { get; private set; }
        public int? PayingCompanyId { get; private set; }
        public bool On { get; private set; }
        public string? Justification { get; private set; }
        public string? CreatedAt { get; private set; }
        public string? UpdatedAt { get; private set; }
        public ExpenseContainerResponse? Expenses { get; private set; }

        public Report(string description)
        {
            SetDescription(description);
            Status = ReportStatus.ENVIADO;
        }

        public static Report Create(int? id, int? external_id, int? userId, int? deviceId, string? description, ReportStatus status, int? approvalStageId, 
            int? approvalUserId, string? approvalDate, DateTime? paymentDate, int? paymentMethodId, string? observation, int? payingCompanyId, bool on, 
            string? justification, string? pdfLink, string? excelLink, string? createdAt, string? updatedAt, ExpenseContainerResponse? expenses)
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
