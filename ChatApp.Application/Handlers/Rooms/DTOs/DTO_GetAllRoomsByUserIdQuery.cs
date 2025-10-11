using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.Handlers.Rooms.DTOs
{
    public class DTO_GetAllRoomsByUserIdQuery
    {        
        public string? RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? ImagePath { get; set; }
        public RoomType RoomType { get; set; }
        public DateTime LastMessageAt { get; set; }   
    }
}
