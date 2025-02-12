using GamingCenter.Application.Interfaces;

namespace GamingCenter.Infrastructure.Services
{
    /// <summary>
    /// Service responsible for logging information and error messages.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        /// <summary>
        /// Logs an informational message to the console with a timestamp.
        /// </summary>
        /// <param name="message">The message to be logged as informational.</param>
        public void LogInformation(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now}: {message}");
        }

        /// <summary>
        /// Logs an error message to the console with a timestamp.
        /// </summary>
        /// <param name="message">The message to be logged as an error.</param>
        public void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now}: {message}");
        }
    }
}
