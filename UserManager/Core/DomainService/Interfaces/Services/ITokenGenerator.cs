using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainService.Interfaces.Services
{
    public interface ITokenGenerator
    {
        Task<string> GenerateJwtToken(User user);
    }
}
