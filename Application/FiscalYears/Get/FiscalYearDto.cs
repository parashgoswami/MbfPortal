using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.FiscalYears.Get;

public class FiscalYearDto
{
    public string FinYear { get; set; } = string.Empty;
    public decimal DepositInterest { get; set; }
    public decimal LoanInterest { get; set; }
    public FiscalYearStatus Status { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<FiscalYear, FiscalYearDto>();
        }
    }
}
