using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    
    public class UserController :BaseController
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

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes("key"));

            var claims = new Claim[]
            {
                 new Claim(ClaimTypes.Name,user.Name),
                 new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };

            var signingCredintiels = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = signingCredintiels,

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);

        }
    }
}
