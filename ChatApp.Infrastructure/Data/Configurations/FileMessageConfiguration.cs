// ChatApp.Infrastructure/Data/Configurations/FileMessageConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Data.Configurations
{
    public class FileMessageConfiguration : BaseEntityConfiguration<FileMessage>
    {
        public void Configure(EntityTypeBuilder<FileMessage> builder)
        {
            builder.HasKey(fm => fm.Id);
            builder.Property(fm => fm.Id).ValueGeneratedOnAdd();

            builder.Property(fm => fm.MessageId)
                .IsRequired();

            builder.Property(fm => fm.FileName)
                .IsRequired()
                .HasMaxLength(255);

            //builder.Property(fm => fm.FileType)
            //    .IsRequired()
            //    .HasMaxLength(50);

            builder.Property(fm => fm.FilePath)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(fm => fm.FileSize)
                .IsRequired();

            builder.Property(fm => fm.CreatedAt)
                .IsRequired();

            // New BaseEntity properties
            //builder.Property(fm => fm.UpdatedAt);
            //builder.Property(fm => fm.IsDeleted)
            //    .IsRequired();
            //builder.Property(fm => fm.DeletedAt);

            // Relationship (One-to-One with Message)
            builder.HasOne(fm => fm.Message)
                .WithOne(m => m.FileMessage)
                .HasForeignKey<FileMessage>(fm => fm.MessageId)
                .OnDelete(DeleteBehavior.Cascade); // If message is deleted, delete its file message
        }
    }
}