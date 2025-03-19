using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Locations.GetById;

public class GetLocationByIdQuery : IRequest<Location>
{
    public int Id { get; set; }
}

public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, Location>
{
    private readonly IAppDbContext _context;

    public GetLocationByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Location> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Locations.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Location), request.Id);
        }

        return entity;
    }
}
