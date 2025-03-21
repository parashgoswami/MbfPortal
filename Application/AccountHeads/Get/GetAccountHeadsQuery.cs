using Application.Common.Interfaces;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AccountHeads.Get;

public class GetAccountHeadsQuery : IRequest<List<AccountHeadDto>>
{
}

public class GetAccountHeadsQueryHandler : IRequestHandler<GetAccountHeadsQuery, List<AccountHeadDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountHeadsQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountHeadDto>> Handle(GetAccountHeadsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountHeads
            .ProjectTo<AccountHeadDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
