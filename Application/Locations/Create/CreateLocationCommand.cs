using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Create;

public sealed class CreateLocationCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
}

public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(EntityConstants.NameLength).WithMessage($"Name must not exceed {EntityConstants.NameLength} characters.");
    }
}

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, int>
{
    private readonly IAppDbContext _context;
    
    public CreateLocationCommandHandler(IAppDbContext context)
    {
        _context = context;        
    }
    public async Task<int> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = new Location { Name = request.Name };
        var existing = await _context.Locations.FirstOrDefaultAsync(x => x.Name == entity.Name);
        if (existing != null)
        {
            throw new BadRequestException("Location already exists.");
        }
        _context.Locations.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

