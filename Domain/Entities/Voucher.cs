using Domain.Enums;

namespace Domain.Entities;
public class Voucher : BaseEntity
{
    public string VoucherNo { get; set; } = string.Empty;
    public string FinYear { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Narration { get; set; } = string.Empty;
    public VoucherStatus Status { get; set; } = VoucherStatus.DRAFT;
    public IReadOnlyList<VoucherLine> VoucherLines { get; set; } = new List<VoucherLine>();

    public decimal DebitAmt => VoucherLines.Sum(line => line.DebitAmt);
    public decimal CreditAmt => VoucherLines.Sum(line => line.CreditAmt);

    public void AddVoucherLine(VoucherLine voucherLine)
    {
        if (Status != VoucherStatus.CREATED)
        {
            throw new InvalidOperationException("Cannot add lines to a posted or cancelled voucher.");
        }
        VoucherLines.Append(voucherLine);
    }

    public bool IsBalanced()
    {
        return DebitAmt == CreditAmt;
    }
}
