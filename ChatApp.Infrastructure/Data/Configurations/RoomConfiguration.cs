// ChatApp.Infrastructure/Data/Configurations/RoomConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Data.Configurations
{
    public class RoomConfiguration : BaseEntityConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.Property(r => r.RoomName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.ImagePath)
                .HasMaxLength(600);

            builder.Property(r => r.RoomType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            // New BaseEntity properties
            //builder.Property(r => r.UpdatedAt);
            //builder.Property(r => r.IsDeleted)
            //    .IsRequired();
            //builder.Property(r => r.DeletedAt);

            //builder.Property(r => r.CreatedByUserId)
            //    .IsRequired();

            //builder.Property(r => r.IsActive)
            //    .IsRequired();

            // Relationships
            builder.HasOne(r => r.CreatedByUser)
                .WithMany(u => u.CreatedRooms)
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if they created rooms

            builder.HasMany(r => r.Messages)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Cascade); // If room is deleted, delete its messages

            builder.HasMany(r => r.RoomMembers)
                .WithOne(rm => rm.Room)
                .HasForeignKey(rm => rm.RoomId)
                .OnDelete(DeleteBehavior.Cascade); // If room is deleted, delete its memberships
        }
    }
}