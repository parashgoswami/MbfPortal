using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.AccountHeads.Delete;

public class DeleteAccountHeadCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteAccountHeadCommandHandler : IRequestHandler<DeleteAccountHeadCommand>
{
    private readonly IAppDbContext _context;

    public DeleteAccountHeadCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAccountHeadCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AccountHeads.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AccountHead), request.Id);
        }

        _context.AccountHeads.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
