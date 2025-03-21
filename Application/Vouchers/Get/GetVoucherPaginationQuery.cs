using Application.Common.Interfaces;
using Application.Vouchers.Base;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vouchers.Get;

public class GetVoucherPaginationQuery : IRequest<PaginatedList<VoucherDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetVoucherPaginationQueryHandler : IRequestHandler<GetVoucherPaginationQuery, PaginatedList<VoucherDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetVoucherPaginationQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<VoucherDto>> Handle(GetVoucherPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Vouchers
            .Include(v => v.VoucherLines)
            .AsNoTracking()
            .ProjectTo<VoucherDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<VoucherDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}



