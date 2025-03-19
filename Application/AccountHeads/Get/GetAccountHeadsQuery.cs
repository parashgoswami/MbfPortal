using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AccountHeads.Get;

public class GetAccountHeadsQuery : IRequest<List<AccountHead>>
{
}

public class GetAccountHeadsQueryHandler : IRequestHandler<GetAccountHeadsQuery, List<AccountHead>>
{
    private readonly IAppDbContext _context;

    public GetAccountHeadsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AccountHead>> Handle(GetAccountHeadsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AccountHeads.ToListAsync(cancellationToken);
    }
}
