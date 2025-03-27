using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Members.Edit;

public class EditMemberCommand : IRequest
{
    public int Id { get; set; }
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

public class EditMemberValidator : AbstractValidator<EditMemberCommand>
{
    public EditMemberValidator()
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

public class EditMemberCommandHandler : IRequestHandler<EditMemberCommand>
{
    private readonly IAppDbContext _context;

    public EditMemberCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Members.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        var emailExists = await _context.Members.AnyAsync(x => x.Email == request.Email && x.Id != request.Id, cancellationToken);
        if (emailExists)
        {
            throw new BadRequestException("Email already taken");
        }

        var empNoExists = await _context.Members.AnyAsync(x => x.EmpNo == request.EmpNo && x.Id != request.Id, cancellationToken);
        if (empNoExists)
        {
            throw new BadRequestException("EmpNo already taken");
        }

        entity.EmpNo = request.EmpNo;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Nominee = request.Nominee;
        entity.Email = request.Email;
        entity.DOJ = request.DOJ;
        entity.DOS = request.DOS;
        entity.LocationId = request.LocationId;
        entity.Share = request.Share;

        await _context.SaveChangesAsync(cancellationToken);
    }
}


