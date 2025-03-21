using Application.AccountHeads.Get;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AccountHeads.GetById;

public class GetAccountHeadByIdQuery : IRequest<AccountHeadDto>
{
    public int Id { get; set; }
}

public class GetAccountHeadByIdQueryHandler : IRequestHandler<GetAccountHeadByIdQuery, AccountHeadDto>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountHeadByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccountHeadDto> Handle(GetAccountHeadByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AccountHeads
            .Where(x => x.Id == request.Id)
            .ProjectTo<AccountHeadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AccountHead), request.Id);
        }

        return entity;
    }
}


