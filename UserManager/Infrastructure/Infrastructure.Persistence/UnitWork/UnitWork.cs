using Core.DomainService.Interfaces.Repository;
using Core.DomainService.Interfaces.UnitWork;
using Infrastructure.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure.Persistence.UnitWork
{
    public class UnitWork : IUnitWork, IDisposable
    {
        private readonly AppDBContext _context;
        public IUserRepository UserRepository { get; set; }

        public UnitWork(AppDBContext context,IUserRepository userRepository)
        {
            _context = context;
            UserRepository = userRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }
    }
 
}
