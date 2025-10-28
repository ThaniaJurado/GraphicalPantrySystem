using GraphQLServer.Models;

namespace GraphQLServer.GraphQL.Types
{
    public class InventoryInputType
    {
        public int ItemId { get; set; }
        public Items Item { get; set; }
        public DateOnly ExpirationDate { get; set; }
    }
}
