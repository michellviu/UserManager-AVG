﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainService.Interfaces.UnitWork
{
    public interface IUnitWork : IDisposable
    {
        Task SaveAsync();
    }
}
