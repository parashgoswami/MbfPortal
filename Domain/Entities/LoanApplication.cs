namespace Domain.Entities;
public class LoanApplication : BaseEntity
{
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmount { get; set; }
    public decimal SanctionedAmount { get; set; }
    public DateTime SanctionDate { get; set; }   
    public bool IsApproved { get; set; }
    public string Remarks { get; set; } = string.Empty;
}
