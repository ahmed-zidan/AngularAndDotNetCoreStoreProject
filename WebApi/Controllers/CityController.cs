
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IUnitOFWork _uow;
        private readonly IMapper _mapper;

        public CityController(IUnitOFWork uow,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var cities =  await _uow.cityRepo.GetCitiesAsync();
            var citiesDto = _mapper.Map<List<CityDto>>(cities); 
            return Ok(citiesDto);
        }
        //post api/city/add?cityName=
        [HttpPost("add")]
        //post api/city/add/cityName
        [HttpPost("add/{cityName}")]

        public async Task<IActionResult> PostAsync(string cityName)
        {
            if(!string.IsNullOrEmpty(cityName) && 
                !await _uow.cityRepo.Exists(cityName))
            {
                City city = new City() { Name = cityName };
                await _uow.cityRepo.AddCityAsync(city);
                await _uow.SaveAsync();
                return StatusCode(201);
            }
            else
            {
                return StatusCode(300);
            }
            
        }

        //post api/city/add?cityName=
        [HttpPost("post")]
        public async Task<IActionResult> PostAsync(CityDto city)
        {
            if (city!=null &&!string.IsNullOrEmpty(city.Name) &&
                !await _uow.cityRepo.Exists(city.Name))
            {
                City c = _mapper.Map<City>(city);
                
                await _uow.cityRepo.AddCityAsync(c);
                await _uow.SaveAsync();
                return StatusCode(201);
            }
            else
            {
                return StatusCode(300);
            }

        }

        //delete api/city/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            City city = await _uow.cityRepo.FindAsync(id);
            if(city == null)
            {
                return NotFound();
            }
            else
            {
                _uow.cityRepo.DeleteCityAsync(city);
                await _uow.SaveAsync();
                return Ok(id);
            }

        }
    }
}
