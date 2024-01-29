//using DAL.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BL.Repositories.UserOfDepartmentRepository
//{
//    public class UserOfDepartmentRepository : IUserOfDepartmentRepository
//    {
//        private readonly PettyCashNeveDbContext _context;
//        public UserOfDepartmentRepository(PettyCashNeveDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<int> GetUserIdById(int userOfDepartmentId)
//        {
//            var userId = await _context.UserOfDepartments.Where(ud => ud.Id == userOfDepartmentId)
//                .Select(ud => ud.UserId).FirstOrDefaultAsync();
//            return (int)userId;
//        }

//        public async Task<UserOfDepartment> GetUserOfDepartmentByIdAsync(int userOfDepartmentId)
//        {
//            var userOfDepartment = await _context.UserOfDepartments.FirstOrDefaultAsync(ud => ud.Id == userOfDepartmentId);
//            return userOfDepartment;
//        }
//    }
//}
