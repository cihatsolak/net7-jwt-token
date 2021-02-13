using System.Collections.Generic;

namespace AuthServer.Core.Configuration
{
    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }

        //Üyelik sistemi olmayan hangi api'lere erişebilir.
        public List<string> Audiences { get; set; }
    }
}
