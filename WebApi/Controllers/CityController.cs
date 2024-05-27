
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    
    [Authorize]
    public class CityController :BaseController
    {
        private readonly IUnitOFWork _uow;
        private readonly IMapper _mapper;

        public CityController(IUnitOFWork uow,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
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
            var apiError = new ApiError();
            if (!string.IsNullOrEmpty(cityName) && 
                !await _uow.cityRepo.Exists(cityName))
            {
                City city = new City() { Name = cityName };
                await _uow.cityRepo.AddCityAsync(city);
                await _uow.SaveAsync();
                return StatusCode(201);
            }
            else
            {
                apiError.ErrorMessage = "Name is not valid";
                apiError.StatusCode = 300;
                return BadRequest(apiError);
            }
            
        }

        //post api/city/add?cityName=
        [HttpPost("post")]
        public async Task<IActionResult> PostAsync(CityDto city)
        {
            var apiError = new ApiError();
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
                apiError.ErrorMessage = "City name is not valid";
                apiError.StatusCode = 300;
                return BadRequest(apiError);
               
            }

        }

        //delete api/city/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var apiError = new ApiError();
            City city = await _uow.cityRepo.FindAsync(id);
            if(city == null)
            {
                apiError.ErrorMessage = "city is not found";
                apiError.StatusCode = 404;
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
            var apiError = new ApiError();
            if (id != city.Id)
            {
                apiError.ErrorMessage = "update is not allowed";
                apiError.StatusCode = 400;
                return BadRequest(apiError);
            }
            var c = await _uow.cityRepo.FindAsync(id);
            if (c == null)
            {
                apiError.ErrorMessage = "update is not allowed";
                apiError.StatusCode = 400;
                return BadRequest(apiError);
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
            var apiError = new ApiError();
            var c = await _uow.cityRepo.FindAsync(id);
            if(c == null)
            {
                apiError.ErrorMessage = "update is not allowed";
                apiError.StatusCode = 400;
                return BadRequest(apiError);
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
