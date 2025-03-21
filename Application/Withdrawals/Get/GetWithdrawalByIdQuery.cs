using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Withdrawals.Get;

public class GetWithdrawalByIdQuery : IRequest<WithdrawalDto>
{
    public int Id { get; set; }
}

public class GetWithdrawalByIdQueryHandler : IRequestHandler<GetWithdrawalByIdQuery, WithdrawalDto>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetWithdrawalByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WithdrawalDto> Handle(GetWithdrawalByIdQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the existing withdrawal entity
        var withdrawal = await _context.Withdrawals
            .Where(l => l.Id == request.Id)
            .ProjectTo<WithdrawalDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (withdrawal == null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        return withdrawal;
    }
}

