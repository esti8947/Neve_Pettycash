using DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<List<NdbUser>> GetUsersOfDepartment(int departmentId);
    }
}
