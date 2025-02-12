namespace GamingCenter.Domain.ValueObjects.Reservation
{
    public sealed record Duration
    {
        public int Value { get; private init; }


        private Duration() { }

        public Duration (int value)
        {
            if (value < 0 || value > 12) throw new ArgumentException("Duration can't be negative or more than 12 hours");
            Value = value;
        }
    }
}
