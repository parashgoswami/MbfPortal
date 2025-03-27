using MediatR;
using AutoMapper;
using Application.Common.Interfaces;
using AutoMapper.QueryableExtensions;

namespace Application.FiscalYears.Get;

public class GetFiscalYearByPaginationQuery : IRequest<PaginatedList<FiscalYearDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetFiscalYearByPaginationQueryHandler : IRequestHandler<GetFiscalYearByPaginationQuery, PaginatedList<FiscalYearDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetFiscalYearByPaginationQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FiscalYearDto>> Handle(GetFiscalYearByPaginationQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the paginated list of loan applications
        var query = _context.FiscalYears
            .OrderByDescending(l => l.FinYear)
            .ProjectTo<FiscalYearDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<FiscalYearDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}
