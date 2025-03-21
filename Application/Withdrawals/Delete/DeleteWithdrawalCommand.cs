using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Withdrawals.Delete;

public class DeleteWithdrawalCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteWithdrawalCommandHandler : IRequestHandler<DeleteWithdrawalCommand>
{
    private readonly IAppDbContext _context;

    public DeleteWithdrawalCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteWithdrawalCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the existing withdrwal entity
        var withdrawal = await _context.Withdrawals.FindAsync(request.Id);

        if (withdrawal == null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        // Remove the loan application entity
        _context.Withdrawals.Remove(withdrawal);

        // Save the changes to the repository
        await _context.SaveChangesAsync(cancellationToken);
    }
}

