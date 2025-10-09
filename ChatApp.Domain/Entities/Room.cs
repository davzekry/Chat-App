using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class Room : BaseEntity
    {
        public string RoomName { get; set; }
        public string? Description { get; set; }
        public RoomType RoomType { get; set; } = 0;
        public string CreatedByUserId { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public User CreatedByUser { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<RoomMember> RoomMembers { get; set; }

        public Room()
        {
            Messages = new HashSet<Message>();
            RoomMembers = new HashSet<RoomMember>();
        }
    }
}
