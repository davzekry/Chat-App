using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Handlers.Messages.DTOs
{
    public class DTO_VoiceMessage
    {
        public string Id { get; set; }
        public string? AudioUrl { get; set; }
        public int DurationSeconds { get; set; }
    }
}
