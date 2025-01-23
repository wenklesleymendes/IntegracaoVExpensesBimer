namespace PARS.Inhouse.Systems.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal Value { get; set; }
        public string? Observation { get; set; }
        public string? ReceiptUrl { get; set; }
        public bool Reimbursable { get; set; }
    }
}
