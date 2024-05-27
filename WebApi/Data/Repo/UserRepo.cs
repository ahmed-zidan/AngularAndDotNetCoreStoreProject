using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
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
          
            var user = await _db.Users.Where(x => x.Name == name )//&& x.Password == pass)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return null;
            }

            byte[] passHash;
            using(var hmac = new HMACSHA512(user.PasswordKey))
            {
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
            for(int i = 0; i < passHash.Length; i++)
            {
                if(passHash[i] != user.Password[i])
                {
                    return null;
                }
            }


            return user;
        
        }

        public async Task<bool> IsExist(string name)
        {
            return await _db.Users.AnyAsync(x => x.Name == name);
        }

        public async Task Register(string name, string pass)
        {
            byte[] passHash, passKey;
            using(var hmac = new HMACSHA512())
            {
                passKey = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

            }
            await _db.Users.AddAsync(new User()
            {
                Name = name,
                Password = passHash,
                PasswordKey = passKey
            });
        }
    }
}
