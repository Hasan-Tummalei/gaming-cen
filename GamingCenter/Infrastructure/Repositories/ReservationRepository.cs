using GamingCenter.Core.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GamingCenter.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for handling CRUD operations on Reservation entities.
    /// </summary>
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/> instance for accessing the database.</param>
        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of reservations that overlap with the specified time frame.
        /// </summary>
        /// <param name="startTime">The start time of the desired time frame.</param>
        /// <param name="endTime">The end time of the desired time frame.</param>
        /// <returns>A list of <see cref="Reservation"/> entities that overlap with the specified time frame.</returns>
        public async Task<List<Reservation>> GetReservationsInTimeFrameAsync(DateTime startTime, DateTime endTime)
        {
            return await _context.Reservations
                .Where(r => r.TimeFrame.StartTime < endTime && r.TimeFrame.EndTime > startTime)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all reservations made by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose reservations are to be retrieved.</param>
        /// <returns>A list of <see cref="Reservation"/> entities made by the specified user.</returns>
        public async Task<List<Reservation>> GetReservationsByUserAsync(Guid userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>The <see cref="Reservation"/> entity if found, otherwise <c>null</c>.</returns>
        public async Task<Reservation?> GetReservationByIdAsync(Guid id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        /// <summary>
        /// Adds a new reservation to the database asynchronously.
        /// </summary>
        /// <param name="reservation">The <see cref="Reservation"/> entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing reservation in the database asynchronously.
        /// </summary>
        /// <param name="reservation">The <see cref="Reservation"/> entity with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a reservation by its ID from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteReservationAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
