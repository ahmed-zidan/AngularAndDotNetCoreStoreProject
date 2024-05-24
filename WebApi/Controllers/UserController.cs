using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public UserController(IUnitOFWork uow , IMapper mapper,IConfiguration configuration)
        {
            _uow = uow;
            _mapper = mapper;
            _configuration = configuration;
        }

        //post api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> login(UserLogin userDto)
        {

            var user = await _uow.userRepo.Authenticate(userDto.Name, userDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                var token = GenerateToken(user);

                UserLoginResDto res = new UserLoginResDto()
                {
                    Name = user.Name,
                    expired = token.ValidTo,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
                return Ok(res);
            }
        }

            private SecurityToken GenerateToken(User user)
            {
            var secretKey = _configuration.GetSection("AppSetting:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes("shh.. keyjsdfjs sfjsk sdj"));

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
                    Expires = DateTime.Now.AddMinutes(1),
                    SigningCredentials = signingCredintiels,

                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescription);
                return token;

            }

        }
       
    }

