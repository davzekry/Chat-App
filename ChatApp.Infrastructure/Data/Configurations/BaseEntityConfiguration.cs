using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Data.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt);

            builder.Property(e => e.IsDeleted)
                .IsRequired();

            builder.Property(e => e.DeletedAt);
        }
    }
}
