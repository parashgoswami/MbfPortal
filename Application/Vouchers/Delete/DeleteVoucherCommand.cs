using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vouchers.Delete;

public class DeleteVoucherCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteVoucherCommandHandler : IRequestHandler<DeleteVoucherCommand>
{
    private readonly IAppDbContext _context;

    public DeleteVoucherCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
    {
        var voucher = await _context.Vouchers
            .Include(v => v.VoucherLines)
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (voucher == null)
        {
            throw new NotFoundException(nameof(Voucher), request.Id);
        }

        _context.Vouchers.Remove(voucher);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

