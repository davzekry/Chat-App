// ChatApp.Infrastructure/Data/Configurations/VoiceMessageConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Data.Configurations
{
    public class VoiceMessageConfiguration : BaseEntityConfiguration<VoiceMessage>
    {
        public void Configure(EntityTypeBuilder<VoiceMessage> builder)
        {
            builder.HasKey(vm => vm.Id);
            builder.Property(vm => vm.Id).ValueGeneratedOnAdd();

            builder.Property(vm => vm.MessageId)
                .IsRequired();

            builder.Property(vm => vm.UserId)
                .IsRequired();

            builder.Property(vm => vm.AudioFilePath)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(vm => vm.DurationSeconds)
                .IsRequired();

            //builder.Property(vm => vm.RecordedAt)
            //    .IsRequired();

            // New BaseEntity properties
            //builder.Property(vm => vm.UpdatedAt);
            //builder.Property(vm => vm.IsDeleted)
            //    .IsRequired();
            //builder.Property(vm => vm.DeletedAt);

            // Relationships
            builder.HasOne(vm => vm.Message)
                .WithOne(m => m.VoiceMessage)
                .HasForeignKey<VoiceMessage>(vm => vm.MessageId)
                .OnDelete(DeleteBehavior.Cascade); // If message is deleted, delete its voice message

            builder.HasOne(vm => vm.User)
                .WithMany(u => u.VoiceMessages)
                .HasForeignKey(vm => vm.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if voice messages exist
        }
    }
}