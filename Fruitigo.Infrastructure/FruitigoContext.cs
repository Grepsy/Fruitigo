using Fruitigo.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fruitigo.Infrastructure
{
    public class FruitigoContext : DbContext
    {
        public DbSet<FruitStock> Stock { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=Fruitigo;User Id=sa;Password=P@a$$w0rd;Trust Server Certificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FruitStock>()
                .OwnsMany(x => x.StockMutations);
        }

        public void SeedDatabase()
        {
            if (!Stock.Any())
            {
                var faker = new Bogus.Faker();
                var fruitTypes = Enum.GetValues(typeof(FruitType)).Cast<FruitType>().ToArray();
                var randomFruits = new Bogus.Faker<FruitStock>()
                    .RuleFor(f => f.Type, f => f.PickRandom(fruitTypes))
                    .RuleFor(f => f.ExpiryDate, f => DateOnly.FromDateTime(f.Date.Future(1).Date))
                    .RuleFor(f => f.OriginCountry, f => f.Address.Country())
                    .RuleFor(f => f.OrderPrice, f => f.Finance.Amount(0.5m, 10m))
                    .RuleFor(f => f.QuotePrice, (f, stock) => stock.OrderPrice + f.Random.Decimal(0.1m, 2m))
                    .Generate(20);

                foreach (var fruit in randomFruits)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        fruit.AddStock(Random.Shared.Next(999) + 1);
                    }
                }

                randomFruits[0].Id = new Guid("d8e2d1c1-a3b7-471b-b17f-083231852107");

                Stock.AddRange(randomFruits);
                SaveChanges();
            }
        }
    }
}
