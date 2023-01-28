using AuthServer.Core.Configuration;
using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using System.Collections.Generic;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user, IList<string> roles);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
