using Domain.Enums;
using MediatR;
namespace Application.Loans;
public abstract class BaseLoanApplicationCommand : IRequest<int>
{
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }   
    public ApplicationStatus Status { get; set; } = ApplicationStatus.NEW;
    public string Remarks { get; set; } = string.Empty;
}
