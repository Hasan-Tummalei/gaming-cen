namespace GamingCenter.Domain.ValueObjects.Membership
{
    public sealed record DiscountPercentage
    {
        public decimal Value { get; private init; }


        private DiscountPercentage() { }
        public DiscountPercentage(decimal value)
        {
            if (value < 0 || value > 1) throw new ArgumentException("discount percentage must be between 0.0 and 1");
            Value = value;
        }
    }
}
