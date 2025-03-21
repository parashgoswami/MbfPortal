using Domain.Enums;

namespace Domain.Entities;
public class Loan : BaseEntity
{
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }
    public decimal SanctionedAmt { get; set; }
    public DateTime? SanctionDate { get; set; }   
    public LoanStatus Status { get; set; } = LoanStatus.NEW;
    public string? Remarks { get; set; }
    public Member? Member { get; set; }
}
