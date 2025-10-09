using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using FileMessage = ChatApp.Domain.Entities.FileMessage;

namespace ChatApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RoomMember> RoomMembers { get; set; }
        public DbSet<FileMessage> FileMessages { get; set; }
        public DbSet<VoiceMessage> VoiceMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new RoomMemberConfiguration());
            modelBuilder.ApplyConfiguration(new FileMessageConfiguration());
            modelBuilder.ApplyConfiguration(new VoiceMessageConfiguration());
        }
    }
}
