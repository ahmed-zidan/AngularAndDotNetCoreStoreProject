using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOFWork _uow;
        private readonly IMapper _mapper;
        public UserController(IUnitOFWork uow , IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        //post api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> login(UserLogin userDto)
        {
            
           var user = await _uow.userRepo.Authenticate(userDto.Name , userDto.Password);
            if(user == null)
            {
                return Unauthorized();
            }
            else
            {
                UserLoginResDto res = new UserLoginResDto()
                {
                    Name = user.Name,
                    Token = "token res"
                };
                return Ok(res);
            }
        }
    }
}
