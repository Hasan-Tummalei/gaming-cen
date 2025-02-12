namespace GamingCenter.Domain.ValueObjects.User
{
    public sealed record TotalPlayTime
    {
        public decimal Value { get; private init; } = 0;

        private TotalPlayTime() { }
        public TotalPlayTime(decimal value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException("Total play time hours must not be negative");
            Value = value;
        }
    }
}
