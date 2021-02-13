using AuthServer.Core.Configuration;
using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        private IEnumerable<Claim> GetClaims(User user, List<string> audiences)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return claims;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString())
            };

            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return claims;
        }
        #endregion

        #region Methods
        public TokenDto CreateToken(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenSetting.AccessTokenExpiration); //Token Süresi
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenSetting.RefreshTokenExpiration); //Token Süresi

            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenSetting.SecurityKey); //Belirlediğim string şifreyi simetrik şifreye çeviriyorum

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //İmzalıyorum.

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _customTokenSetting.Issuer, //Sağlayıcı
                expires: accessTokenExpiration, //Token Süresi
                notBefore: DateTime.Now, //Şuanki saatten itibaren tokena süresini ekle ve geçerli olsun
                claims: GetClaims(user, _customTokenSetting.Audience),
                signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler(); //Token Oluşturacak arkadaş.
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,

                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }
        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenSetting.AccessTokenExpiration); //Token Süresi

            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenSetting.SecurityKey); //Belirlediğim string şifreyi simetrik şifreye çeviriyorum

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //İmzalıyorum.

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _customTokenSetting.Issuer, //Sağlayıcı
                expires: accessTokenExpiration, //Token Süresi
                notBefore: DateTime.Now, //Şuanki saatten itibaren tokena süresini ekle ve geçerli olsun
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler(); //Token Oluşturacak arkadaş.
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration
            };

            return tokenDto;
        }
        #endregion
    }
}
