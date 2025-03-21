using Application.Common.Interfaces;
using Domain.Entities;
using Infrastucture.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastucture.Data;
public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
{
    public DbSet<AccountHead> AccountHeads => Set<AccountHead>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Withdrawal> Withdrawals => Set<Withdrawal>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
