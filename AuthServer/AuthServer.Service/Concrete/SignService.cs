using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthServer.Service.Concrete
{
    static class SignService
    {
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
