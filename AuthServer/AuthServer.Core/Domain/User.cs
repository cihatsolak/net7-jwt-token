using AuthServer.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Domain
{
    public class User : IdentityUser<string>, IEntityTable
    {
        public string Country { get; set; }
    }
}
