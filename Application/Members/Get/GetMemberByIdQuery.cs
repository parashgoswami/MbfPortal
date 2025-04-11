using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Locations.Get;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Members.Get;

public class GetMemberByIdQuery : IRequest<MemberDto>
{
    public int Id { get; set; }
}

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto>
{
    private readonly IAppDbContext _context;
    public GetMemberByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _context.Members
            .Include(m => m.Location)
            .FirstOrDefaultAsync(m => m.Id == request.Id);

        if (member == null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        return new MemberDto
        {
            Id = member.Id,
            EmpNo = member.EmpNo,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Nominee = member.Nominee,
            Email = member.Email,
            DOJ = member.DOJ,
            DOS = member.DOS,
            Share = member.Share,
            IsActive = member.IsActive,
            Location = new LocationDto
            {
                Id = member.LocationId,
                Name = member.Location?.Name ?? string.Empty
            }
        };
    }
}
