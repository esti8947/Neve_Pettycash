using DAL.Models;
using PettyCashNeve_ServerSide.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.AuthService
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(string username, string password);
        Task<bool> RegisterUser(string username, string password);
    }
}
