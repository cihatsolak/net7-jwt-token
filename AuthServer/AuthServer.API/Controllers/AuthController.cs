using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByUser(LoginDto loginDto)
        {
            var tokenDto = await _authenticationService.CreateTokenAsync(loginDto);
            return ActionResultInstance<TokenDto>(tokenDto);
        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var tokenDto = _authenticationService.CreateTokenByClient(clientLoginDto);
            return ActionResultInstance<ClientTokenDto>(tokenDto);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var noDataDto = await _authenticationService.RevokeRefreshTokenAsync(refreshTokenDto.Token);
            return ActionResultInstance<NoDataDto>(noDataDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var tokenDto = await _authenticationService.CreateTokenByRefreshTokenAsync(refreshTokenDto.Token);
            return ActionResultInstance<TokenDto>(tokenDto);
        }
    }
}
