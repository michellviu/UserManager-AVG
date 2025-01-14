using Core.Domain.Entities;
using Core.DomainService.Interfaces.Repository;
using Infrastructure.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(AppDBContext context) : base(context)
        {
            _users = context.Set<User>();
        }
       
    }
}
