using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AccountController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            Console.WriteLine(registerDto.Email + " " + registerDto.Password);
            if (await UserExists(registerDto.Email)) return BadRequest("User already taken");
            using var hmac = new HMACSHA512();
            var user = new User
            {
                Email = registerDto.Email.ToLower(),
                Username = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return new UserDto
            {
                Email = registerDto.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized("Invalid User");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid User");
            }
            return new UserDto
            {
                Email = loginDto.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email) != null;
        }
    }
}