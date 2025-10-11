using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Messages.DTOs;

namespace ChatApp.Application.Handlers.Messages.DTOs
{
    public class DTO_GetAllMessagesByRoomIdQuery
    {
        public List<DTO_Message> Messages { get; set; } = new();
        public int TotalCount { get; set; }
        public bool HasMore { get; set; }
    }
}
