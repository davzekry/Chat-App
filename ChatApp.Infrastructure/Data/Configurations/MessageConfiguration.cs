// ChatApp.Infrastructure/Data/Configurations/MessageConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Data.Configurations
{
    public class MessageConfiguration : BaseEntityConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();

            builder.Property(m => m.UserId)
                .IsRequired();

            builder.Property(m => m.RoomId)
                .IsRequired();

            builder.Property(m => m.MessageText)
                .HasMaxLength(150); // Enforce max length as per requirements

            //builder.Property(m => m.SentAt)
            //    .IsRequired();

            builder.Property(m => m.IsEdited)
                .IsRequired();

            builder.Property(m => m.MessageType)
                .IsRequired()
                .HasMaxLength(50); // e.g., "Text", "File", "Voice"

            // New BaseEntity properties
            //builder.Property(m => m.UpdatedAt);
            //builder.Property(m => m.DeletedAt);

            // Relationships
            builder.HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if messages exist

            builder.HasOne(m => m.Room)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Cascade); // If room is deleted, delete its messages

            builder.HasOne(m => m.FileMessage)
                .WithOne(fm => fm.Message)
                .HasForeignKey<FileMessage>(fm => fm.MessageId)
                .OnDelete(DeleteBehavior.Cascade); // If message is deleted, delete its file message

            builder.HasOne(m => m.VoiceMessage)
                .WithOne(vm => vm.Message)
                .HasForeignKey<VoiceMessage>(vm => vm.MessageId)
                .OnDelete(DeleteBehavior.Cascade); // If message is deleted, delete its voice message
        }
    }
}