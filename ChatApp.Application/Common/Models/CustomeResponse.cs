using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Common.Interfaces;

namespace ChatApp.Application.Common.Models
{
    public class CustomeResponse<T> : ICustomResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }
        public ResponseStatus Status { get; set; }

        public CustomeResponse()
        {
            
        }

        public CustomeResponse(ResponseStatus status, T data, string message = null, string internalMessage = null, int subStatus = 0)
        {
            Status = status;
            Data = data;
            Message = message;
            InternalMessage = internalMessage;
        }


        public static CustomeResponse<T> Success(T data, string message = null)
        {
            return new CustomeResponse<T>(ResponseStatus.Success, data, message);
        }


        public static CustomeResponse<T> Fail(string message, ResponseStatus status = ResponseStatus.BadRequest, int subStatus = 0, string internalMessage = null)
        {
            return new CustomeResponse<T>
            {
                Status = status,
                Message = message,
                InternalMessage = internalMessage,
                Data = default
            };
        }


        public static CustomeResponse<T> Error(string message, string internalMessage = null, int subStatus = 0)
        {
            return new CustomeResponse<T>
            {
                Status = ResponseStatus.InternalServerError,
                Message = message,
                InternalMessage = internalMessage,
                Data = default
            };
        } 
    }


    public enum ResponseStatus
    {


        Success = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,

        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        TooManyRequests = 429,

        InternalServerError = 500,
        NotImplemented = 501,
        ServiceUnavailable = 503,
    }

    public enum CustomeResponse
    {
        Success = 0,
        Error = 1,
        AuthFailure = 2,
        Conflict = 3,
    }
}
