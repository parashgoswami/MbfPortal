using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Constants;

namespace Infrastucture.Data.Configurations;

public class LoanApplicationConfiguration : BaseEntityConfiguration<LoanApplication>
{
    public override void Configure(EntityTypeBuilder<LoanApplication> builder)
    {
        base.Configure(builder);
        builder.ToTable("LoanApplications");

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
            .HasMaxLength(EntityConstants.DescriptionLength);
    }
}