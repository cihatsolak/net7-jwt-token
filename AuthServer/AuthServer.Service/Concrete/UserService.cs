using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using AuthServer.Service.AutoMappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region Ctor
        public UserService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<ResponseModel<NoDataDto>> CreateUserRolesAsync(string userName)
        {
            await CheckRolesAsync();

            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                return ResponseModel<NoDataDto>.Fail("user not found!", 404, true);
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            await _userManager.AddToRoleAsync(user, "Manager");

            return ResponseModel<NoDataDto>.Success(StatusCodes.Status201Created);
        }
        #endregion

        private async Task CheckRolesAsync()
        {
            bool adminRoleExist = await _roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin"
                });
            }

            bool managerRoleExist = await _roleManager.RoleExistsAsync("Manager");
            if (!managerRoleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Manager"
                });
            }
        }
    }
}
