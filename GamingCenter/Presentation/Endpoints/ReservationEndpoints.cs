using System.Net.Http;
using System.Security.Claims;
using GamingCenter.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GamingCenter.Presentation.Endpoints
{
    /// <summary>
    /// Defines the reservation-related API endpoints for the Gaming Center.
    /// </summary>
    public static class ReservationEndpoints
    {
        /// <summary>
        /// Maps the reservation-related endpoints to the specified <see cref="WebApplication"/> instance.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to map the endpoints to.</param>
        public static void MapReservationEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/reservations").WithTags("Reservations");

            /// <summary>
            /// Gets the list of reservations for a specific user.
            /// </summary>
            /// <param name="userId">The unique identifier of the user.</param>
            /// <param name="reservationService">The <see cref="IReservationService"/> to handle the reservation logic.</param>
            /// <param name="httpContext">The current HTTP context.</param>
            /// <returns>An <see cref="IResult"/> containing the user's reservations or an error result.</returns>
            group.MapGet("/user/{userId}", async (Guid userId, IReservationService reservationService, HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userIdToken))
                {
                    return Results.Unauthorized();
                }
                if (userIdToken != userId)
                {
                    return Results.Forbid();
                }
                var reservations = await reservationService.GetReservationsByUserAsync(userId);
                return Results.Ok(reservations);
            }).RequireAuthorization("User");

            /// <summary>
            /// Creates a new reservation for a user.
            /// </summary>
            /// <param name="request">The <see cref="ReservationRequestDto"/> containing reservation details.</param>
            /// <param name="reservationService">The <see cref="IReservationService"/> to handle the reservation logic.</param>
            /// <param name="httpContext">The current HTTP context.</param>
            /// <returns>An <see cref="IResult"/> indicating the outcome of the reservation attempt.</returns>
            group.MapPost("/reserve", async ([FromBody] ReservationRequestDto request, IReservationService reservationService, HttpContext httpContext) =>
            {
                try
                {
                    var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                    {
                        return Results.Unauthorized();
                    }
                    if (userId != request.UserId)
                    {
                        return Results.Forbid();
                    }

                    var reservation = await reservationService.ReservePCAsync(request);
                    return Results.Ok($"Reservation confirmed. Total price: ${reservation.TotalPrice}");
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            }).RequireAuthorization("User");

            /// <summary>
            /// Updates an existing reservation.
            /// </summary>
            /// <param name="id">The unique identifier of the reservation to update.</param>
            /// <param name="reservationDto">The <see cref="ReservationDto"/> containing updated reservation details.</param>
            /// <param name="reservationService">The <see cref="IReservationService"/> to handle the reservation update logic.</param>
            /// <returns>An <see cref="IResult"/> indicating the outcome of the update attempt.</returns>
            group.MapPut("/{id}", async (Guid id, ReservationDto reservationDto, IReservationService reservationService) =>
            {
                try
                {
                    await reservationService.UpdateReservationAsync(id, reservationDto);
                    return Results.Ok("Reservation updated successfully.");
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            }).RequireAuthorization("User");

            /// <summary>
            /// Deletes a reservation by its identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the reservation to delete.</param>
            /// <param name="reservationService">The <see cref="IReservationService"/> to handle the reservation deletion logic.</param>
            /// <returns>An <see cref="IResult"/> indicating the outcome of the deletion attempt.</returns>
            group.MapDelete("/{id}", async (Guid id, IReservationService reservationService) =>
            {
                try
                {
                    await reservationService.DeleteReservationAsync(id);
                    return Results.Ok("Reservation deleted successfully.");
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            }).RequireAuthorization("User");
        }
    }
}
