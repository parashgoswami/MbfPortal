using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Withdrawals.Get;
public class WithdrawalDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }
    public decimal SanctionedAmt { get; set; }
    public DateTime? SanctionDate { get; set; }
    public LoanStatus Status { get; set; }
    public string Remarks { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Withdrawal, WithdrawalDto>();
        }
    }
}

