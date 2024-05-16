using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICityRepo
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        void DeleteCityAsync(City city);
        Task AddCityAsync(City city);
        Task<bool> Exists(string name);
        Task<City> FindAsync(int id);
        void Update(City city);
      
    }
}
