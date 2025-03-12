namespace Domain.Entities;
public class VoucherLine : BaseEntity
{
    public int VoucherId { get; set; }
    public int AccountHeadId { get; set; }
    public decimal DebitAmt { get; set; }
    public decimal CreditAmt { get; set; }
    public string Narration { get; set; } = string.Empty;

}