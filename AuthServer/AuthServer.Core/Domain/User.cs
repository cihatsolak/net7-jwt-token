using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Domain
{
    public class User : IdentityUser<int>
    {
        public string CityName { get; set; }
    }
}
