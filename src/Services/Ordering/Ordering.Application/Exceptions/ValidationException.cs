using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class ValidationException: ApplicationException
    {
        public ValidationException(): base("One or more validation failures have occured")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public IDictionary<string,string[]> Errors { get; set; }
        public ValidationException(IEnumerable<ValidationFailure> failures): this()
        {
            Errors = failures.GroupBy(g => g.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(key => key.Key, value => value.ToArray());
        }
    }
}
