using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var userDto = await _userService.CreateUserAsync(createUserDto);
            return ActionResultInstance<UserDto>(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string username)
        {
            var noDataDto = await _userService.CreateUserRolesAsync(username);
            return ActionResultInstance<NoDataDto>(noDataDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            string username = User.Identity.Name;
            var userDto = await _userService.GetUserByName(username);
            return ActionResultInstance<UserDto>(userDto);
        }
    }
}
