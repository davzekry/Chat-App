using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class RoomMember : BaseEntity
    {
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime LastReadAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Room Room { get; set; }
        public AppUser User { get; set; }
    }
}
