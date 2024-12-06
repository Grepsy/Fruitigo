namespace Fruitigo.Domain
{
    public class FruitStock
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public FruitType Type { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public string OriginCountry { get; set; }
        public decimal OrderPrice { get; set; }
        public decimal QuotePrice { get; set; }

        public decimal MarginPrice => QuotePrice - OrderPrice;
        public int QuantityAvailable => _stockMutations.Sum(x => x.Quantity);

        public IEnumerable<StockMutation> StockMutations => _stockMutations;
        private List<StockMutation> _stockMutations = [];

        // Used for seeding with Bogus (should be a better way, but POC)
        public FruitStock() { }

        public FruitStock(FruitType type, DateOnly expiryDate, string originCountry, decimal orderPrice, decimal quotePrice)
        {
            Type = type;
            ExpiryDate = expiryDate;
            OriginCountry = originCountry;
            OrderPrice = orderPrice;
            QuotePrice = quotePrice;
        }

        public void AddStock(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            _stockMutations.Add(new StockMutation(quantity));
        }

        public void RemoveStock(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            if (QuantityAvailable - quantity < 0)
            {
                throw new ArgumentException("Operation would reduce stock below zero.");
            }

            _stockMutations.Add(new StockMutation(-quantity));
        }
    }
}