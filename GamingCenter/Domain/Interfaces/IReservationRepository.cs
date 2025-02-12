using GamingCenter.Domain.Entities;

namespace GamingCenter.Core.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetReservationsInTimeFrameAsync(DateTime startTime, DateTime endTime);
        Task<List<Reservation>> GetReservationsByUserAsync(Guid userId);
        Task<Reservation?> GetReservationByIdAsync(Guid id);
        Task AddReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(Guid id);
    }
}