using System;

namespace AuthServer.Core.Domain
{
    public class UserRefreshToken
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
