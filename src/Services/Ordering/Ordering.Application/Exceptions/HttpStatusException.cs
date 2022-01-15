using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class HttpStatusException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpStatusException(HttpStatusCode statusCode, string msg): base(msg)
        {
            StatusCode = statusCode;
        }
    }
    public class NotFoundException: HttpStatusException
    {
        public NotFoundException(string msg, HttpStatusCode statusCode = HttpStatusCode.NotFound):base(statusCode, msg)
        {

        }
    }
    public class UnauthorizeException: HttpStatusException
    {
        public UnauthorizeException(string msg, HttpStatusCode statusCode = HttpStatusCode.Unauthorized) : base(statusCode, msg)
        {

        }
    }
    public class BadRequestException: HttpStatusException
    {
        public BadRequestException(string msg, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(statusCode, msg)
        {

        }
    }
}
