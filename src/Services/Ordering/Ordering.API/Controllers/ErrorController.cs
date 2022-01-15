using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [AllowAnonymous]
    public class ErrorController: ControllerBase
    {
        [Route("/error")]
        [HttpGet]
        public CustomErrorResponse Error()
        {
            var ctx = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = ctx.Error; // Exception
            var code = 500;

            if (exception is NotFoundException) code = 404;
            else if (exception is UnauthorizeException) code = 401;
            else if (exception is BadRequestException) code = 400;
            Response.StatusCode = code;

            return new CustomErrorResponse(exception);
        }
    }
}
