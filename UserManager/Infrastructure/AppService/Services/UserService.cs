using Core.Domain.Entities;
using Core.DomainService.Interfaces.Services;
using Core.DomainService.Interfaces.UnitWork;
using Infrastructure.Infrastructure.Persistence.UnitWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppService.Services
{
    public class UserService : IUserService
    {
        private readonly UnitWork unitWork;
        public UserService(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public async Task AddAsync(User entity)
        {
           await unitWork.UserRepository.AddAsync(entity);
           await unitWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await unitWork.UserRepository.DeleteAsync(id);
            await unitWork.SaveAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await unitWork.UserRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await unitWork.UserRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int id, User entity)
        {
            var entityToUpdate = await GetByIdAsync(id);
            if (entityToUpdate == null)
            {
                throw new Exception("Entity not found");
            }

            entityToUpdate.Email = entity.Email;
            entityToUpdate.UserName = entity.UserName;

            unitWork.UserRepository.Update(entityToUpdate);
            await unitWork.SaveAsync();
        }
    }
}
