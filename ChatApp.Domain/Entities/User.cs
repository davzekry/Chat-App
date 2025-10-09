using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastSeenAt { get; set; }
        public bool IsOnline { get; set; } = false;

        // Navigation properties
        public ICollection<Message> Messages { get; set; }
        public ICollection<RoomMember> RoomMembers { get; set; }
        public ICollection<VoiceMessage> VoiceMessages { get; set; }
        public ICollection<Room> CreatedRooms { get; set; } // Rooms created by this user

        public User()
        {
            Messages = new HashSet<Message>();
            RoomMembers = new HashSet<RoomMember>();
            VoiceMessages = new HashSet<VoiceMessage>();
            CreatedRooms = new HashSet<Room>();
        }
    }
}
