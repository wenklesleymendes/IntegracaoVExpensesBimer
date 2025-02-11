public class VExpenseResponse
{
    public int? id { get; set; }
    public string? description { get; set; } = string.Empty;
    public string? status { get; set; } = string.Empty;
    public string? approval_date { get; set; }
    public string? payment_date { get; set; }
    public string? pdf_link { get; set; }
    public string? excel_link { get; set; }

    public DateTime? GetPaymentDate()
    {
        if (DateTime.TryParse(payment_date, out var parsedDate))
        {
            return parsedDate;
        }
        return null;
    }
}