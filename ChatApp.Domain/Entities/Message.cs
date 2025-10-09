using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string UserId { get; set; }
        public string RoomId { get; set; }
        public string MessageText { get; set; }
        public bool IsEdited { get; set; } = false;
        public MessageType MessageType { get; set; } = 0;

        // Navigation properties
        public User User { get; set; }
        public Room Room { get; set; }
        public FileMessage FileMessage { get; set; }
        public VoiceMessage VoiceMessage { get; set; }
    }
}
