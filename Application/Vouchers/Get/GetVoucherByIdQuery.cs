using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Vouchers.Base;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Vouchers.GetById;

public class GetVoucherByIdQuery : IRequest<VoucherDto>
{
    public int Id { get; set; }
}

public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, VoucherDto>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetVoucherByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VoucherDto> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var voucher = await _context.Vouchers
            .Include(v => v.VoucherLines)
            .Where(v => v.Id == request.Id)
            .ProjectTo<VoucherDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (voucher == null)
        {
            throw new NotFoundException(nameof(Voucher), request.Id);
        }

        return voucher;
    }
}



