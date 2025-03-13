namespace Domain.Entities;
public class AccountBalance : BaseEntity
{
    public int AccountHeadId { get; set; }
    public string FinYear { get; set; } = string.Empty;
    public decimal DebitBalance { get; set; }
    public decimal CreditBalance { get; set; }
}
