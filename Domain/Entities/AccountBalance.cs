namespace Domain.Entities;
public class AccountBalance : BaseEntity
{
    public int AccountHeadId { get; set; }
    public decimal DebitBalance { get; set; }
    public decimal CreditBalance { get; set; }
}
