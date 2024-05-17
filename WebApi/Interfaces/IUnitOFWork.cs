using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Interfaces
{
    public interface IUnitOFWork
    {
        ICityRepo cityRepo { get; }
        IUserRepo userRepo { get; }
        Task<bool> SaveAsync();

    }
}
