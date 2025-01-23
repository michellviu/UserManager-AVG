using Core.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure.Persistence.Validator
{
    public class RequestUserRegisterValidator : AbstractValidator<RequestUserRegister>
    {
        public RequestUserRegisterValidator()
        {
            RuleFor(x => x.username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("The username must be between 3 and 50 characters long.");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("The password is required.")
                .MinimumLength(6).WithMessage("The password must be at least 6 characters long.");

            RuleFor(x => x.email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is invalid.");
        }
    }
}
