using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<AccountHead> AccountHeads { get; }
    DbSet<Location> Locations { get; }
    DbSet<Member> Members { get; }
    DbSet<MemberLedger> MemberLedgers { get; }
    DbSet<Voucher> Vouchers { get; }
    DbSet<Loan> Loans { get; }
    DbSet<Withdrawal> Withdrawals { get; }
    DbSet<FiscalYear> FiscalYears { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
