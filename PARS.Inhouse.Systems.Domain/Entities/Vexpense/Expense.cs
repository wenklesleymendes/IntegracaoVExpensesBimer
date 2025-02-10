namespace PARS.Inhouse.Systems.Domain.Entities.vexpense
{
    public class Expense
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public decimal Value { get; private set; }
        public string? Observation { get; private set; }
        public string? ReceiptUrl { get; private set; }
        public bool Reimbursable { get; private set; }

        public Expense(string title, decimal value, bool reimbursable, string? observation = null, string? receiptUrl = null)
        {
            SetTitle(title);
            SetValue(value);
            Reimbursable = reimbursable;
            Observation = observation;
            ReceiptUrl = receiptUrl;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título da despesa não pode estar vazio.");

            Title = title;
        }

        public void SetValue(decimal value)
        {
            if (value <= 0)
                throw new ArgumentException("O valor da despesa deve ser maior que zero.");

            Value = value;
        }

        public void SetReceiptUrl(string? url)
        {
            if (url != null && !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new ArgumentException("A URL do recibo não é válida.");

            ReceiptUrl = url;
        }
    }
}
