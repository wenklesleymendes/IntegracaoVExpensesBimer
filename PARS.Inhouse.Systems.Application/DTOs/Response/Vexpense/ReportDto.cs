namespace PARS.Inhouse.Systems.Application.DTOs.Response.Vexpense
{
    public class ReportDto
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? PdfLink { get; set; }
        public string? ExcelLink { get; set; }
    }
}
