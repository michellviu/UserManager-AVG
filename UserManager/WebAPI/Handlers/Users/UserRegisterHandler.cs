using Core.Domain.Entities;
using Core.Domain.Models;
using Core.DomainService.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Handlers.Users
{
    public class UserRegisterHandler
    {
        private readonly UserManager<User> userManager;
        private readonly IUserService userservice;
        private readonly ITokenGenerator tokenGenerator;
        public UserRegisterHandler(UserManager<User> userManager, IUserService userservice, ITokenGenerator tokenGenerator)
        {
            this.userManager = userManager;
            this.userservice = userservice;
            this.tokenGenerator = tokenGenerator;

        }

        public async Task<ResponseLogin> HandleAsync(RequestUserRegister r)
        {
            var user = new User
            {
                UserName = r.username,
                Email = r.email
            };
            var result = await userManager.CreateAsync(user, r.password);
            await userManager.AddToRoleAsync(user, "USER");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception(error.Description);
                }
            }

            var usercreated = await userManager.FindByNameAsync(r.username);
            var jwtToken = await tokenGenerator.GenerateJwtTokenAsync(usercreated);

            return new ResponseLogin { username = usercreated.UserName, token = jwtToken };
           
        }
    }
}
