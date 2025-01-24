using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Core.Exceptions;
using Infrastructure.Mappers;

namespace WebAPI.Handlers.Users
{
    public class UserGetHandler
    {
        private readonly IUserService userService;
        public UserGetHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<UserDto> HandleAsync(int userId)
        {
            var user = await userService.GetByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            var usermapper = new UserMapper(userService);
            return usermapper.MapToDto(user);
        }
    }
}
