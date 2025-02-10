namespace PARS.Inhouse.Systems.Domain.Entities.vexpense
{
    public class Report
    {
        public int id { get; private set; }
        public string description { get; private set; } = string.Empty;
        public ReportStatus status { get; private set; }
        public string? approvalDate { get; private set; }
        public DateTime? paymentDate { get; private set; }
        public string? pdfLink { get; private set; }
        public string? excelLink { get; private set; }
        private readonly List<Expense> _expenses;
        public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

        public Report(string description)
        {
            SetDescription(description);
            status = ReportStatus.Pending;
            _expenses = new List<Expense>();
        }

        public static Report Create(int? id, string? description, ReportStatus status, string? approvalDate, DateTime? paymentDate, string? pdfLink, string? excelLink)
        {
            return new Report(description)
            {
                id = (int)id,
                status = status,
                approvalDate = approvalDate,
                paymentDate = paymentDate,
                pdfLink = pdfLink,
                excelLink = excelLink
            };
        }


        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição do relatório não pode estar vazia.");

            this.description = description;
        }

        public void Approve()
        {
            if (status != ReportStatus.Pending)
                throw new InvalidOperationException("Somente relatórios pendentes podem ser aprovados.");

            status = ReportStatus.Approved;
            approvalDate = DateTime.UtcNow.ToString();
        }

        public void ProcessPayment()
        {
            if (status != ReportStatus.Approved)
                throw new InvalidOperationException("Somente relatórios aprovados podem ser pagos.");

            status = ReportStatus.Paid;
            paymentDate = DateTime.UtcNow;
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

            this.pdfLink = pdfLink;
        }

        public void SetExcelLink(string excelLink)
        {
            if (!Uri.IsWellFormedUriString(excelLink, UriKind.Absolute))
                throw new ArgumentException("O link do Excel não é válido.");

            this.excelLink = excelLink;
        }
    }

    public enum ReportStatus
    {
        Pending,
        Approved,
        Paid,
        APROVADO
    }
}
