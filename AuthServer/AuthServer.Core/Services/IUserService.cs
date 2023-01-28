using AuthServer.Core.DTOs;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IUserService
    {
        Task<ResponseModel<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<ResponseModel<UserDto>> GetUserByName(string userName);
        Task<ResponseModel<NoDataDto>> CreateUserRolesAsync(string userName);
    }
}
