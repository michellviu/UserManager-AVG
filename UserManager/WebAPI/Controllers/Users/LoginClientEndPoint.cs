using Core.Domain.Entities;
using Core.Domain.Models;
using Core.DomainService.Interfaces.Services;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Controllers.Users
{
    public class LoginClientEndPoint : Endpoint<RequestUserLogin>
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenGenerator tokenGenerator;
        public LoginClientEndPoint(UserManager<User> userManager, SignInManager<User> signInManager, ITokenGenerator tokenGenerator)
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

            var result = await signInManager.PasswordSignInAsync(r.username, r.password, false, false);

            if (!result.Succeeded)
            {
                AddError("Sorry! Username or Password is wrong.");
                ThrowIfAnyErrors();
                return;
            }

            var user = await userManager.FindByNameAsync(r.username);
            var Id = user.Id;
            var jwtToken = await tokenGenerator.GenerateJwtToken(user);

            var responselogin = new ResponseLogin { username = r.username, token = jwtToken.ToString() };
            await SendAsync(responselogin);

        }
    }
}
