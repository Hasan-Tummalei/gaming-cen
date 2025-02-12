namespace GamingCenter.Domain.ValueObjects.Membership
{
    public sealed record HoursThreshold
    {

        public decimal Value { get; private init; }
        public decimal MinValue = 0;
        public decimal MaxValue = 2000;

        private HoursThreshold() { }

        public HoursThreshold(decimal value)
        {
            if(value < MinValue || value > MaxValue) throw new ArgumentOutOfRangeException($"Hours threshold must be between {MinValue} and {MaxValue}");
            Value = value;
        }
    }
}
