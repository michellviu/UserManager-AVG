using Core.Domain.DTOs;
using Core.DomainService.Interfaces.Services;
using Infrastructure.Mappers;
using Infrastructure.Infrastructure.Persistence.Validator;
using FluentValidation.Results;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Controllers.Users
{
    public class UpdateUserEndPoint : Endpoint<UserDto>
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        public UpdateUserEndPoint(IUserService userService,UserManager<User> userManager)
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


            var user = await userService.GetByIdAsync(userId);
            if (user == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }
          
            var usermapper = new UserMapper(userService);
            var user1 = await usermapper.MapToEntity(newuser);

            user1.UserName = newuser.UserName;
            user1.Email = newuser.EmailAddress;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    AddError(error.Description);
                }
                ThrowIfAnyErrors();
                return;
            }
            await SendOkAsync(ct);
        }
    }
}
