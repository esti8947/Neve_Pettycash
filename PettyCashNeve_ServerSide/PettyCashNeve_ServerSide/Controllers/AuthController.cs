//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.AspNetCore.Authorization;
//using DAL.Models;
//using BL.Services.AuthService;
//using PettyCashNeve_ServerSide.Dto;
//using BL.Services;

//namespace PettyCashNeve_ServerSide.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        IAuthService _authService;
//        public AuthController(IAuthService authService)
//        {
//            _authService = authService;
//        }

//        [HttpPost("Register")]
//        public async Task<ActionResult<ServiceResponse<int>>> Register(UserDto request)
//        {
//            var response = await _authService.Register(
//                new User { Username = request.Username }, request.Password
//            );
//            if (response.Success)
//            {
//                return BadRequest(response);
//            }
//            return Ok(response);
//        }

//        [HttpPost("Login")]
//        public async Task<ActionResult<ServiceResponse<int>>> Login(UserDto request)
//        {
//            var response = await _authService.Login(request.Username, request.Password);

//            if (!response.Success)
//            {
//                return BadRequest(response);
//            }
//            return Ok(response);
//        }
//    }
//}