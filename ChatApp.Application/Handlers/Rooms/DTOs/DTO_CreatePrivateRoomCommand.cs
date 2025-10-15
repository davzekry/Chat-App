using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Handlers.Rooms.DTOs
{
    public class DTO_CreatePrivateRoomCommand
    {
        public string? RoomId { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
