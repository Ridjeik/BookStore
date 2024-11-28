using BLL.Models.Responses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using BLL.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BLL.Constants;
using DAL.Repositories.Implementation;
using System.Security.Claims;
using AutoMapper;
using DAL.Repositories.Interfaces;
using System.Security.Cryptography;
using BLL.Models.AppSettings;
using BLL.Models;

namespace BLL.Services.Implemantation
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings jwtSettings;
        private readonly TokenValidationParameters tokenValidationParameters;

        public TokenService(IOptions<JwtSettings> jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            IMapper mapper)
        {
            this.jwtSettings = jwtSettings.Value;
            this.tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthSuccessResponse> GenerateToken(UserDetailsDto user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("Id", user.Id),
                    new System.Security.Claims.Claim("NameIdentifier", user.Id),
                    new System.Security.Claims.Claim("Email", user.Email),
                    new System.Security.Claims.Claim("UserName", user.UserName),
                    new System.Security.Claims.Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.Add(this.jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);


            var result = new AuthSuccessResponse
            {
                UserName = user.UserName,
                Token = encryptedToken,
            };
            return result;
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var clonedTokenValidationParameters = this.tokenValidationParameters.Clone();
                clonedTokenValidationParameters.ValidateLifetime = false;
                var principal =
                    tokenHandler.ValidateToken(token, clonedTokenValidationParameters, out var validatedToken);
                return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
            }
            catch
            {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken token)
           => (token is JwtSecurityToken jwtSecurityToken)
              && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                  StringComparison.InvariantCultureIgnoreCase);
    }
}
