using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.FiscalYears.Commands;

public class EditFinYearCommand : IRequest
{
    public int Id { get; set; }
    public string FinYear { get; set; } = string.Empty;
    public decimal DepositInterest { get; set; }
    public decimal LoanInterest { get; set; }
}

public class EditFinYearValidator : AbstractValidator<EditFinYearCommand>
{
    public EditFinYearValidator()
    {
        RuleFor(x => x.FinYear)
            .NotEmpty()
                .WithMessage("Financial year is required.")
            .MaximumLength(EntityConstants.FinYearLength)
                .WithMessage($"Financial year must not exceed {EntityConstants.FinYearLength} characters.");

        RuleFor(x => x.DepositInterest)
           .GreaterThan(0).WithMessage("Deposit Interest rate must be greater than 0.");

        RuleFor(x => x.LoanInterest)
            .GreaterThan(0).WithMessage("Loan Interest rate must be greater than 0.");
    }
}

public class EditFinYearCommandHandler : IRequestHandler<EditFinYearCommand>
{
    private readonly IAppDbContext _context;

    public EditFinYearCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditFinYearCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FiscalYears.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(FiscalYears), request.Id);
        }

        if(entity.Status == FiscalYearStatus.Draft)
        {
            throw new BadRequestException("Can edit only draft fiscal year record.");
        }

        entity.FinYear = request.FinYear;
        entity.DepositInterest = request.DepositInterest;
        entity.LoanInterest = request.LoanInterest;

        await _context.SaveChangesAsync(cancellationToken);
    }
}


