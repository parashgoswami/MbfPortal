using Domain.Enums;

namespace Domain.Entities;
public class FiscalYear : BaseEntity
{
    public string FinYear { get; set; } = string.Empty;
    public decimal DepositInterest{ get; set; }
    public decimal LoanInterest { get; set; }
    public FiscalYearStatus Status { get; private set; } = FiscalYearStatus.Draft;

    public void Open()
    {
        if (Status == FiscalYearStatus.Draft)
        {
            Status = FiscalYearStatus.Open;
        }
    }
}
