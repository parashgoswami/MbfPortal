using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AccountHeads.Create;

public class CreateAccountHeadCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AccountType Type { get; set; }
}

public class CreateAccountHeadCommandValidator : AbstractValidator<CreateAccountHeadCommand>
{
    public CreateAccountHeadCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Name is required.")
            .MaximumLength(EntityConstants.NameLength)
                .WithMessage($"Name must not exceed {EntityConstants.NameLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(EntityConstants.NameLength)
                .WithMessage($"Description must not exceed {EntityConstants.DescriptionLength} characters.");
    }
}

public class CreateAccountHeadCommandHandler : IRequestHandler<CreateAccountHeadCommand, int>
{
    private readonly IAppDbContext _context;
    private readonly ITimeService _timeService;

    public CreateAccountHeadCommandHandler(IAppDbContext context, ITimeService timeService)
    {
        _context = context;
        _timeService = timeService;
    }

    public async Task<int> Handle(CreateAccountHeadCommand request, CancellationToken cancellationToken)
    {
        var entity = new AccountHead
        {
            Name = request.Name,
            Description = request.Description,
            Type = request.Type
        };

        var existing = await _context.AccountHeads.FirstOrDefaultAsync(x => x.Name == entity.Name);
        if (existing != null)
        {
            throw new BadRequestException("AccountHead already exists.");
        }
        _context.AccountHeads.Add(entity);

        var accountHeadId = await _context.AccountHeads
            .Where(x => x.Name == entity.Name)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (accountHeadId != 0)
        {
            var accountBalance = new AccountBalance
            {
                AccountHeadId = accountHeadId,
                FinYear = _timeService.GetFinancialYear(DateTime.Today),
                DebitBalance = 0,
                CreditBalance = 0
            };

            _context.AccountBalances.Add(accountBalance);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
