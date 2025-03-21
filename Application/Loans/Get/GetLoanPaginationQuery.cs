using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Loans.Get;

public class GetLoansWithPaginationQuery : IRequest<PaginatedList<LoanDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetLoansWithPaginationQueryHandler : IRequestHandler<GetLoansWithPaginationQuery, PaginatedList<LoanDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetLoansWithPaginationQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LoanDto>> Handle(GetLoansWithPaginationQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the paginated list of loan applications
        var query = _context.Loans
            .OrderByDescending(l => l.ApplicationDate)
            .ProjectTo<LoanDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<LoanDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}
