namespace Domain.Entities;

public class AccountLedger : BaseEntity
{
    public string FinYear { get; set; } = string.Empty;
    public int AccountHeadId { get; set; }
    public DateTime PostingDate { get; set; }
    public decimal DebitAmt {get; set; }
    public decimal CreditAmt { get; set; }
    public string Narration { get; set; } = string.Empty;
    public int VoucherId { get; set; }

}
