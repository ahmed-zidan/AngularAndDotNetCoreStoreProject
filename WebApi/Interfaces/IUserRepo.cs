using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IUserRepo
    {
        Task<User> Authenticate(string name, string pass);
        Task Register(string name, string pass);
        Task<bool> IsExist(string name);
    }
}
