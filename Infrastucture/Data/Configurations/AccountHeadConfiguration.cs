using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastucture.Data.Configurations;

public class AccountHeadConfiguration : BaseEntityConfiguration<AccountHead>
{
    public override void Configure(EntityTypeBuilder<AccountHead> builder)
    {
        base.Configure(builder);

        builder.ToTable("AccountHeads");

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(EntityConstants.NameLength);

        builder.Property(a => a.Description)
            .HasMaxLength(EntityConstants.DescriptionLength);

        builder.Property(a => a.Type)
            .IsRequired();
    }
}
