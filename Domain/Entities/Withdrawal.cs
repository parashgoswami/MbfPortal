using Domain.Enums;

namespace Domain.Entities;
public class Withdrawal : BaseEntity
{
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }
    public decimal SanctionedAmt { get; set; }
    public DateTime? SanctionDate { get; set; }
    public WithdrawalStatus Status { get; set; } 
    public string? Remarks { get; set; } 
}
