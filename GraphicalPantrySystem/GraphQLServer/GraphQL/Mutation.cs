using GraphQLServer.Data;
using GraphQLServer.GraphQL.Types;
using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.GraphQL
{
    public class Mutation
    {
        public async Task<InventoryPayload> AddToInventory([Service] IDbContextFactory<BlogContext> dbFactory, InventoryInputType inputInventory)
        {
            await using var context = await dbFactory.CreateDbContextAsync();

            var inventory = new Inventory
            {
                ItemId = inputInventory.ItemId,
                ExpirationDate = inputInventory.ExpirationDate
            };

            await context.Inventories.AddAsync(inventory);
            await context.SaveChangesAsync();

            var item = await context.Items.FindAsync(inventory.ItemId);

            return new InventoryPayload
            {
                Id = inventory.Id,
                ItemId = inventory.ItemId,
                Item = item,
                ExpirationDate = inventory.ExpirationDate
            };
        }
    }
}
