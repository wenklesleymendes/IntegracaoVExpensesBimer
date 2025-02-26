public class AtualizaStatusDto
{
    public string payment_date { get; set; }
    public string comment { get; set; }

    public AtualizaStatusDto() { }

    public AtualizaStatusDto(DateTime paymentDate, string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("comment is required.", nameof(comment));

        payment_date = paymentDate.ToString("yyyy-MM-dd HH:mm:ss");
        this.comment = comment;
    }
}