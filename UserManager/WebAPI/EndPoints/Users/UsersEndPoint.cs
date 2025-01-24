using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Handlers.Users;

namespace WebAPI.EndPoints.Users
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
            var userhandler = new UsersHandler(userService);
            await SendAsync(await userhandler.HandleAsync());
        }
    }
}
