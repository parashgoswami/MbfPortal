using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<AccountHead> AccountHeads { get; }
    DbSet<Location> Locations { get; }
    DbSet<Member> Members { get; }
    DbSet<Voucher> Vouchers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
