using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Models
{
    public static class Response
    {
        public static Response<T> Ok<T>(string msg, T data) => new Response<T>(200, msg, data);
        public static Response<T> Created<T>(string msg, T data) => new Response<T>(201, msg, data);

        public static Response<T> NotFound<T>(string msg) => new Response<T>(404, msg, default);
        public static Response<T> BadRequest<T>(string msg, T data) => new Response<T>(400, msg, data);
        public static Response<T> ServerError<T>(string msg) => new Response<T>(500, msg, default);
    }
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Response(int statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}
