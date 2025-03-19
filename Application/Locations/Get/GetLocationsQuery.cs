using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Get;

public class GetLocationsQuery : IRequest<List<Location>>
{
}

public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, List<Location>>
{
    private readonly IAppDbContext _context;

    public GetLocationsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Location>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Locations.ToListAsync(cancellationToken);
    }
}
