using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastucture.Data.Configurations;

public class MemberLedgerConfiguration : BaseEntityConfiguration<MemberLedger>
{
    public override void Configure(EntityTypeBuilder<MemberLedger> builder)
    {
        base.Configure(builder);

        builder.ToTable("MemberLedgers");

        builder.Property(ml => ml.EmpNo)
            .IsRequired()
            .HasMaxLength(EntityConstants.EmpNoLength);

        builder.Property(ml => ml.YearMonth)
            .IsRequired()
            .HasMaxLength(EntityConstants.YearMonthLength);
    }
}
