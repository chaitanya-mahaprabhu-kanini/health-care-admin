﻿using MedicalReact.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseAdminReact.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Token1Controller : Controller
	{
		public IConfiguration _configuration;
		private readonly medicalDbContext _context;

		public Token1Controller(IConfiguration config, medicalDbContext context)
		{
			_configuration = config;
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> Post(User _userData)
		{
			if (_userData != null && _userData.UserEmail != null && _userData.Password != null)
			{
				var user = await GetUser(_userData.UserEmail, _userData.Password, _userData.Role);

				if (user != null)
				{
					//create claims details based on the user information
					var claims = new[] {
						new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
						 new Claim("UserId", user.UserId.ToString()),
						 new Claim("Email", user.UserEmail),
						new Claim("Password",user.Password),
						new Claim(ClaimTypes.Role, user.Role)

					};

					var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
					var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var token = new JwtSecurityToken(
						_configuration["Jwt:Issuer"],
						_configuration["Jwt:Audience"],
						claims,
						expires: DateTime.UtcNow.AddMinutes(60),
						signingCredentials: signIn);

					return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
				}
				else
				{
					return BadRequest("Invalid credentials");
				}
			}
			else
			{
				return BadRequest();
			}
		}

		private async Task<User> GetUser(string email, string password, string role)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email && u.Password == password && u.Role == role);
		}
	}
}
