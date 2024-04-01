using BL.Services.EmailService;
using DAL.Data;
using DAL.Models;
using Entities.Models_Dto.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PettyCashNeve_ServerSide.Repositories.DepartmentRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseUserController
    {
        private readonly IConfiguration _configuration;
        private Microsoft.AspNetCore.Identity.UserManager<NdbUser> _userManager;
        private SignInManager<NdbUser> _signInManager;
        private IDepartmentRepository _departmentRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        PettyCashNeveDbContext _dbContext;

        public AccountController(UserManager<NdbUser> userManager,
                                 SignInManager<NdbUser> signInManager,
                                 PettyCashNeveDbContext dbContext,
                                 IConfiguration configuration,
                                 IDepartmentRepository departmentRepository,
                                 RoleManager<IdentityRole> roleManager,
                                 IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _configuration = configuration;
            _departmentRepository = departmentRepository;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [Route("Register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                // Check if a user with the same username already exists
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null)
                {
                    // Username is not unique
                    return BadRequest("Username already exists");
                }

                var newUser = new NdbUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    DepartmentId = model.DepartmentId,
                    IsActive = true
                };

                // Attempt to create the user
                Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(newUser, model.PasswordHash);
                if (result.Succeeded)
                {
                    if (model.IsAdmin)
                    {
                        if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                        }
                        await _userManager.AddToRoleAsync(newUser, Roles.Admin);
                    }
                    return Ok();
                }
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            NdbUser appUser = await _userManager.FindByNameAsync(model.Username);
            if (appUser == null || appUser.IsActive == false)
                return BadRequest();

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result == SignInResult.Success)
            {
                UserInfoModel userInfo = await GetUserInfoByName(model.Username);


                var authClaims = new List<Claim>
                {
                   new Claim(ClaimTypes.Name, userInfo.Username),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                };

                if (userInfo.Department != null)
                {
                    authClaims.Add(new Claim(ClaimTypes.PrimaryGroupSid, userInfo.Department.DepartmentId.ToString()));
                }

                var userRole = await _userManager.GetRolesAsync(appUser);
                foreach (var role in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                string token = GenerateToken(authClaims);
                userInfo.Token = token;

                return Ok(userInfo);
            }
            else
            {
                return Forbid("Unknown username or bad password.");
            }
        }


        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT").GetValue<string>("Secret")));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [Route("~/auth/current")]
        [HttpGet]
        [Authorize]
        public async Task<UserInfoModel?> GetCurrentUserInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                return await GetUserInfoByName(User.Identity.Name);
            }
            else
            {
                return null;
            }
        }

        [Route("~/api/GetUsernameByUserId/{userId}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsernameByUserId(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(user.UserName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the username: {ex.Message}");
            }
        }



        [Route("~/api/GetDepartments")]
        [HttpGet]
        [AllowAnonymous]
        public List<Department> GetDepartments()
        {
            return _dbContext.Departments.ToList();

        }

        [Route("~/api/GetUserDepartment")]
        [HttpGet]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetUserDepartment()
        {
            //get departments from claims

            var dep = GetDepartment();
            return Ok(dep);
        }



        [Route("deleteUser/{username}")]
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                // Find the user by username
                var userToDelete = await _userManager.FindByNameAsync(username);

                if (userToDelete == null)
                {
                    // User not found
                    return NotFound("User not found.");
                }

                // Delete the user
                var result = await _userManager.DeleteAsync(userToDelete);

                if (result.Succeeded)
                {
                    return Ok("User deleted successfully.");
                }
                else
                {
                    return BadRequest(result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("deactivateUser/{username}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateUser(string username)
        {
            try
            {
                // Find the user by username
                var userToDeactivate = await _userManager.FindByNameAsync(username);

                if (userToDeactivate == null)
                {
                    // User not found
                    return NotFound("User not found.");
                }

                // Deactivate the user
                userToDeactivate.IsActive = false;
                var result = await _userManager.UpdateAsync(userToDeactivate);

                if (result.Succeeded)
                {
                    return Ok(new { message = "User deactivated successfully." });
                }
                else
                {
                    return BadRequest(result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deactivating the user: {ex.Message}");
            }
        }


        [Route("updateUser")]
        [HttpPost]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel updatedUser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(updatedUser.Id);
                if(user == null)
                {
                    return NotFound("User not found");
                }

                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    _dbContext.SaveChanges();
                    return Ok();
                    //return Ok("User updated successfully");
                }
                else
                {
                    return BadRequest(result.Errors.Select(e => e.Description));
                }
            }catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the user: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("restorePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RestorePassword(RestorePasswordModel model)
        {
            try
            {
                // Verify user identity based on username and email
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || user.Email != model.Email)
                {
                    // User not found or email does not match
                    return BadRequest("Invalid username or email.");
                }

                // Generate a new password
                string newPassword = GenerateRandomPassword(); // Implement this method to generate a random password

                // Reset the user's password
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (resetResult.Succeeded)
                {

                    await _emailService.SendEmailAsync(model.Email, "Password Reset", $"Your new password is: {newPassword}");
                    _emailService.Send(model.Email, "Password Reset", $"Your new password is: {newPassword}", "ester38947@gmail.com");

                    return Ok("Password reset successfully. New password sent to your email.");
                }
                else
                {
                    // Password reset failed
                    return BadRequest("Failed to reset password.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [Route("getUsersByDepartmentId/{departmentId}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersByDepartmentId(int departmentId)
        {
            try
            {
                var userList = await _departmentRepository.GetUsersByDepartmentId(departmentId);

                userList = userList.Where(u => u.IsActive).ToList();

                // Create a list to store user information
                List<UserInfoModel> userInfoList = new List<UserInfoModel>();

                // Iterate over the user list and call GetUserInfoByName for each user
                foreach (var user in userList)
                {
                    var userInfo = await GetUserInfoByName(user.UserName);
                    userInfoList.Add(userInfo);
                }
                userInfoList.ForEach(u => u.Department = null);

                return Ok(userInfoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user list: {ex.Message}");
            }
        }



        private string GetDepartment()
        {
            if (User.Identity.IsAuthenticated)
            {
                //get claims
                var departmentId = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.PrimaryGroupSid);
                return departmentId?.Value;
            }
            return null;
        }

        private bool GetIsManager()
        {
            bool isManagerRes = false;

            if (User.Identity.IsAuthenticated)
            {
                //get claims
                var ismanager = User.Claims.FirstOrDefault(p => p.Type == "IsManager");
                return Boolean.TryParse(ismanager?.Value, out isManagerRes);
            }
            return isManagerRes;
        }

        private async Task<UserInfoModel> GetUserInfoByName(string username)
        {
            return await GetUserInfoBy(user => user.UserName == username, username);
        }
        private bool UserHasActiveReminders(string remind)
        {
            return true;
            //return ReportService.HasReminders(remind);

        }

        private async Task<UserInfoModel> GetUserInfoBy(Expression<Func<NdbUser, bool>> predicate, string username)
        {
            NdbUser appUser = await _userManager.FindByNameAsync(username);
            var isAdminRole = await _userManager.IsInRoleAsync(appUser, Roles.Admin);

            var user = _dbContext.Users
                .Where(predicate)
                .Select(user => new UserInfoModel
                {
                    Username = user.UserName,
                    LoggedIn = true,
                    DepartmentId = user.DepartmentId,
                    Id = user.Id,
                    IsManager = isAdminRole,
                })
                .SingleOrDefault();
            if (user != null && user.DepartmentId != 0)
            {
                user.Department = await _departmentRepository.GetDepartmentByIdAsync(user.DepartmentId);
            }

            return user;
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
    public static class Roles
    {
        public const string Admin = "Admin";
    }


    public class LoginResultModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
    public class UpdateUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class RestorePasswordModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }

}
