using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
namespace Entities.Models_Dto.UserDto
{
    public class UserInfoModel
    {
        public string Id { get; set; }
        public string Username {  get; set; }
        public int DepartmentId { get; set; }
        public bool IsManager { get; set; }
        public bool LoggedIn { get; set; }
        public string Token { get; set; }
        public  Department Department { get; set; }
        public string email { get; set; }
        public string phoneNubmer { get; set; }

    }
}
