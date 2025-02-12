namespace GamingCenter.Domain.ValueObjects.PC
{
    public sealed record PerformanceRate
    {
        public int Value { get; private init; }
        public readonly int MaxRate = 10;
        public readonly int MinRate = 4;

        private PerformanceRate() { }
        public PerformanceRate(int rate)
        {
            if (rate < MinRate || rate > MaxRate) throw new ArgumentException($"Rate must be between {MinRate} and {MaxRate}");
            Value = rate;
        }
    }
}
