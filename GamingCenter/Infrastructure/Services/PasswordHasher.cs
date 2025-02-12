using BCrypt.Net;
using GamingCenter.Application.Interfaces;

namespace GamingCenter.Infrastructure.Services
{
    /// <summary>
    /// Service responsible for hashing and verifying passwords using BCrypt.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Hashes a plain-text password using BCrypt hashing algorithm.
        /// </summary>
        /// <param name="password">The plain-text password to be hashed.</param>
        /// <returns>A hashed password string.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the password hashing process.</exception>
        public string HashPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (SaltParseException e)
            {
                throw new Exception($"Error during hashing password: {e.Message}");
            }
        }

        /// <summary>
        /// Verifies a plain-text password against a hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare with.</param>
        /// <returns>True if the password matches the hashed password, otherwise false.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the password verification process.</exception>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (SaltParseException e)
            {
                throw new Exception($"Error during verifying password due to salt: {e.Message}");
            }
            catch (ArgumentException e)
            {
                throw new Exception("Error due to invalid argument during password verification.");
            }
        }
    }
}
