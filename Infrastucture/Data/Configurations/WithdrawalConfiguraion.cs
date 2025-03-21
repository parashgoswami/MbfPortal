using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastucture.Data.Configurations;
public class WithdrawalConfiguraion : BaseEntityConfiguration<Withdrawal>
{
    public  override void Configure(EntityTypeBuilder<Withdrawal> builder)
        
    {
        base.Configure(builder);
        builder.ToTable("Withdrawals");
        builder.HasKey(la => la.Id);

        builder.Property(la => la.MemberId)
            .IsRequired();

        builder.Property(la => la.ApplicationDate)
            .IsRequired();

        builder.Property(la => la.AppliedAmt)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(la => la.SanctionedAmt)
            .HasColumnType("decimal(18,2)");

        builder.Property(la => la.Status)
            .IsRequired();

        builder.Property(la => la.Remarks)
            .HasMaxLength(EntityConstants.RemarksLength);
    }
   
}
