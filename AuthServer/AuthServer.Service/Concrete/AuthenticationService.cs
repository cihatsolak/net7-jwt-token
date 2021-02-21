using AuthServer.Core.Configuration;
using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Service.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Ctor
        public AuthenticationService(
            IOptions<List<Client>> clients,
            ITokenService tokenService,
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IGenericRepository<UserRefreshToken> userRefreshTokenRepository)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<ResponseModel<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto), "loginDto null");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return ResponseModel<TokenDto>.Fail("Email or password is wrong!", 400, true);

            bool checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!checkPassword)
                return ResponseModel<TokenDto>.Fail("Email or password is wrong!", 400, true);

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenRepository.GetAllByFilter(p => p.UserId.Equals(user.Id)).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;

                _userRefreshTokenRepository.Update(userRefreshToken);
            }

            await _unitOfWork.SaveChangesAsync();

            return ResponseModel<TokenDto>.Success(token, 200);
        }

        public ResponseModel<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(p => p.Id.Equals(clientLoginDto.ClientId, StringComparison.OrdinalIgnoreCase) && p.Secret.Equals(clientLoginDto.ClientSecret, StringComparison.OrdinalIgnoreCase));
            if (client == null)
                return ResponseModel<ClientTokenDto>.Fail("Client id or ClientSecret not found!", 404, true);

            var token = _tokenService.CreateTokenByClient(client);

            return ResponseModel<ClientTokenDto>.Success(token, 200);
        }

        public async Task<ResponseModel<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var userRefreshToken = await _userRefreshTokenRepository.GetAllByFilter(p => p.Code.Equals(refreshToken)).SingleOrDefaultAsync();
            if (userRefreshToken == null)
                return ResponseModel<TokenDto>.Fail("Refresh token not found", 404, true);

            var user = await _userManager.FindByIdAsync(userRefreshToken.UserId);
            if (user == null)
                return ResponseModel<TokenDto>.Fail("User not found", 404, true);

            var tokenDto = _tokenService.CreateToken(user);

            userRefreshToken.Code = tokenDto.RefreshToken;
            userRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            _userRefreshTokenRepository.Update(userRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return ResponseModel<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<ResponseModel<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var userRefreshToken = await _userRefreshTokenRepository.GetAllByFilter(p => p.Code.Equals(refreshToken)).SingleOrDefaultAsync();
            if (userRefreshToken == null)
                return ResponseModel<NoDataDto>.Fail("Refresh token not found", 404, true);

            _userRefreshTokenRepository.Remove(userRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return ResponseModel<NoDataDto>.Success(200);
        }
        #endregion
    }
}
