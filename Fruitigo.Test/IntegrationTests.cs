namespace Fruitigo.Test
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;
    using Xunit;

    public class FruitApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public IFruitigoClient Client { get; }

        // Run against factory, can't make it work
        //public FruitApiIntegrationTests(WebApplicationFactory<Program> factory)
        //{
        //    _factory = factory;

        //    var serviceCollection = new ServiceCollection();
        //    serviceCollection
        //        .AddHttpClient(FruitigoClient.ClientName, client => client.BaseAddress = _factory.Server.BaseAddress)
        //        .ConfigurePrimaryHttpMessageHandler(_ => _factory.Server.CreateHandler());
        //    serviceCollection
        //        .AddFruitigoClient();

        //    Client = serviceCollection.BuildServiceProvider().GetRequiredService<IFruitigoClient>();
        //}

        /// <summary>
        /// Run against real instance
        /// </summary>
        /// <param name="factory"></param>
        public FruitApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddFruitigoClient()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5095/graphql/"));

            Client = serviceCollection.BuildServiceProvider().GetRequiredService<IFruitigoClient>();
        }

        [Fact]
        public async Task Should_return_stock()
        {
            // Act
            var result = await Client.Stock.ExecuteAsync();

            // Assert
            Assert.NotNull(result.Data);
            Assert.InRange(result.Data.Stock.Count, 0, 10_000);
            Assert.All(result.Data.Stock, fruit =>
            {
                Assert.NotNull(fruit.Id.ToString());
                Assert.NotNull(fruit.OriginCountry);
            });
        }

        [Fact]
        public async Task Should_mutate_stock()
        {
            // Act
            var result = await Client.Modify.ExecuteAsync();

            // Assert
            Assert.NotNull(result.Data);
            Assert.InRange(result.Data.AddStock.FruitStock.QuantityAvailable, 0, 10_000);
            Assert.InRange(result.Data.RemoveStock.FruitStock.QuantityAvailable, 0, 10_000);
        }
    }
}