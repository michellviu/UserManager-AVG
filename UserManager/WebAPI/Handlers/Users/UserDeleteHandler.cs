using Core.DomainService.Interfaces.Services;
using Core.Exceptions;

namespace WebAPI.Handlers.Users
{
    public class UserDeleteHandler
    {
        private readonly IUserService userService;
        public UserDeleteHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task HandleAsync(int userId)
        {
            var user = await userService.GetByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            await userService.DeleteAsync(userId);
            
        }
    }
}
