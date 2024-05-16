
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
            throw new UnauthorizedAccessException();
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
        
        //put api/city/update
        [HttpPut("update/{id}")]
        public async Task<IActionResult>UpdateAsync(int id,CityDto city)
        {
            if(id != city.Id)
            {
                return BadRequest("update is not allowed");
            }
            var c = await _uow.cityRepo.FindAsync(id);
            if (c == null)
            {
                return BadRequest("update is not allowed");
            }
            c.LastUpdated = DateTime.Now;
            c.LastUpdatedBy = 1;
            _mapper.Map(city , c);
          //  _uow.cityRepo.Update(c);
            await _uow.SaveAsync();
            return StatusCode(201);
        }
        //put api/city/update
        [HttpPut("updateCityName/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CityUpdateDto city)
        {
            var c = await _uow.cityRepo.FindAsync(id);
            if(c == null)
            {
                return BadRequest("update is not allowed");
            }
            c.LastUpdated = DateTime.Now;
            c.LastUpdatedBy = 1;
            _mapper.Map(city, c);
            //  _uow.cityRepo.Update(c);
            await _uow.SaveAsync();
            return StatusCode(201);
        }
    }
}
