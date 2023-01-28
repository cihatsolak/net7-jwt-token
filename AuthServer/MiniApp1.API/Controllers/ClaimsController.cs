using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp1.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClaimsController : ControllerBase
    {
        [Authorize(Roles = "Admin, Manager", Policy = "BritishPeople")]
        [HttpGet]
        public IActionResult ManagerAndBritish()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin, Manager", Policy = "TurkishPeople")]
        [HttpGet]
        public IActionResult ManagerAndTurkish()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin, Manager", Policy = "TurkishAndOver18YearsOld")]
        [HttpGet]
        public IActionResult TurkishButHisAgeIs17YearsOld()
        {
            return Ok();
        }
    }
}
