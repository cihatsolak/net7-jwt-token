using AuthServer.Core.Configuration;
using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
