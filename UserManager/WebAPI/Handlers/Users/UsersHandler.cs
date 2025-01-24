using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Infrastructure.Mappers;

namespace WebAPI.Handlers.Users
{
    internal class UsersHandler
    {
        private readonly IUserService userService;
        public UsersHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<List<UserDto>> HandleAsync()
        {
            var users = await userService.GetAllAsync();
            var usersDto = new List<UserDto>();
            var usermapper = new UserMapper(userService);
            foreach (var user in users)
            {
                usersDto.Add(usermapper.MapToDto(user));
            }
            return usersDto;
        }
    }
}
