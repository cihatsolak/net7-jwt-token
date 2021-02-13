using AuthServer.Core.Configuration;
using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Settings;
using System;
using System.Security.Cryptography;

namespace AuthServer.Service.Concrete
{
    public class TokenService : ITokenService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly CustomTokenSetting _customTokenSetting;
        #endregion

        #region Ctor
        public TokenService(UserManager<User> userManager, IOptions<CustomTokenSetting> customTokenSetting)
        {
            _userManager = userManager;
            _customTokenSetting = customTokenSetting.Value;
        }
        #endregion

        #region Private Methods
        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }
        #endregion

        public TokenDto CreateToken(User user)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
