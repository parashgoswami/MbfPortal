using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Members.Create;

public class CreateMemberCommand :IRequest<int>
{
    public string EmpNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Nominee { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime DOJ { get; set; }
    public DateTime? DOS { get; set; }
    public int LocationId { get; set; }
    public decimal Share { get; set; }
}

public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.EmpNo)
            .NotEmpty().WithMessage("Employee number is required.")
            .Length(EntityConstants.EmpNoLength).WithMessage($"Employee number must be {EntityConstants.EmpNoLength} characters long.");

        RuleFor(x => x.LocationId)
            .GreaterThan(0).WithMessage("Please select a location");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(EntityConstants.NameLength).WithMessage($"First name cannot exceed {EntityConstants.NameLength} character");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(EntityConstants.NameLength).WithMessage($"Last name cannot exceed {EntityConstants.NameLength} character");

        RuleFor(x => x.Nominee)
           .MaximumLength(EntityConstants.NomineeLength).WithMessage($"Last name cannot exceed {EntityConstants.NameLength} character");

        RuleFor(x => x.DOJ)
            .NotEmpty().WithMessage("Joining date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Joining date cannot be in the future.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.Share)
           .GreaterThanOrEqualTo(0).WithMessage("Share must be greater than or equal to 0.");

    }
}

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, int>
{
    private readonly IAppDbContext _context;
    public CreateMemberCommandHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _context.Members.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailExists)
        {
            throw new BadRequestException("Email already taken");
        }

        var empNoExists = await _context.Members.AnyAsync(x => x.EmpNo == request.EmpNo, cancellationToken);
        if (empNoExists)
        {
            throw new BadRequestException("EmpNo already taken");
        }

        var entity = new Member
        {
            EmpNo = request.EmpNo,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Nominee = request.Nominee,
            Email = request.Email,
            DOJ = request.DOJ,
            DOS = request.DOS,
            LocationId = request.LocationId,
            Share = request.Share
        };
        _context.Members.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
