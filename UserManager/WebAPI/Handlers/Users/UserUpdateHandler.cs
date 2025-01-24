using Core.Domain.DTOs;
using Core.Domain.Entities;
using Core.DomainService.Interfaces.Services;
using Core.Exceptions;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Handlers.Users
{
    public class UserUpdateHandler
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public UserUpdateHandler(IUserService userService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public async Task HandleAsync(UserDto newuser, int userId)
        {

            var user = await userService.GetByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            var usermapper = new UserMapper(userService);
            var user1 = await usermapper.MapToEntityAsync(newuser);

            user1.UserName = newuser.UserName;
            user1.Email = newuser.EmailAddress;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
               throw new Exception("Error updating user");
            }
        }
    }
}
