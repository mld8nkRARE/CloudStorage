using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Server.Dtos.Auth;
namespace Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<(bool Success, string? ErrorMessage, Guid? UserId)> RegisterAsync(RegisterRequestDto dto)
        {
            // Проверки
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return (false, "Email or password is empty", null);

            var existing = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email || u.Nickname == dto.Nickname);
            if (existing != null)
                return (false, "User with same email or nickname already exists", null);

            var user = new User
            {
                Email = dto.Email,
                Nickname = dto.Nickname
            };
            user.SetPasswordHash(BCrypt.Net.BCrypt.HashPassword(dto.Password));

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return (true, null, user.Id);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Неверный email или пароль");

            var token = GenerateJwtToken(user, out DateTime expiresAt);
            return new AuthResponseDto
            {
                Token = token,
                Expires = expiresAt
            };
        }

        private string GenerateJwtToken(User user, out DateTime expiresAt)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Nickname ?? string.Empty)
            };

            var keyString = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireHours = 3;
            if (int.TryParse(_config["Jwt:ExpiresHours"], out var h))
                expireHours = h;

            expiresAt = DateTime.UtcNow.AddHours(expireHours);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
