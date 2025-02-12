namespace GamingCenter.Domain.ValueObjects.PC
{
    public sealed record Cost
    {
        public double Value { get; init; }
        public string currency { get; init; }

        private Cost() { }
        public Cost(double price, string currency)
        {
            if (price < 0 || price > 10000) throw new ArgumentException("PC price must be between 0 and 10000");
            if (string.IsNullOrEmpty(currency) || currency.Length > 3) throw new ArgumentNullException("Invlid Currncy was entered");
            this.Value = price;
            this.currency = currency;

        }
    }
}
