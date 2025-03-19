using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Locations.Delete;

public class DeleteLocationCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand>
{
    private readonly IAppDbContext _context;

    public DeleteLocationCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Locations.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Location), request.Id);
        }

        _context.Locations.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
