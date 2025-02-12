using GamingCenter.Application.Interfaces;

namespace GamingCenter.Presentation.Middleware
{
    /// <summary>
    /// Middleware to handle errors globally in the application.
    /// Catches exceptions during the request processing and logs them using <see cref="ILoggerService"/>.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to resolve services from the DI container.</param>
        public ErrorHandlingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Invokes the middleware to handle errors during request processing.
        /// Logs any exceptions and sends an internal server error response.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService>();

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    loggerService.LogError($"An error occurred: {ex.Message}");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An unexpected error occurred.");
                }
            }
        }
    }
}
