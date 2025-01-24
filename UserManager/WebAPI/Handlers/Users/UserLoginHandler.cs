using Core.Domain.Entities;
using Core.Domain.Models;
using Core.DomainService.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;

namespace WebAPI.Handlers.Users
{
    public class UserLoginHandler
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenGenerator tokenGenerator;
        public UserLoginHandler(UserManager<User> userManager, SignInManager<User> signInManager, ITokenGenerator tokenGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenGenerator = tokenGenerator;
        }
        public async Task<ResponseLogin> HandleAsync(RequestUserLogin r)
        {
            var result = await signInManager.PasswordSignInAsync(r.username, r.password, false, false);

            if (!result.Succeeded)
            {
                throw new InvalidCredentialException("Sorry! Username or Password is wrong.");
            }

            var user = await userManager.FindByNameAsync(r.username);
            var jwtToken = await tokenGenerator.GenerateJwtTokenAsync(user);

            return  new ResponseLogin { username = r.username, token = jwtToken };
           
        }
    }
}
