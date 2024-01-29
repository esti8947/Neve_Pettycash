using BL.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PettyCashNeve_ServerSide.Dto;

namespace PettyCashNeve_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getUserNameById/{userId}")]
        public async Task<IActionResult> GetUserNameById(int userId)
        {
            var serviceResponse = await _userService.GetUserNameById(userId);
            return HandleResponse(serviceResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var serviceResponse = await _userService.LoginAsync(loginRequest.Username, loginRequest.Password);
            return HandleResponse(serviceResponse);
        }
    }
}
