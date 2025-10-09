using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class VoiceMessage : BaseEntity
    {
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public string AudioFilePath { get; set; }
        public int DurationSeconds { get; set; }

        // Navigation properties
        public Message Message { get; set; }
        public User User { get; set; }
    }
}
