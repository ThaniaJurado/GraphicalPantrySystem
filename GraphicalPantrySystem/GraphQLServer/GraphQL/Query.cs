using GraphQLServer.Data;
using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.GraphQL
{
    public class Query
    {
        [UsePaging(DefaultPageSize =10)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Inventory> GetInventories([Service] BlogContext context) =>
            context.Inventories;
        //.Include(u => u.Item)
        //.ThenInclude(u => u.MeasurementUnit);

        [UseFirstOrDefault]
        [UseProjection]
        public IQueryable<Inventory?> GetInventory([Service] BlogContext context, int id)
            => context.Inventories.Where(u => u.Id == id);

        [UseProjection]
        public IQueryable<Items> GetItems([Service] BlogContext context) =>
        context.Items;

        [UseProjection]
        public IQueryable<MeasurementUnits> GetMeasurementUnits([Service] BlogContext context) =>
      context.MeasurementUnits;
    }
}
