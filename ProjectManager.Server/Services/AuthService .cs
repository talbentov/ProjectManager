using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Data;
using ProjectManager.Server.Dtos;
using ProjectManager.Server.Models;

namespace ProjectManager.Server.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _jwtSecret = "Jf93nfk@#Rjlwe9fJf93nfk@#Rjlwe9fJf93nfk@#Rjlwe9f"; // Normally would be stored securely in environment variables

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hash = HashPassword(password);
            return hash == hashedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return null;
            }

            // Create new user
            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                JwtToken = GenerateJwtToken(user),
                Username = user.Username,
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            // if user or password not correct
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            return new AuthResponseDto
            {
                JwtToken = GenerateJwtToken(user),
                Username = user.Username,
            };
        }
    }
}
