using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Locations.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.GetById;

public class GetLocationByIdQuery : IRequest<LocationDto>
{
    public int Id { get; set; }
}

public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationDto>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetLocationByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LocationDto> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var locationDto = await _context.Locations
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(i => i.Id == request.Id);

        if (locationDto == null)
        {
            throw new NotFoundException(nameof(Location), request.Id);
        }

        return locationDto;
    }
}
