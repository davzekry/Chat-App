// ChatApp.Infrastructure/Data/Configurations/RoomMemberConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Data.Configurations
{
    public class RoomMemberConfiguration : BaseEntityConfiguration<RoomMember>
    {
        public void Configure(EntityTypeBuilder<RoomMember> builder)
        {
            builder.HasKey(rm => rm.Id);
            builder.Property(rm => rm.Id).ValueGeneratedOnAdd();

            builder.Property(rm => rm.RoomId)
                .IsRequired();

            builder.Property(rm => rm.UserId)
                .IsRequired();

            builder.Property(rm => rm.JoinedAt)
                .IsRequired();

            builder.Property(rm => rm.LastReadAt)
                .IsRequired();

            builder.Property(rm => rm.IsActive)
                .IsRequired();

            // New BaseEntity properties
            //builder.Property(rm => rm.UpdatedAt);
            //builder.Property(rm => rm.IsDeleted)
            //    .IsRequired();
            //builder.Property(rm => rm.DeletedAt);

            // Relationships
            builder.HasOne(rm => rm.Room)
                .WithMany(r => r.RoomMembers)
                .HasForeignKey(rm => rm.RoomId)
                .OnDelete(DeleteBehavior.Cascade); // If room is deleted, delete its memberships

            builder.HasOne(rm => rm.User)
                .WithMany(u => u.RoomMembers)
                .HasForeignKey(rm => rm.UserId)
                .OnDelete(DeleteBehavior.Cascade); // If user is deleted, delete their room memberships
        }
    }
}