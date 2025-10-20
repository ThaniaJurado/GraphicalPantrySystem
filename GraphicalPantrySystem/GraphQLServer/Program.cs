using GraphQLServer.Data;
using GraphQLServer.MethodExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnection"),
    x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "blog")));

var app = builder.Build();

await app.ConfigurateMigrations();

app.MapGet("/", () => "Hello World!");

app.Run();
