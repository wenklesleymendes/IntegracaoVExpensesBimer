namespace PARS.Inhouse.Systems.Domain.Entities
{
    public class Report
    {
        public int Id { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public ReportStatus Status { get; private set; }
        public DateTime? ApprovalDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public string? PdfLink { get; private set; }
        public string? ExcelLink { get; private set; }
        private readonly List<Expense> _expenses;
        public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

        public Report(string description)
        {
            SetDescription(description);
            Status = ReportStatus.Pending;
            _expenses = new List<Expense>();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição do relatório não pode estar vazia.");

            Description = description;
        }

        public void Approve()
        {
            if (Status != ReportStatus.Pending)
                throw new InvalidOperationException("Somente relatórios pendentes podem ser aprovados.");

            Status = ReportStatus.Approved;
            ApprovalDate = DateTime.UtcNow;
        }

        public void ProcessPayment()
        {
            if (Status != ReportStatus.Approved)
                throw new InvalidOperationException("Somente relatórios aprovados podem ser pagos.");

            Status = ReportStatus.Paid;
            PaymentDate = DateTime.UtcNow;
        }

        public void AddExpense(Expense expense)
        {
            if (expense == null)
                throw new ArgumentException("A despesa não pode ser nula.");

            _expenses.Add(expense);
        }

        public void RemoveExpense(Expense expense)
        {
            if (expense == null || !_expenses.Contains(expense))
                throw new ArgumentException("A despesa não está no relatório.");

            _expenses.Remove(expense);
        }

        public void SetPdfLink(string pdfLink)
        {
            if (!Uri.IsWellFormedUriString(pdfLink, UriKind.Absolute))
                throw new ArgumentException("O link do PDF não é válido.");

            PdfLink = pdfLink;
        }

        public void SetExcelLink(string excelLink)
        {
            if (!Uri.IsWellFormedUriString(excelLink, UriKind.Absolute))
                throw new ArgumentException("O link do Excel não é válido.");

            ExcelLink = excelLink;
        }
    }

    public enum ReportStatus
    {
        Pending,
        Approved,
        Paid
    }
}
