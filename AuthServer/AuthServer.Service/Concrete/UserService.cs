using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using AuthServer.Service.AutoMappers;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Service.Concrete
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        #endregion

        #region Ctor
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<ResponseModel<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(p => p.Description).ToList();
                return ResponseModel<UserDto>.Fail(new ErrorDto(errors, true), 400);
            }

            var userDto = ObjectMapper.Mapper.Map<UserDto>(user);

            return ResponseModel<UserDto>.Success(userDto, 201);
        }

        public async Task<ResponseModel<UserDto>> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return ResponseModel<UserDto>.Fail("UserName not found!", 404, true);

            var userDto = ObjectMapper.Mapper.Map<UserDto>(user);

            return ResponseModel<UserDto>.Success(userDto, 200);

        }
        #endregion
    }
}
