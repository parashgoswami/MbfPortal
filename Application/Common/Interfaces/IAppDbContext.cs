using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<AccountHead> AccountHeads { get; }
    DbSet<Location> Locations { get; }
    DbSet<Member> Members { get; }
    DbSet<Voucher> Vouchers { get; }
    DbSet<LoanApplication> LoanApplications { get; }
    DbSet<WithdrawalApplication> WithdrawalApplications { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
