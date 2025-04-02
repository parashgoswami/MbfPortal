using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Vouchers.Base;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var voucherNo = await GetVoucherNo(request.FinYear);
        var voucher = new Voucher
        {
            VoucherNo = voucherNo,
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

    private async Task<string> GetVoucherNo(string finYear)
    {
        var lastVoucher = await _context.Vouchers
            .Where(v => v.FinYear == finYear)
            .OrderByDescending(v => v.VoucherNo)
            .FirstOrDefaultAsync();

        int sequenceNumber = 1;
        if (lastVoucher != null)
        {
            var lastSequence = int.Parse(lastVoucher.VoucherNo.Substring(6));
            sequenceNumber = lastSequence + 1;
        }

        var finYearPart = finYear.Substring(2, 2) + finYear.Substring(5, 2);
        return $"{finYearPart}{sequenceNumber:D6}";
    }
}

