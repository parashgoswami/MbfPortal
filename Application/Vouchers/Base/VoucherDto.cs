using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Vouchers.Base;

public class VoucherDto
{
    public int Id { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public string FinYear { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Narration { get; set; } = string.Empty;
    public VoucherStatus Status { get; set; }
    public List<VoucherLineDto> VoucherLines { get; set; } = new();
    public decimal DebitAmt => VoucherLines.Sum(i => i.DebitAmt);
    public decimal CreditAmt => VoucherLines.Sum(i => i.CreditAmt);

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Voucher, VoucherDto>();
        }
    }
}

public class VoucherLineDto
{
    public int Id { get; set; }
    public int AccountHeadId { get; set; }
    public decimal DebitAmt { get; set; }
    public decimal CreditAmt { get; set; }
    public string Narration { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<VoucherLine, VoucherLineDto>();
        }
    }
}
