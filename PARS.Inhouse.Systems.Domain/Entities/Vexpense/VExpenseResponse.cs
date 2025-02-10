public class VExpenseResponse
{
    public int? id { get; set; }
    public string? description { get; set; } = string.Empty;
    public string? status { get; set; } = string.Empty;
    public string? approval_date { get; set; }
    public DateTime? payment_date { get; set; }
    public string? pdf_link { get; set; }
    public string? excel_link { get; set; }
}