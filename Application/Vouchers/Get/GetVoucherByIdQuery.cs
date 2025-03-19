using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vouchers.GetById;

public class GetVoucherByIdQuery : IRequest<Voucher>
{
    public int Id { get; set; }
}

public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, Voucher>
{
    private readonly IAppDbContext _context;

    public GetVoucherByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Voucher> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var voucher = await _context.Vouchers
            .Include(v => v.VoucherLines)
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (voucher == null)
        {
            throw new NotFoundException(nameof(Voucher), request.Id);
        }

        return voucher;
    }
}

