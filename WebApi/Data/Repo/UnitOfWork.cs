using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;

namespace WebApi.Data.Repo
{
    public class UnitOfWork : IUnitOFWork
    {
        private readonly MyDbContext _db;
        public UnitOfWork(MyDbContext db)
        {
            _db = db;
        }
        public ICityRepo cityRepo => new CityRepo(_db);

        public async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
