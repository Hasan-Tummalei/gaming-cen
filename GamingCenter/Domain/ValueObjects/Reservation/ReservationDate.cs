using GamingCenter.Domain.ValueObjects.User;

namespace GamingCenter.Domain.ValueObjects.Reservation
{
    public sealed record ReservationDate
    {
        public DateTime Value { get; private init; }


        private ReservationDate() { }
        private ReservationDate(DateTime reservationDate)
        {
            Value = reservationDate;
        }


        public static ReservationDate Create(DateTime reservationDate)
        {
            if (reservationDate < DateTime.Now) throw new ArgumentException("Reservation date must not be in the past");
            return new ReservationDate(reservationDate);
        }
    }
}
