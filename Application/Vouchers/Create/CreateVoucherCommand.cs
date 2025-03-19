using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Vouchers.Common;
using Domain.Entities;
using MediatR;

namespace Application.Vouchers.Create;

public class CreateVoucherCommand : BaseVoucherCommand, IRequest<int>
{
}
public class CreateVoucherCommandValidator : BaseVoucherCommandValidator<CreateVoucherCommand>
{
}

public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, int>
{
    private readonly IAppDbContext _context;

    public CreateVoucherCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
    {
        var voucher = new Voucher
        {
            VoucherNo = request.VoucherNo,
            FinYear = request.FinYear,
            Date = request.Date,
            Narration = request.Narration,
            Status = request.Status
        };

        foreach (var line in request.VoucherLines)
        {
            voucher.AddVoucherLine(new VoucherLine(line.AccountHeadId, line.DebitAmt, line.CreditAmt, line.Narration));
        }

        if (!voucher.IsBalanced())
        {
            throw new BadRequestException("The voucher is not balanced.");
        }

        _context.Vouchers.Add(voucher);
        await _context.SaveChangesAsync(cancellationToken);
        return voucher.Id;
    }
}

