using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.FiscalYears.Open;

public class OpenFiscalYearCommand : IRequest
{
    public int Id { get; set; }
}

public class OpenFiscalYearCommandHandler : IRequestHandler<OpenFiscalYearCommand>
{
    private readonly IAppDbContext _context;

    public OpenFiscalYearCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(OpenFiscalYearCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FiscalYears.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(FiscalYears), request.Id);
        }

        if (entity.Status == FiscalYearStatus.Draft)
        {
            throw new BadRequestException("Can edit only draft fiscal year record.");
        }

        var anyOpenFiscalYear = await _context.FiscalYears
            .AnyAsync(f => f.Status == FiscalYearStatus.Open, cancellationToken);

        if (anyOpenFiscalYear)
        {
            throw new BadRequestException("There is already an open fiscal year.");
        }

        entity.Open();

        await _context.SaveChangesAsync(cancellationToken);
    }
}


