using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Loans.Get;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Loans.Queries;

public class GetLoanByIdQuery : IRequest<LoanDto>
{
    public int Id { get; set; }
}

public class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, LoanDto>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetLoanByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LoanDto> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the existing loan application entity
        var loanApplication = await _context.Loans
            .Where(l => l.Id == request.Id)
            .ProjectTo<LoanDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (loanApplication == null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        return loanApplication;
    }
}

