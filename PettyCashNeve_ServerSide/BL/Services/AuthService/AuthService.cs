//using System;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Security.Claims;
//using System.Text;
//using DAL.Models;
//using Microsoft.Extensions.Configuration;
//using PettyCashNeve_ServerSide.Dto;
//using BL.Repositories.UserRepository;

//namespace BL.Services.AuthService
//{
//    public class AuthService : IAuthService
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly string _jwtKey;
//        private readonly string _jwtIssuer;
//        private readonly string _jwtAudience;
//        public AuthService(IUserRepository userRepository, string jwtKey, string jwtIssuer, string jwtAudience)
//        {
//            _userRepository = userRepository;
//            _jwtKey = jwtKey;
//            _jwtIssuer = jwtIssuer;
//            _jwtAudience = jwtAudience;
//        }

//        public async Task<string> GenerateJwtToken(string username, string password)
//        {
//            var user = await _userRepository.GetUserByUsernameAndPassword(username, password);
//            if (user != null)
//            {
//                var tokenDescriptor = new SecurityTokenDescriptor
//                {
//                    Subject = new ClaimsIdentity(new[]
//                    {
//                        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//                    }),
//                    Expires = DateTime.UtcNow.AddMinutes(15),
//                    Issuer = _jwtIssuer,
//                    Audience = _jwtAudience,
//                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
//                    SecurityAlgorithms.HmacSha256Signature)
//                };

//                var tokenHandler = new JwtSecurityTokenHandler();
//                var token = tokenHandler.CreateToken(tokenDescriptor);
//                return tokenHandler.WriteToken(token);
//            }

//            return null;
//        }

//        public Task<bool> RegisterUser(string username, string password)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
