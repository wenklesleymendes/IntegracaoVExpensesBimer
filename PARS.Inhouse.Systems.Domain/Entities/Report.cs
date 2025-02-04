namespace PARS.Inhouse.Systems.Domain.Entities
{
    public class Report
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PdfLink { get; set; }
        public string? ExcelLink { get; set; }
        public List<Expense>? Expenses { get; set; }
    }
}
