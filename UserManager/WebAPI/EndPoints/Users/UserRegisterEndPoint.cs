using Core.Domain.Entities;
using Core.Domain.Models;
using Core.DomainService.Interfaces.Services;
using FastEndpoints;
using FluentValidation.Results;
using Infrastructure.Infrastructure.Persistence.Validator;
using Microsoft.AspNetCore.Identity;
using WebAPI.Handlers.Users;


namespace WebAPI.EndPoints.Users
{
    public class UserRegisterEndPoint:Endpoint<RequestUserRegister>
    {
        private readonly UserManager<User> userManager;
        private readonly IUserService userservice;
        private readonly ITokenGenerator tokenGenerator;
        public UserRegisterEndPoint(UserManager<User> userManager, IUserService userservice,ITokenGenerator tokenGenerator)
        {
            this.userManager = userManager;
            this.userservice = userservice;
            this.tokenGenerator = tokenGenerator;

        }
        public override void Configure()
        {
            Post("api/user/register");
            AllowAnonymous();
            Validator<RequestUserRegisterValidator>();
        }


        public override async Task HandleAsync(RequestUserRegister r, CancellationToken c)
        {

            var userRegisterHandler = new UserRegisterHandler(userManager, userservice, tokenGenerator);
            try
            {
                await SendAsync(await userRegisterHandler.HandleAsync(r));
            }
            catch (Exception e)
            {
                AddError(e.Message);
                ThrowIfAnyErrors();
                return;
            }

        }
    }
}
