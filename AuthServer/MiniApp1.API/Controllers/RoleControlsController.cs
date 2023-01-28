using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp1.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoleControlsController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminCanAccess()
        {
            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult ManagerCanAccess()
        {
            return Ok();
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult EmployeeCanAccess()
        {
            return Ok();
        }
    }
}
