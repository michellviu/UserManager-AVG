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
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .Length(3, 50).WithMessage("El nombre de usuario debe tener entre 3 y 50 caracteres.");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");
        }
    }
}
