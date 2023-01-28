using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp1.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClaimsController : ControllerBase
    {
        [Authorize(Roles = "Admin", Policy = "BritishPeople")]
        [HttpGet]
        public IActionResult ManagerAndBritish()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin", Policy = "TurkishPeople")]
        [HttpGet]
        public IActionResult ManagerAndTurkish()
        {
            return Ok();
        }
    }
}
