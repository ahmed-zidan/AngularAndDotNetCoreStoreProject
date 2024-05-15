using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Data.Repo
{
    public class CityRepo : ICityRepo
    {
        private readonly MyDbContext _myContext;
        public CityRepo(MyDbContext myContext)
        {
            _myContext = myContext;
        }
        public async Task AddCityAsync(City city)
        {
            await _myContext.Cities.AddAsync(city);
        }
        
        public void DeleteCityAsync(City city)
        {
             _myContext.Cities.Remove(city);
        }

        public async Task<bool> Exists(string name)
        {
            return await _myContext.Cities.AnyAsync(x => x.Name == name);
        }

        public Task<City> FindAsync(int id)
        {
            return _myContext.Cities.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _myContext.Cities.ToListAsync();
        }

       
    }
}
