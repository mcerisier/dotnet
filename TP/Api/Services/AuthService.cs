using Api.Dtos;
using Api.Interfaces;
using Api.Models;
using Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config, ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
        }

        public async Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return (false, new List<string> { "Passwords do not match" });

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToList());

            // Ajouter automatiquement le rôle "client"
            await _userManager.AddToRoleAsync(user, "client");

            return (true, new List<string>());
        }

        public async Task<(bool Success, string Token, string RefreshToken, List<string> Errors)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return (false, "", "", new List<string> { "Invalid credentials" });

            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = user.Id
            });

            await _context.SaveChangesAsync();

            return (true, token, refreshToken, new List<string>());
        }

        public async Task<(bool Success, string Token, List<string> Errors)> RefreshTokenAsync(RefreshDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return (false, "", new List<string> { "User not found" });

            var tokenInDb = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken && r.UserId == user.Id);

            if (tokenInDb == null || tokenInDb.IsRevoked)
                return (false, "", new List<string> { "Invalid or expired refresh token" });

            var newToken = await GenerateJwtToken(user);
            return (true, newToken, new List<string>());
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool Success, List<string> Errors)> RegisterTechnicianAsync(RegisterTechnicianDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToList());

            await _userManager.AddToRoleAsync(user, "technicien");

            return (true, new List<string>());
        }

        public async Task<List<string>> GetAllTechniciansAsync()
        {
            return await _userManager.GetUsersInRoleAsync("technicien")
                .ContinueWith(task => task.Result.Select(u => u.Email).ToList());
        }


        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
