using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.UserName).IsUnique();

            //builder.Property(u => u.Password)
            //    .IsRequired()
            //    .HasMaxLength(256);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            //builder.Property(u => u.LastSeenAt)
            //    .IsRequired();

            builder.Property(u => u.IsOnline)
                .IsRequired();

            // New BaseEntity properties
            builder.Property(u => u.UpdatedAt);
            builder.Property(u => u.IsDeleted)
                .IsRequired();
            builder.Property(u => u.DeletedAt);

            // Relationships
            builder.HasMany(u => u.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if messages exist

            builder.HasMany(u => u.RoomMembers)
                .WithOne(rm => rm.User)
                .HasForeignKey(rm => rm.UserId)
                .OnDelete(DeleteBehavior.Cascade); // If user is deleted, remove their room memberships

            builder.HasMany(u => u.VoiceMessages)
                .WithOne(vm => vm.User)
                .HasForeignKey(vm => vm.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if voice messages exist

            builder.HasMany(u => u.CreatedRooms)
                .WithOne(r => r.CreatedByUser)
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if they created rooms
        }
    }
}