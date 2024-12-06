using Fruitigo.Domain;
using Fruitigo.Infrastructure;

namespace Fruitigo.GraphQL;

public class Mutation
{
    public async Task<FruitStock?> AddStock(FruitigoContext context, Guid id, int quantity)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        var fruitStock = (await context.Stock.FindAsync(id));
        if (fruitStock != null)
        {
            fruitStock.AddStock(quantity);
            await context.SaveChangesAsync();
        }

        return fruitStock;
    }

    public async Task<FruitStock?> RemoveStock(FruitigoContext context, Guid id, int quantity)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        var fruitStock = (await context.Stock.FindAsync(id));
        if (fruitStock != null)
        {
            fruitStock.RemoveStock(quantity);
            await context.SaveChangesAsync();
        }

        return fruitStock;
    }
}