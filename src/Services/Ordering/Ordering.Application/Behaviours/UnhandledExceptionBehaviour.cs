using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<TIn, TOut> : IPipelineBehavior<TIn, TOut> where TIn: IRequest<TOut>
    {
        private readonly ILogger<TIn> _logger;

        public UnhandledExceptionBehaviour(ILogger<TIn> logger)
        {
            _logger = logger;
        }

        public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TIn).Name;
                _logger.LogError(ex, $"Application Request: Unhandled Exception for Request {requestName}");
                throw;
            }
        }
    }
}
