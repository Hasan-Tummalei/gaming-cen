using GamingCenter.Application.DTOs;
using GamingCenter.Application.Interfaces.GamingCenter.Application.Interfaces;

namespace GamingCenter.Presentation.Endpoints
{
    /// <summary>
    /// Defines the PC-related API endpoints.
    /// </summary>
    public static class PCEndpoints
    {
        /// <summary>
        /// Maps the PC-related endpoints to the provided WebApplication.
        /// </summary>
        /// <param name="app">The WebApplication instance to which the endpoints will be mapped.</param>
        public static void MapPCEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/pcs").WithTags("PCs").RequireAuthorization("Admin");

            /// <summary>
            /// Retrieves all PCs.
            /// </summary>
            /// <param name="pcService">The service used to retrieve all PCs.</param>
            /// <returns>A result containing the list of all PCs.</returns>
            group.MapGet("/", async (IPCService pcService) =>
            {
                var pcs = await pcService.GetAllPCsAsync();
                return Results.Ok(pcs);
            });

            /// <summary>
            /// Retrieves a specific PC by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the PC to retrieve.</param>
            /// <param name="pcService">The service used to retrieve the specific PC.</param>
            /// <returns>A result containing the details of the requested PC.</returns>
            group.MapGet("/{id}", async (string id, IPCService pcService) =>
            {
                try
                {
                    if (!Guid.TryParse(id, out var pcId))
                    {
                        return Results.BadRequest("Invalid PC ID format.");
                    }
                    var pc = await pcService.GetPCByIdAsync(pcId);
                    return Results.Ok(pc);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });

            /// <summary>
            /// Adds a new PC.
            /// </summary>
            /// <param name="pcDto">The data transfer object containing the details of the PC to add.</param>
            /// <param name="pcService">The service used to handle the PC addition logic.</param>
            /// <returns>A result indicating the success of the PC addition operation.</returns>
            group.MapPost("/", async (PCDto pcDto, IPCService pcService) =>
            {
                try
                {
                    await pcService.AddPCAsync(pcDto);
                    return Results.Ok("PC added successfully.");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Updates the details of an existing PC.
            /// </summary>
            /// <param name="id">The unique identifier of the PC to update.</param>
            /// <param name="pcDto">The data transfer object containing the updated PC details.</param>
            /// <param name="pcService">The service used to handle the PC update logic.</param>
            /// <returns>A result indicating the success or failure of the PC update operation.</returns>
            group.MapPut("/{id}", async (string id, PCDto pcDto, IPCService pcService) =>
            {
                try
                {
                    if (!Guid.TryParse(id, out var pcId))
                    {
                        return Results.BadRequest("Invalid PC ID format.");
                    }
                    await pcService.UpdatePCAsync(pcId, pcDto);
                    return Results.Ok("PC updated successfully.");
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
            /// Deletes a PC by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the PC to delete.</param>
            /// <param name="pcService">The service used to handle the PC deletion logic.</param>
            /// <returns>A result indicating the success or failure of the PC deletion operation.</returns>
            group.MapDelete("/{id}", async (string id, IPCService pcService) =>
            {
                try
                {
                    if (!Guid.TryParse(id, out var pcId))
                    {
                        return Results.BadRequest("Invalid PC ID format.");
                    }
                    await pcService.DeletePCAsync(pcId);
                    return Results.Ok("PC deleted successfully.");
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
        }
    }
}
