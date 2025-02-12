using GamingCenter.Application.DTOs;
using GamingCenter.Application.Interfaces;
using GamingCenter.Core.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Interfaces;
using GamingCenter.Domain.ValueObjects.Reservation;
using GamingCenter.Domain.ValueObjects.User;

namespace GamingCenter.Application.Services
{
    /// <summary>
    /// Provides services related to PC reservations.
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPCRepository _pcRepository;
        private readonly IUserRespository _userRepository;
        private readonly ILoggerService _loggerService;

        public ReservationService(
            IReservationRepository reservationRepository,
            IPCRepository pcRepository,
            IUserRespository userRepository,
            ILoggerService loggerService)
        {
            _reservationRepository = reservationRepository;
            _pcRepository = pcRepository;
            _userRepository = userRepository;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Retrieves a list of available PCs within the specified time frame.
        /// </summary>
        /// <param name="startTime">The start time of the desired reservation period.</param>
        /// <param name="endTime">The end time of the desired reservation period.</param>
        /// <returns>A list of available PCs.</returns>
        public async Task<List<PC>> GetAvailablePCsAsync(DateTime startTime, DateTime endTime)
        {
            var allPCs = await _pcRepository.GetAllPCsAsync();

            var overlappingReservations = await _reservationRepository.GetReservationsInTimeFrameAsync(startTime, endTime);
            var availablePCs = allPCs
                .Where(pc => !overlappingReservations.Any(r => r.PCId == pc.Id))
                .ToList();

            return availablePCs;
        }

        /// <summary>
        /// Reserves a PC based on the provided reservation request.
        /// </summary>
        /// <param name="request">The reservation request details.</param>
        /// <returns>A response containing reservation details.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the specified PC or user is not found.</exception>
        /// <exception cref="ArgumentException">Thrown when the PC is not available at the selected time.</exception>
        public async Task<ReservationResponse> ReservePCAsync(ReservationRequestDto request)
        {
            var pc = await _pcRepository.GetPCByIdAsync(request.PCId);
            if (pc == null)
                throw new KeyNotFoundException("PC not found.");

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var isAvailable = await IsPCAvailableAsync(request.PCId, request.ReservationStartDate, request.ReservationEndDate);
            if (!isAvailable)
                throw new ArgumentException("PC is not available at the selected time.");

            double totalPrice = CalculateTotalPrice(request.ReservationStartDate, request.ReservationEndDate, user.Membership);

            var reservation = new Reservation(
                ReservationTimeFrame.Create(request.ReservationStartDate, request.ReservationEndDate),
                new Duration((int)(request.ReservationEndDate - request.ReservationStartDate).TotalHours),
                request.UserId, request.PCId, totalPrice);

            await _reservationRepository.AddReservationAsync(reservation);
            user.UpdateTotalPlayTime(new TotalPlayTime( user.TotalPlaytimeHours.Value + (int)(request.ReservationEndDate - request.ReservationStartDate).TotalHours));
            await _userRepository.UpdateUserAsync(user);

            _loggerService.LogInformation($"Reservation created with ID: {reservation.Id}.");

            return new ReservationResponse
            {
                Id = reservation.Id,
                PCId = reservation.PCId,
                StartTime = reservation.TimeFrame.StartTime,
                EndTime = reservation.TimeFrame.EndTime,
                TotalPrice = reservation.Price
            };
        }

        /// <summary>
        /// Retrieves reservations associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of reservations for the specified user.</returns>
        public async Task<List<Reservation>> GetReservationsByUserAsync(Guid userId)
        {
            return await _reservationRepository.GetReservationsByUserAsync(userId);
        }

        /// <summary>
        /// Updates an existing reservation with new details.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservationDto">The new reservation details.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the reservation or user is not found.</exception>
        /// <exception cref="ArgumentException">Thrown when the PC is not available at the selected time.</exception>
        public async Task UpdateReservationAsync(Guid id, ReservationDto reservationDto)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
                throw new KeyNotFoundException("Reservation not found.");

            var isAvailable = await IsPCAvailableAsync(reservation.PCId, reservationDto.ReservationStartDate, reservationDto.ReservationEndDate);
            if (!isAvailable)
                throw new ArgumentException("PC is not available at the selected time.");

            var user = await _userRepository.GetUserByIdAsync(reservation.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            double totalNewPrice = CalculateTotalPrice(reservationDto.ReservationStartDate, reservationDto.ReservationEndDate, user.Membership);

            reservation.UpdateReservation(
                ReservationTimeFrame.Create(reservationDto.ReservationStartDate, reservationDto.ReservationEndDate),
                new Duration((int)(reservationDto.ReservationEndDate - reservationDto.ReservationStartDate).TotalHours),
                totalNewPrice);

            await _reservationRepository.UpdateReservationAsync(reservation);
        }

        /// <summary>
        /// Deletes an existing reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the reservation is not found.</exception>
        public async Task DeleteReservationAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
                throw new KeyNotFoundException("Reservation not found.");

            await _reservationRepository.DeleteReservationAsync(id);
        }

        /// <summary>
        /// Checks if a PC is available within the specified time frame.
        /// </summary>
        /// <param name="pcId">The ID of the PC.</param>
        /// <param name="startTime">The start time of the reservation period.</param>
        /// <param name="endTime">The end time of the reservation period.</param>
        /// <returns><c>true</c> if the PC is available; otherwise, <c>false</c>.</returns>
        private async Task<bool> IsPCAvailableAsync(Guid pcId, DateTime startTime, DateTime endTime)
        {
            var overlappingReservations = await _reservationRepository.GetReservationsInTimeFrameAsync(startTime, endTime);
            return !overlappingReservations.Any(r => r.PCId == pcId);
        }

        /// <summary>
        /// Calculates the total price for a reservation based on duration and membership discount.
        /// </summary>
        /// <param name="startTime">The start time of the reservation.</param>
        /// <param name="endTime">The end time of the reservation.</param>
        /// <param name="membership">The user's membership details for discount calculation.</param>
        /// <returns>The total price of the reservation.</returns>
        private double CalculateTotalPrice(DateTime startTime, DateTime endTime, Membership? membership)
        {
            var durationHours = (int)(endTime - startTime).TotalHours;
            double totalPrice = durationHours * 1;

            if (membership != null)
            {
                totalPrice -= totalPrice * (double)(membership.DiscountPercentage.Value / 100);
            }

            return totalPrice;
        }
    }
}
