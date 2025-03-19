using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Locations.Edit;

public class EditLocationCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class EditLocationCommandValidator : AbstractValidator<EditLocationCommand>
{
    public EditLocationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(EntityConstants.NameLength).WithMessage($"Name must not exceed {EntityConstants.NameLength} characters.");
    }
}

public class EditLocationCommandHandler : IRequestHandler<EditLocationCommand>
{
    private readonly IAppDbContext _context;

    public EditLocationCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Locations.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Location), request.Id);
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
