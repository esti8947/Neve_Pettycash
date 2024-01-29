// Controllers/BaseController.cs
using Microsoft.AspNetCore.Mvc;
using BL.Services;
using PettyCashNeve_ServerSide.Dto;
using PettyCashNeve_ServerSide.Services.DepartmentService;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Claims;
using PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService;
using Microsoft.AspNetCore.Identity;

namespace PettyCashNeve_ServerSide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {

        protected string UserId
        {
            get
            {
                return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            }
        }



        protected IActionResult HandleResponse<T>(ServiceResponse<T> serviceResponse)
        {
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                {
                    return BadRequest(new { success = false, message = serviceResponse.Message });
                }
            }
        }

        protected IActionResult HandleResponses(params object[] responses)
        {
            foreach (var response in responses)
            {
                if (response is IActionResult)
                {
                    // If the response is already an IActionResult, return it as is
                    return (IActionResult)response;
                }
                else if (response is ServiceResponse<bool> boolServiceResponse)
                {
                    // Handle ServiceResponse<bool> as needed
                    if (boolServiceResponse.Success)
                    {
                        // You can customize the response based on the boolean value
                        return Ok(new { success = true, message = "Successfully processed the boolean response." });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = boolServiceResponse.Message });
                    }
                }
                else if (response is ServiceResponse<object> serviceResponse)
                {
                    // Handle ServiceResponse<T> as needed, for example, convert it to IActionResult
                    if (serviceResponse.Success)
                    {
                        return Ok(serviceResponse);
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = serviceResponse.Message });
                    }
                }
                else
                {
                    // Handle other types of responses as needed
                }
            }

            // Default response if none of the above conditions are met
            return BadRequest(new { success = false, message = "Unknown response type." });
        }

    }
}