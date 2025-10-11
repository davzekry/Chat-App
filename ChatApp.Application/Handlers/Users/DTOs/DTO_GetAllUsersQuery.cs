using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Handlers.Users.DTOs
{
    public class DTO_GetAllUsersQuery
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ImagePath { get; set; }
        public bool? IsOnline { get; set; }
    }
}
