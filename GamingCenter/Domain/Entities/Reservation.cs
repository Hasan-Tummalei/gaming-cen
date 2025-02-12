using GamingCenter.Domain.ValueObjects.Reservation;

namespace GamingCenter.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        //public ReservationDate ReservateionDate { get; private set; }
        public ReservationTimeFrame TimeFrame { get; private set; }
        public Duration Duration {  get; private set; }
        public Guid UserId { get; private set; }
        public Guid PCId { get; private set; }

        public double Price { get; private set; }

        public PC PC { get; private set; }
        public User User { get; private set; }

        private double  PricePerHour = 1;

        private Reservation() { }
        public Reservation (ReservationTimeFrame reservationDate, Duration duration, Guid usreId, Guid pcId, double totalPrice)
        {
            TimeFrame = reservationDate;
            Duration = duration;
            UserId = usreId;
            PCId = pcId;
            Price = totalPrice;
        }

        public void UpdateReservation(ReservationTimeFrame reservationDate ,Duration duration, double totalPrice)
        {
            TimeFrame = reservationDate;
            Duration = duration;
            Price = totalPrice;
        }
    }
}
