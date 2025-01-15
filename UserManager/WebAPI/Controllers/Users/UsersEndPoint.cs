using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.Users
{
    public class UsersEndPoint : EndpointWithoutRequest<List<UserDto>>
    {
        private readonly IUserService userService;
        public UsersEndPoint(IUserService userService)
        {
            this.userService = userService;
        }
        public override void Configure()
        {
            Get("api/users");
      
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var users = await userService.GetAllAsync();
            var usersDto = new List<UserDto>();
            var usermapper = new UserMapper(userService);
            foreach (var user in users)
            {
                usersDto.Add(usermapper.MapToDto(user));
            }
            await SendAsync(usersDto);
        }
    }
}
