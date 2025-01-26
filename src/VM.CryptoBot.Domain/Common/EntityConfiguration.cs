using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VM.CryptoBot.Domain.Common.Entities;

namespace VM.CryptoBot.Domain.Common;

public class BaseEntityConfiguration<T, TKey> : IEntityTypeConfiguration<T>
    where T : BaseEntity<TKey>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);
    }
}

public class AuditableEntityConfiguration<T, TKey> : BaseEntityConfiguration<T, TKey>
    where T : Auditable<TKey>
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(255);

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        builder.Property(e => e.DeactivatedBy)
            .HasMaxLength(255);

        builder.Property(e => e.DeactivatedAt)
            .IsRequired(false);
    }
}
