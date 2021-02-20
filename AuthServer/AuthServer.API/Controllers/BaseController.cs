using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult ActionResultInstance<TResponseModel>(ResponseModel<TResponseModel> responseModel) where TResponseModel : class
        {
            return new ObjectResult(responseModel)
            {
                StatusCode = responseModel.StatusCode
            };
        }
    }
}
