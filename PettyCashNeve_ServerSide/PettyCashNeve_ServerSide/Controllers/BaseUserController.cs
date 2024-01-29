using DAL.Data;
using DAL.Models;
using Entities.Models_Dto.UserDto;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseUserController : ControllerBase
    {
        private UserInfoModel GetUserInfoByName(string username)
        {
            return GetUserInfoBy(user => user.UserName == username, username);
        }
        private bool UserHasActiveReminders(string remind)
        {
            return true;
            //return ReportService.HasReminders(remind);
        }

        private UserInfoModel GetUserInfoBy(Expression<Func<NdbUser, bool>> predicate, string username)
        {;
            var services = HttpContext.RequestServices;
            PettyCashNeveDbContext dbContext = services.GetService<PettyCashNeveDbContext>();
            // var canSeeAllComments = Db.CommentUserGroups.Any(cug => cug.CommentUser == User.Identity.Name  && cug.CommentUserGroup1 == "ViewAll");
            bool hasActiveReminders = UserHasActiveReminders(username);
            return dbContext.Users
                .Where(predicate)
                .Select(user => new UserInfoModel
                {
                    Username = user.UserName,
                    LoggedIn = true,
                })
                .SingleOrDefault();
           
        }
    }
}
