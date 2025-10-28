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

        //This method is useful when you want to make a correction to an existing inventory record
        public async Task<InventoryPayload> UpdateToInventory([Service] IDbContextFactory<BlogContext> dbFactory, int id, InventoryInputType inputInventory)
        {
            await using var context = await dbFactory.CreateDbContextAsync();

            var inventory = new Inventory
            {
                Id = id,
                ItemId = inputInventory.ItemId,
                ExpirationDate = inputInventory.ExpirationDate
            };

            context.Inventories.Update(inventory);
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

        public async Task<bool> DeleteFromInventory([Service] IDbContextFactory<BlogContext> dbFactory, int id)
        {
            await using var context = await dbFactory.CreateDbContextAsync();

            try
            {
                var inventoryId = await context.Inventories.FindAsync(id);
                if (inventoryId != null)
                {
                    context.Inventories.Remove(inventoryId);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
