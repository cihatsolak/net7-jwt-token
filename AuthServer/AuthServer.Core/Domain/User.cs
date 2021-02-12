using AuthServer.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Domain
{
    public class User : IdentityUser<int>, IEntityTable
    {
        public string CityName { get; set; }
    }
}
