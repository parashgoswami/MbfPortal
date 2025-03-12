namespace Domain.Entities;
public class AccountBalance : BaseEntity
{
    public int AccountHeadId { get; set; }
    public decimal debitBalance { get; set; }
    public decimal creditBalance { get; set; }
}
