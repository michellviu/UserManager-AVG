using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainService.Interfaces.Services
{
    //CRUD operations for all entities
    public interface IGenericService<T> where T : class
    {
        public Task<List<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task AddAsync(T entity);
        public Task DeleteAsync(int id);
        public Task UpdateAsync(int id, T entity);
    }
}
