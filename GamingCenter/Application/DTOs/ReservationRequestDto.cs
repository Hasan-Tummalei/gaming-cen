namespace GamingCenter.Application.DTOs
{
    public class ReservationRequestDto
    {
        public Guid UserId { get; set; }
        public Guid PCId { get; set; }
        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }
        //public int DurationHours { get; set; }
    }
}
