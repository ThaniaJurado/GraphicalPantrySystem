using GraphQLServer.Data;
using GraphQLServer.GraphQL;
using GraphQLServer.MethodExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

//In case you want to implement Automapper. Personally, I don't like it but can be useful in a future
//builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .RegisterDbContextFactory<BlogContext>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddType<LocalDateType>();

// Register factory (IDbContextFactory<BlogContext>)
builder.Services.AddDbContextFactory<BlogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnection"),
    x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "blog")));

var app = builder.Build();

await app.ConfigurateMigrations();

app.UseCors(c => c.AllowAnyHeader().WithMethods("POST").AllowAnyOrigin());

app.MapGraphQL();

app.MapGet("/", () => "Hello World!");

app.Run();
