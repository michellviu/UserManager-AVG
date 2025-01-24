using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Core.Exceptions;
using Infrastructure.Mappers;
using WebAPI.Handlers.Users;

namespace WebAPI.EndPoints.Users
{
    public class UserGetEndPoint : EndpointWithoutRequest<UserDto>
    {
        private readonly IUserService userService;
        public UserGetEndPoint(IUserService userService)
        {
            this.userService = userService;
        }

        public override void Configure()
        {
            Get("/api/users/{id}");
      
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = Route<int>("id");
            var usergethandler = new UserGetHandler(userService);
            try
            {
                await SendOkAsync(await usergethandler.HandleAsync(userId), ct);
            }
            catch (EntityNotFoundException)
            {
                await SendNotFoundAsync(ct);
                return;
            }
        }
    }
}
