using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Data.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly MyDbContext _db;
        public UserRepo(MyDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<User> Authenticate(string name, string pass)
        {
            var user = await _db.Users.Where(x => x.Name == name && x.Password == pass)
                .FirstOrDefaultAsync();
            return user;
        
        }
    }
}
