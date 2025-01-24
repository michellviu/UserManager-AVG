using Core.DomainService.Interfaces.Repository;
using Core.DomainService.Interfaces.Services;
using Core.DomainService.Interfaces.UnitWork;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Handlers.Users;

namespace WebAPI.EndPoints.Users
{
    public class UserDeleteEndPoint : EndpointWithoutRequest
    {
        private readonly IUserService userService;
        public UserDeleteEndPoint(IUserService userService)
        {
            this.userService = userService;
        }

        public override void Configure()
        {
            Delete("/api/users/{id}");
            Roles("ADMIN");

        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = Route<int>("id");
            var userdeleteHandler = new UserDeleteHandler(userService);
            try
            {
                await userdeleteHandler.HandleAsync(userId);
                await SendOkAsync(ct);


            }
            catch (EntityNotFoundException)
            {
                await SendNotFoundAsync(ct);
                return;
            }  
        }
    }
}
