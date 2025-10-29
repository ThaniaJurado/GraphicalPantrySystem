using GraphQLServer.Models;

namespace GraphQLServer.GraphQL.Types
{
    public class ItemInputType
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public int MeasurementUnitId { get; set; }
    }
}
