using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvocesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInvocies()
        {
            var userName = HttpContext.User.Identity.Name;
            return Ok($"username: {userName}");
        }
    }
}
