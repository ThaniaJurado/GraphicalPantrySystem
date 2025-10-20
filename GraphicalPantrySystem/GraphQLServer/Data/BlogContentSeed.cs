using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Data
{
    public class BlogContentSeed
    {
        public static async Task SeedAsync(BlogContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                await context.Database.OpenConnectionAsync();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GraphicalPantrySystemDB.MeasurementUnits ON");

                if (!context.MeasurementUnits.Any())
                {
                    var measurementUnits = new List<MeasurementUnits>() {
                    new MeasurementUnits(){ Id=1,Name="Kg", Description="Kilogramm"},
                    new MeasurementUnits(){ Id=1,Name="Mg", Description="Miligramm"},
                    new MeasurementUnits(){ Id=1,Name="Lt", Description="Litre"},
                };

                    await context.MeasurementUnits.AddRangeAsync(measurementUnits);
                    await context.SaveChangesAsync();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GraphicalPantrySystemDB.MeasurementUnits OFF");


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GraphicalPantrySystemDB.Items ON");

                if (!context.Items.Any())
                {
                    var items = new List<Items>() {
                    new Items(){ Id=1,Name="Valle verde beans", Description="Valle verde beans", MeasurementUnitId =1, Quantity=400},
                    new Items(){ Id=1,Name="Valle verde rice", Description="Valle verde rice", MeasurementUnitId =1, Quantity=400},
                    new Items(){ Id=1,Name="Zaragoza milk", Description="Milk", MeasurementUnitId =3, Quantity=1},
                };

                    await context.Items.AddRangeAsync(items);
                    await context.SaveChangesAsync();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GraphicalPantrySystemDB.Items OFF");


                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GraphicalPantrySystemDB.Inventory ON");

                if (!context.Inventories.Any())
                {
                    var inventory = new List<Inventory>() {
                    new Inventory(){ Id=1, ItemId = 1, ExpirationDate = new DateOnly(2026, 05, 02)},
                    new Inventory(){ Id=2, ItemId = 2, ExpirationDate = new DateOnly(2026, 07, 02)},
                    new Inventory(){ Id=3, ItemId = 3, ExpirationDate = new DateOnly(2025, 12, 02)},
                };

                    await context.Inventories.AddRangeAsync(inventory);
                    await context.SaveChangesAsync();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GraphicalPantrySystemDB.Inventory OFF");

                await context.Database.CloseConnectionAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<BlogContentSeed>();
                logger.LogError(ex, "There was an error in the initialization information to seed the database");
            }
        }
    }
}
