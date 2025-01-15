using Core.DomainService.Interfaces.Repository;
using Core.DomainService.Interfaces.Services;
using Core.DomainService.Interfaces.UnitWork;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.Users
{
    public class DeleteUserEndPoint : EndpointWithoutRequest
    {
        private readonly IUserService userService;
        public DeleteUserEndPoint(IUserService userService)
        {
            this.userService = userService;
        }

        public override void Configure()
        {
            Delete("/api/users/{id}");
            Roles("Admin");

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

            await userService.DeleteAsync(userId);
            await SendOkAsync(ct);
        }
    }
}
