using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastucture.Data.Configurations;

public class FiscalYearConfiguration : BaseEntityConfiguration<FiscalYear>
{
    public override void Configure(EntityTypeBuilder<FiscalYear> builder)
    {
        base.Configure(builder);

        builder.ToTable("FiscalYears");

        builder.Property(f => f.FinYear)
            .IsRequired()
            .HasMaxLength(EntityConstants.FinYearLength); 

        builder.Property(f => f.DepositInterest)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(f => f.LoanInterest)
           .IsRequired()
           .HasColumnType("decimal(5,2)");
    }
}

