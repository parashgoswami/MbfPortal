using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Withdrawals.Get;


public class GetWithdrawalPaginationQuery : IRequest<PaginatedList<WithdrawalDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetWithdrawalPaginationQueryHandler : IRequestHandler<GetWithdrawalPaginationQuery, PaginatedList<WithdrawalDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetWithdrawalPaginationQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WithdrawalDto>> Handle(GetWithdrawalPaginationQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the paginated list of loan applications
        var query = _context.Withdrawals
            .OrderByDescending(l => l.ApplicationDate)
            .ProjectTo<WithdrawalDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<WithdrawalDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}
