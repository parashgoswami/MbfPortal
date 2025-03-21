using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Loans.Delete;

public class DeleteLoanCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand>
{
    private readonly IAppDbContext _context;

    public DeleteLoanCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the existing loan application entity
        var loanApplication = await _context.Loans.FindAsync(request.Id);

        if (loanApplication == null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        // Remove the loan application entity
        _context.Loans.Remove(loanApplication);

        // Save the changes to the repository
        await _context.SaveChangesAsync(cancellationToken);
    }
}
