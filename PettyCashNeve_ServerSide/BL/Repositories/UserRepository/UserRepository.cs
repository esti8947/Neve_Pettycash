using DAL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly PettyCashNeveDbContext _context;

        public UserRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }

        public async Task<List<NdbUser>> GetUsersOfDepartment(int departmentId)
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.DepartmentId == departmentId)
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }
    }
}




