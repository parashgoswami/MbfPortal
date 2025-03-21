using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Constants;

namespace Infrastucture.Data.Configurations;

public class LoanConfiguration : BaseEntityConfiguration<Loan>
{
    public override void Configure(EntityTypeBuilder<Loan> builder)
    {
        base.Configure(builder);
        builder.ToTable("Loans");

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