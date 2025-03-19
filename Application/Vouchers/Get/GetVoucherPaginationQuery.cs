using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vouchers.Get;

public class GetVoucherPaginationQuery : IRequest<PaginatedList<Voucher>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetVoucherPaginationQueryHandler : IRequestHandler<GetVouchersQuery, PaginatedList<Voucher>>
{
    private readonly IAppDbContext _context;

    public GetVoucherPaginationQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Voucher>> Handle(GetVouchersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Vouchers
            .Include(v => v.VoucherLines)
            .AsNoTracking();

        return await PaginatedList<Voucher>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}