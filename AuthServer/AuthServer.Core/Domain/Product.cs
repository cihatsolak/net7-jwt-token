using AuthServer.Core.Domain.Interfaces;

namespace AuthServer.Core.Domain
{
    public class Product : IEntityTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string UserId { get; set; }
    }
}
