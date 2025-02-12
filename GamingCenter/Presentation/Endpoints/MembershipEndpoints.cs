using GamingCenter.Application.DTOs;
using GamingCenter.Application.Interfaces;

namespace GamingCenter.Presentation.Endpoints
{
    /// <summary>
    /// Defines the membership-related API endpoints for the Gaming Center, requiring Admin authorization.
    /// </summary>
    public static class MembershipEndpoints
    {
        /// <summary>
        /// Maps the membership-related endpoints to the specified <see cref="WebApplication"/> instance.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to map the endpoints to.</param>
        public static void MapMembershipEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/memberships").WithTags("Memberships").RequireAuthorization("Admin");

            /// <summary>
            /// Gets the list of all memberships.
            /// </summary>
            /// <param name="membershipService">The <see cref="IMembershipService"/> to handle the membership logic.</param>
            /// <returns>An <see cref="IResult"/> containing the list of memberships.</returns>
            group.MapGet("/", async (IMembershipService membershipService) =>
            {
                var memberships = await membershipService.GetAllMembershipsAsync();
                return Results.Ok(memberships);
            });

            /// <summary>
            /// Gets a specific membership by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the membership to retrieve.</param>
            /// <param name="membershipService">The <see cref="IMembershipService"/> to handle the membership retrieval logic.</param>
            /// <returns>An <see cref="IResult"/> containing the membership details or an error result.</returns>
            group.MapGet("/{id}", async (Guid id, IMembershipService membershipService) =>
            {
                try
                {
                    var membership = await membershipService.GetMembershipByIdAsync(id);
                    return Results.Ok(membership);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });

            /// <summary>
            /// Creates a new membership.
            /// </summary>
            /// <param name="membershipDto">The <see cref="MembershipDto"/> containing membership details.</param>
            /// <param name="membershipService">The <see cref="IMembershipService"/> to handle the membership creation logic.</param>
            /// <returns>An <see cref="IResult"/> indicating the outcome of the membership creation attempt.</returns>
            group.MapPost("/", async (MembershipDto membershipDto, IMembershipService membershipService) =>
            {
                try
                {
                    await membershipService.AddMembershipAsync(membershipDto);
                    return Results.Ok("Membership added successfully.");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Updates an existing membership by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the membership to update.</param>
            /// <param name="membershipDto">The <see cref="MembershipDto"/> containing updated membership details.</param>
            /// <param name="membershipService">The <see cref="IMembershipService"/> to handle the membership update logic.</param>
            /// <returns>An <see cref="IResult"/> indicating the outcome of the membership update attempt.</returns>
            group.MapPut("/{id}", async (Guid id, MembershipDto membershipDto, IMembershipService membershipService) =>
            {
                try
                {
                    await membershipService.UpdateMembershipAsync(id, membershipDto);
                    return Results.Ok("Membership updated successfully.");
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Deletes a membership by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the membership to delete.</param>
            /// <param name="membershipService">The <see cref="IMembershipService"/> to handle the membership deletion logic.</param>
            /// <returns>An <see cref="IResult"/> indicating the outcome of the membership deletion attempt.</returns>
            group.MapDelete("/{id}", async (Guid id, IMembershipService membershipService) =>
            {
                try
                {
                    await membershipService.DeleteMembershipAsync(id);
                    return Results.Ok("Membership deleted successfully.");
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
        }
    }
}
