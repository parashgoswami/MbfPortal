using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Members.Get;

public class GetMemberPaginationQuery : IRequest<PaginatedList<MemberDto>>
{
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}

public class GetMemberPaginationQueryHandler : IRequestHandler<GetMemberPaginationQuery, PaginatedList<MemberDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public GetMemberPaginationQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<MemberDto>> Handle(GetMemberPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Members
            .Include(m => m.Location)
            .OrderBy(m => m.EmpNo)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<MemberDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}
