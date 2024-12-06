using Fruitigo.Domain;
using Fruitigo.Infrastructure;
using HotChocolate.Data;

namespace Fruitigo.GraphQL;

[QueryType]
public static class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public static IQueryable<FruitStock> GetStock(FruitigoContext context) => context.Stock.AsQueryable();
}
