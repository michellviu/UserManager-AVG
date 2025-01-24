using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Infrastructure.Mappers;
using Infrastructure.Infrastructure.Persistence.Validator;
using FluentValidation.Results;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using WebAPI.Handlers.Users;
using Core.Exceptions;

namespace WebAPI.EndPoints.Users
{
    public class UserUpdateEndPoint : Endpoint<UserDto>
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        public UserUpdateEndPoint(IUserService userService,UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public override void Configure()
        {
            Put("/api/users/{id}");
            Validator<UserDtoValidator>();
            Roles("ADMIN");
        }

        public override async Task HandleAsync(UserDto newuser,CancellationToken ct)
        {
            var userId = Route<int>("id");
            var userupdateHandler = new UserUpdateHandler(userService, userManager);
            try
            {
                await userupdateHandler.HandleAsync(newuser, userId);
                await SendOkAsync(ct);
            }
            catch(EntityNotFoundException)
            {
                await SendNotFoundAsync(ct);
                return;
            }catch (Exception ex)
            {
                AddError(ex.Message);
                ThrowIfAnyErrors();
                return;
            }

        }
    }
}
