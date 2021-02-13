using AuthServer.Core.DTOs;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseModel<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<ResponseModel<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
        Task<ResponseModel<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken);
        ResponseModel<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}