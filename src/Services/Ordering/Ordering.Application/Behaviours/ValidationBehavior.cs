using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Ordering.Application.Exceptions.ValidationException;
namespace Ordering.Application.Behaviours
{
    public class ValidationBehavior<TIn, TOut> : IPipelineBehavior<TIn, TOut> where TIn: IRequest<TOut>
    {
        private readonly IEnumerable<IValidator<TIn>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TIn>> validators)
        {
            _validators = validators;
        }

        public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
        {
            if (_validators.Any())
            {
                var ctx = new ValidationContext<TIn>(request);

                var validationResults = await Task.WhenAll(_validators
                    .Select(sel => sel.ValidateAsync(ctx, cancellationToken)));

                var failures = validationResults.SelectMany(sel => sel.Errors).Where(f => f != null).ToList();

                if(failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }

}
