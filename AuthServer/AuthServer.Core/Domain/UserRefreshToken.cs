using AuthServer.Core.Domain.Interfaces;
using System;

namespace AuthServer.Core.Domain
{
    public class UserRefreshToken : IEntityTable
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
