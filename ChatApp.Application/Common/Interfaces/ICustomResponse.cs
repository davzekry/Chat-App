using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Common.Models;

namespace ChatApp.Application.Common.Interfaces
{
    public interface ICustomResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }

        public ResponseStatus Status { get; set; }

    }
    
}
