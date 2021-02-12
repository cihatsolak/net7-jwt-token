using System.Collections.Generic;

namespace AuthServer.Core.Configuration
{
    public class Client
    {
        public int Id { get; set; }
        public string Secret { get; set; }
        public List<string> Audiences { get; set; }
    }
}
