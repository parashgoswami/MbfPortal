using Domain.Enums;
using MediatR;

namespace Application.Vouchers.Base;

public abstract class BaseVoucherCommand : IRequest<int>
{
    public string VoucherNo { get; set; } = string.Empty;
    public string FinYear { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Narration { get; set; } = string.Empty;
    public VoucherStatus Status { get; set; }
    public List<BaseVoucherLineDto> VoucherLines { get; set; } = new();
}

public class BaseVoucherLineDto
{
    public int? Id { get; set; }
    public int AccountHeadId { get; set; }
    public decimal DebitAmt { get; set; }
    public decimal CreditAmt { get; set; }
    public string Narration { get; set; } = string.Empty;
}
