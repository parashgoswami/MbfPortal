using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.AccountHeads.GetById;

public class GetAccountHeadByIdQuery : IRequest<AccountHead>
{
    public int Id { get; set; }
}

public class GetAccountHeadByIdQueryHandler : IRequestHandler<GetAccountHeadByIdQuery, AccountHead>
{
    private readonly IAppDbContext _context;

    public GetAccountHeadByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AccountHead> Handle(GetAccountHeadByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AccountHeads.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AccountHead), request.Id);
        }

        return entity;
    }
}
