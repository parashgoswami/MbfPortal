using Application.AccountHeads.Create;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AccountHeads.Edit;

public class EditAccountHeadCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AccountType Type { get; set; }
}

public class EditAccountHeadCommandValidator : AbstractValidator<EditAccountHeadCommand>
{
    public EditAccountHeadCommandValidator()
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

public class EditAccountHeadCommandHandler : IRequestHandler<EditAccountHeadCommand>
{
    private readonly IAppDbContext _context;

    public EditAccountHeadCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditAccountHeadCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AccountHeads.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AccountHead), request.Id);
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Type = request.Type;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
