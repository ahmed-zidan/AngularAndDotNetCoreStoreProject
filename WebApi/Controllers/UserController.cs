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
using WebApi.Errors;
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


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO model)
        {
            ApiError apiError = new ApiError();
            if (!ModelState.IsValid)
            {
                apiError.ErrorMessage = "model is not correct";
                apiError.StatusCode = 500;
                return BadRequest(apiError);
            }
            if (await _uow.userRepo.IsExist(model.Name))
            {
                apiError.ErrorMessage = "user already exist";
                apiError.StatusCode = 400;
                return BadRequest(apiError);
            }

            await _uow.userRepo.Register(model.Name , model.Password);
            await _uow.SaveAsync();
            return StatusCode(201);

        }

        //post api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> login(UserLogin userDto)
        {
            var apiError = new ApiError();
            var user = await _uow.userRepo.Authenticate(userDto.Name, userDto.Password);
            if (user == null)
            {
                apiError.ErrorMessage = "Invalid user ID Or Password";
                apiError.StatusCode = 401;
                return Unauthorized(apiError);
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

