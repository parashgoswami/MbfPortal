namespace Domain.Entities;
public class Voucher : BaseEntity
{
    public string VoucherNo { get; set; } = string.Empty;
    public string FinYear { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal DebitAmt { get; set; }
    public decimal CreditAmt { get; set; }
    public string Narration { get; set; } = string.Empty;    
    public IReadOnlyList<VoucherLine> VoucherLines { get; set; } = new List<VoucherLine>();
}
