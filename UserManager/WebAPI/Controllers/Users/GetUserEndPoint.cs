using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Infrastructure.Mappers;

namespace WebAPI.Controllers.Users
{
    public class GetUserEndPoint : EndpointWithoutRequest<UserDto>
    {
        private readonly IUserService userService;
        public GetUserEndPoint(IUserService userService)
        {
            this.userService = userService;
        }

        public override void Configure()
        {
            Get("/api/users/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = Route<int>("id");

            var user = await userService.GetByIdAsync(userId);
            if (user == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }
            var usermapper = new UserMapper(userService);
            await SendOkAsync(usermapper.MapToDto(user),ct);
        }
    }
}
