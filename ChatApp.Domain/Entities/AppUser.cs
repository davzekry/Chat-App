using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        public string? ImagePath { get; set; }
        public bool IsOnline { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public ICollection<Message> Messages { get; set; }
        public ICollection<RoomMember> RoomMembers { get; set; }
        public ICollection<VoiceMessage> VoiceMessages { get; set; }
        public ICollection<Room> CreatedRooms { get; set; } // Rooms created by this user

        public AppUser()
        {
            Messages = new HashSet<Message>();
            RoomMembers = new HashSet<RoomMember>();
            VoiceMessages = new HashSet<VoiceMessage>();
            CreatedRooms = new HashSet<Room>();
        }
    }
}
