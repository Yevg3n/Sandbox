﻿using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // TODO: Take JSON instead of parameters
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // TODO: Ensure that the provided data is valid and meets the required criteria
            // TODO: Check if username already exists in the database
            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = registerDto.Username.ToLower(),
                // TODO: Consider using the Task.Run pattern to perform the CPU-intensive password hashing asynchronously.
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == loginDto.Username);
            if (user == null) return Unauthorized();

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            bool valid = StructuralComparisons.StructuralEqualityComparer.Equals(PasswordHash, user.PasswordHash);
            if (!valid) return Unauthorized("Invalid Password");

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
