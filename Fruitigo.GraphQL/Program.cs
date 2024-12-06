using Fruitigo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FruitigoContext>();

builder
    .AddGraphQL()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddTypes()
    .AddMutationConventions(applyToAllMutations: true)
    .AddMutationType<Mutation>();


var app = builder.Build();

using var context = new FruitigoContext();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
context.SeedDatabase();
Console.WriteLine("Database seeded with random data.");

app.MapGraphQL();
app.RunWithGraphQLCommands(args);

public partial class Program { }