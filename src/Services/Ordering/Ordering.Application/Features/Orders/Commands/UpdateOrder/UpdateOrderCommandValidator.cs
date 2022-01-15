using FluentValidation;
using FluentValidation.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator: AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.updateOrder.Id).Equal(p => p.id).WithMessage($"Bad request! Order id is not correct");
            RuleFor(p => p.updateOrder.Address).NotEmpty().WithMessage("{Username} is required")
               .NotNull()
               .MaximumLength(18).WithMessage("{Username} maximum length is 18 characters")
               .MinimumLength(10).WithMessage("{Username} minimum length is 10 characters");

            RuleFor(p => p.updateOrder.Email).NotEmpty().WithMessage("{Email} is required")
                .NotNull()
                .EmailAddress(mode: FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("{Email} is not in correct format");

            RuleFor(p => p.updateOrder.Phone).NotEmpty().WithMessage("{Phone} is required")
                .NotNull()
                .MaximumLength(12).WithMessage("{Phone} maximum digit is 12")
                .MinimumLength(10).WithMessage("{Phone} minium digit is 10")
                .Matches(@"/\d/g").WithMessage("{Phone} is not in correct format");
            RuleFor(p => p.updateOrder.Address).NotEmpty().WithMessage("{Address} is required");
        }
    }
}
