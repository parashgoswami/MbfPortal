using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Vouchers.Base;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vouchers.Edit;

public class EditVoucherCommand : BaseVoucherCommand, IRequest
{
    public int Id { get; set; }
}

public class EditVoucherCommandValidator : BaseVoucherCommandValidator<EditVoucherCommand>
{
}

public class EditVoucherCommandHandler : IRequestHandler<EditVoucherCommand>
{
    private readonly IAppDbContext _context;

    public EditVoucherCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditVoucherCommand request, CancellationToken cancellationToken)
    {
        var voucher = await _context.Vouchers
            .Include(v => v.VoucherLines)
            .FirstOrDefaultAsync(v => v.Id == request.Id);

        if (voucher == null)
        {
            throw new NotFoundException(nameof(Voucher), request.Id);
        }

        if(voucher.Status != VoucherStatus.DRAFT) 
        {
            throw new BadRequestException("Can edit a draft voucher only.");
        }

        voucher.FinYear = request.FinYear;
        voucher.Date = request.Date;
        voucher.Narration = request.Narration;
        voucher.Status = request.Status;

        // Update voucher lines
        foreach (var line in request.VoucherLines)
        {
            var existingLine = voucher.VoucherLines.FirstOrDefault(vl => vl.Id == line.Id);
            if (existingLine != null)
            {
                existingLine.SetAccountHeadId(line.AccountHeadId);
                existingLine.SetDebitAmt(line.DebitAmt);
                existingLine.SetCreditAmt(line.CreditAmt);
                existingLine.SetNarration(line.Narration);
            }
            else
            {
                voucher.AddVoucherLine(new VoucherLine(line.AccountHeadId, line.DebitAmt, line.CreditAmt, line.Narration));
            }
        }

        if (!voucher.IsBalanced())
        {
            throw new BadRequestException("The voucher is not balanced.");
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

