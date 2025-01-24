using Core.Domain.Entities;
using Core.Domain.Models;
using Core.DomainService.Interfaces.Services;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Core.Exceptions;
using WebAPI.Handlers.Users;

namespace WebAPI.EndPoints.Users
{
    public class UserLoginEndPoint : Endpoint<RequestUserLogin>
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenGenerator tokenGenerator;
        public UserLoginEndPoint(UserManager<User> userManager, SignInManager<User> signInManager, ITokenGenerator tokenGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenGenerator = tokenGenerator;
        }
        public override void Configure()
        {
            Post("api/user/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RequestUserLogin r, CancellationToken c)
        {
            var userloginHandler = new UserLoginHandler(userManager, signInManager, tokenGenerator);

            try
            {
                await SendAsync(await userloginHandler.HandleAsync(r));
            }
            catch (InvalidCredentialsException e)
            {
                AddError(e.Message);
                ThrowIfAnyErrors();
                return;
            }
           
        }
    }
}
