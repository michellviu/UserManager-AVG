using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.DTOs;
using Core.Domain.Entities;
using Core.DomainService.Interfaces.Services;
using Infrastructure.Infrastructure.Persistence.Contexts;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Mappers
{

    [Mapper]
    public partial class UserMapper
    {
  
        private IUserService _userService;
        public UserMapper(IUserService userService)
        {
            _userService = userService;
        }

        [MapProperty(nameof(User.Email), nameof(UserDto.EmailAddress))]
        public partial UserDto MapToDto(User userEntity);

        public async Task<User> MapToEntity(UserDto userDto)
        {
            var user = await _userService.GetByIdAsync(userDto.Id);
            if (user == null)
            {
                throw new Exception("Entity not found");
            }
            return user;
        }
    }

}