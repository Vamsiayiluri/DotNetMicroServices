using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Infrastructure.Services
{

    public class AuthService : IAuthService
    {
        private readonly IdentityDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(IdentityDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                throw new Exception("User already exists");

            var user = new User
            {
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GenerateTokens(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null ||
                !PasswordHasher.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return await GenerateTokens(user);
        }

        private async Task<AuthResponseDto> GenerateTokens(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            // 🔥 Revoke old tokens (rotation strategy)
            foreach (var token in user.RefreshTokens)
            {
                token.IsRevoked = true;
            }

            user.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token == null || token.IsRevoked || token.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            // 🔥 Rotate token
            token.IsRevoked = true;

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            newRefreshToken.UserId = token.UserId;

            token.User.RefreshTokens.Add(newRefreshToken);

            var accessToken = _tokenService.GenerateAccessToken(token.User);

            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
        public async Task RevokeTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token == null)
                throw new Exception("Token not found");

            token.IsRevoked = true;

            await _context.SaveChangesAsync();
        }
    }
}
