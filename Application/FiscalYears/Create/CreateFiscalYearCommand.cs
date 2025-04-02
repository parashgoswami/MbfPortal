using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.FiscalYears.Commands;

public class CreateFiscalYearCommand : IRequest<int>
{
    public string FinYear { get; set; } = string.Empty;
    public decimal DepositInterest { get; set; }
    public decimal LoanInterest { get; set; }
}

public class CreateFinYearValidator : AbstractValidator<CreateFiscalYearCommand>
{
    public CreateFinYearValidator()
    {
        RuleFor(x => x.FinYear)
            .NotEmpty()
                .WithMessage("Financial year is required.")
            .MaximumLength(EntityConstants.FinYearLength)
                .WithMessage($"Financial year must not exceed {EntityConstants.FinYearLength} characters.")
            .Matches(@"^\d{4}-\d{2}$")
                .WithMessage("Financial year must be in the format xxxx-yy (e.g. 2025-26).");

        RuleFor(x => x.DepositInterest)
            .GreaterThan(0).WithMessage("Deposit Interest rate must be greater than 0.");

        RuleFor(x => x.LoanInterest)
            .GreaterThan(0).WithMessage("Loan Interest rate must be greater than 0.");
    }
}

public class CreateFinYearCommandHandler : IRequestHandler<CreateFiscalYearCommand, int>
{
    private readonly IAppDbContext _context;

    public CreateFinYearCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateFiscalYearCommand request, CancellationToken cancellationToken)
    {
        var entity = new FiscalYear
        {
            FinYear = request.FinYear,
            DepositInterest = request.DepositInterest,
            LoanInterest = request.LoanInterest
        };

        _context.FiscalYears.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


