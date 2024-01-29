using Entities.Models_Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<string>> GetUserNameById(int  id);
        Task<ServiceResponse<UserDto>> LoginAsync(string username, string password);
    }
}
