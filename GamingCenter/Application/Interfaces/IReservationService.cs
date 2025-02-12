using GamingCenter.Application.DTOs;
using GamingCenter.Domain.Entities;
using GamingCenter.Domain.ValueObjects.Reservation;

public interface IReservationService
{
    Task<List<PC>> GetAvailablePCsAsync(DateTime startTime, DateTime endTime);

    Task<ReservationResponse> ReservePCAsync(ReservationRequestDto request);

    Task<List<Reservation>> GetReservationsByUserAsync(Guid userId);

    Task UpdateReservationAsync(Guid id, ReservationDto reservationDto);

    Task DeleteReservationAsync(Guid id);
}