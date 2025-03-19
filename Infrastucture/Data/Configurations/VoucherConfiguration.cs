using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastucture.Data.Configurations;

public class VoucherConfiguration : BaseEntityConfiguration<Voucher>
{
    public override void Configure(EntityTypeBuilder<Voucher> builder)
    {
        base.Configure(builder);

        builder.ToTable("Vouchers");

        builder.Property(v => v.VoucherNo)
            .IsRequired()
            .HasMaxLength(EntityConstants.VoucherNoLength);

        builder.Property(v => v.FinYear)
            .IsRequired()
            .HasMaxLength(EntityConstants.FinYearLength);

        builder.Property(v => v.Narration)
            .IsRequired()
            .HasMaxLength(EntityConstants.NarrationLength);

        builder.Property(v => v.Date)
            .IsRequired();
        
        builder.Ignore(v => v.DebitAmt);
        builder.Ignore(v => v.CreditAmt);

        builder.HasMany(v => v.VoucherLines)
            .WithOne(vl => vl.Voucher)
            .HasForeignKey(vl => vl.VoucherId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class VoucherLineConfiguration : BaseEntityConfiguration<VoucherLine>
{
    public override void Configure(EntityTypeBuilder<VoucherLine> builder)
    {
        base.Configure(builder);

        builder.ToTable("VoucherLines");

        builder.Property(vl => vl.Narration)
            .IsRequired()
            .HasMaxLength(EntityConstants.NarrationLength);

        builder.Property(vl => vl.DebitAmt)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(vl => vl.CreditAmt)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}
