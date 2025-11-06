using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Authorization;
using Domain.Constants;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        [HttpGet("public")]
        [AllowAnonymous] // Added this to explicitly allow public access
        public IActionResult PublicEndpoint()
        {
            return Ok(new { message = "This is a public endpoint" });
        }

        [HttpGet("user")]
        [AuthorizeUser]
        public IActionResult UserEndpoint()
        {
            return Ok(new { message = "This is a user-only endpoint" });
        }

        [HttpGet("admin")]
        [AuthorizeAdmin]
        public IActionResult AdminEndpoint()
        {
            return Ok(new { message = "This is an admin-only endpoint" });
        }

        [HttpGet("both")]
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        public IActionResult BothRolesEndpoint()
        {
            return Ok(new { message = "This endpoint is accessible by both admin and user roles" });
        }

        [HttpGet("claims")]
        [Authorize]
        public IActionResult GetUserClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new { claims });
        }
    }
}