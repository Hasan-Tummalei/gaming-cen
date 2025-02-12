namespace GamingCenter.Domain.ValueObjects.Reservation
{
    public sealed record ReservationTimeFrame
    {
        public DateTime StartTime { get; private init; }
        public DateTime EndTime { get; private init; }
    



    private ReservationTimeFrame() { }
    private ReservationTimeFrame(DateTime start, DateTime end)
        {
            StartTime = start;
            EndTime = end;
        }

    public static ReservationTimeFrame Create(DateTime startDate, DateTime endDate)
        {
            if (startDate < DateTime.Now || endDate < DateTime.Now || endDate < startDate) throw new ArgumentException("Please select a valid start and end date for reservation.");

            return new ReservationTimeFrame(startDate, endDate);
        }
    }
}
