using Core.Domain.Entities;
using Core.Domain.Models;
using Core.DomainService.Interfaces.Services;
using FastEndpoints;
using FluentValidation.Results;
using Infrastructure.Infrastructure.Persistence.Validator;
using Microsoft.AspNetCore.Identity;
using static FastEndpoints.Ep;

namespace WebAPI.Controllers.Users
{
    public class RegisterUserEndPoint:Endpoint<RequestUserRegister>
    {
        private readonly UserManager<User> userManager;
        private readonly IUserService userservice;
     //   private readonly ITokenGenerator tokenGenerator;
        public RegisterUserEndPoint(UserManager<User> userManager, IUserService userservice)
        {
            this.userManager = userManager;
            this.userservice = userservice;
    //        this.tokenGenerator = tokenGenerator;

        }
        public override void Configure()
        {
            Post("api/user/register");
            AllowAnonymous();
            Validator<RequestUserRegisterValidator>();
        }


        public override async Task HandleAsync(RequestUserRegister r, CancellationToken c)
        {

            var user = new User
            {
                UserName = r.username,
                Email = r.email
            };
            var result = await userManager.CreateAsync(user, r.password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    AddError(error.Description);
                }
                ThrowIfAnyErrors();
                return;
            }

            var cliente = await userManager.FindByNameAsync(r.username);
            var Id = cliente.Id;
          
          
          //  var jwtToken = tokenGenerator.GenerateJwtToken(cliente);

            var responselogin = new ResponseLogin { username = cliente.UserName, token = "" };
            await SendAsync(responselogin);

        }
    }
}
