using GraphQLServer.Data;
using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.GraphQL
{
    public class Query
    {
        public IQueryable<Inventory> GetInventories([Service] BlogContext context) =>
            context.Inventories
            .Include(u => u.Item)
            .ThenInclude(u => u.MeasurementUnit);
    }
}
