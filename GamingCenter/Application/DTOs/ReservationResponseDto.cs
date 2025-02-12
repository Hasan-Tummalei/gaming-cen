namespace GamingCenter.Application.DTOs
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public Guid PCId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalPrice { get; set; }
    }
}
