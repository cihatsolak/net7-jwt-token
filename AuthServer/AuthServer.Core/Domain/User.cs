using AuthServer.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace AuthServer.Core.Domain
{
    public class User : IdentityUser<string>, IEntityTable
    {
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
