namespace Fruitigo.Domain
{
    public record StockMutation
    {
        public Guid FruitStockId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int Quantity { get; private set; }

        public StockMutation(int quantity)
        {
            Timestamp = DateTimeOffset.UtcNow;
            Quantity = quantity;
        }
    }
}