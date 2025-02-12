using System.Security.Claims;
using GamingCenter.Application.DTOs;
using GamingCenter.Application.Services;

namespace GamingCenter.Presentation.Endpoints
{
    /// <summary>
    /// Defines the User-related API endpoints.
    /// </summary>
    public static class UserEndpoints
    {
        /// <summary>
        /// Maps the user-related endpoints to the provided WebApplication.
        /// </summary>
        /// <param name="app">The WebApplication instance to which the endpoints will be mapped.</param>
        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/users").WithTags("Users");

            /// <summary>
            /// Registers a new user.
            /// </summary>
            /// <param name="userDto">The data transfer object containing user registration details.</param>
            /// <param name="userService">The user service used to handle user registration logic.</param>
            /// <returns>A result containing the response from the registration process.</returns>
            group.MapPost("/register", async (UserRegistrationDTO userDto, UserService userService) =>
            {
                try
                {
                    var response = await userService.RegisterUser(userDto);
                    return Results.Ok(response);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            /// <summary>
            /// Logs in an existing user.
            /// </summary>
            /// <param name="loginDto">The data transfer object containing user login details.</param>
            /// <param name="userService">The user service used to handle user login logic.</param>
            /// <returns>A result containing a JWT token upon successful login.</returns>
            group.MapPost("/login", async (UserLoginDto loginDto, UserService userService) =>
            {
                try
                {
                    var token = await userService.LoginUser(loginDto);
                    return Results.Ok(new { Token = token });
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.Unauthorized();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });

            /// <summary>
            /// Updates the user information.
            /// </summary>
            /// <param name="id">The unique identifier of the user to update.</param>
            /// <param name="updateDto">The data transfer object containing the updated user information.</param>
            /// <param name="userService">The user service used to handle the update logic.</param>
            /// <param name="httpContext">The HTTP context used to extract user claims for authorization checks.</param>
            /// <returns>A result indicating the success or failure of the update operation.</returns>
            group.MapPut("/{id}", async (Guid id, UserUpdateDto updateDto, UserService userService, HttpContext httpContext) =>
            {
                try
                {
                    var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                    {
                        return Results.Unauthorized();
                    }

                    if (userId != id)
                    {
                        return Results.Forbid();
                    }

                    var updatedUser = await userService.UpdateUser(id, updateDto);
                    return Results.Ok(updatedUser);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).RequireAuthorization();

            /// <summary>
            /// Updates a user's membership information (admin-only).
            /// </summary>
            /// <param name="id">The unique identifier of the user whose membership will be updated.</param>
            /// <param name="membershipUpdateDto">The data transfer object containing the updated membership information.</param>
            /// <param name="userService">The user service used to handle the membership update logic.</param>
            /// <returns>A result indicating the success or failure of the membership update operation.</returns>
            group.MapPut("/admin/{id}", async (Guid id, UserUpdateMembershipDto membershipUpdateDto, UserService userService) =>
            {
                try
                {
                    var updatedMembership = await userService.UpdateUserMemberShipAsync(id, membershipUpdateDto);
                    return Results.Ok(updatedMembership);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).RequireAuthorization("Admin");
        }
    }
}
