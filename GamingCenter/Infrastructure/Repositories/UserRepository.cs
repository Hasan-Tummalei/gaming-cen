using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Interfaces;
using GamingCenter.Domain.ValueObjects.User;
using GamingCenter.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GamingCenter.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for handling CRUD operations on User entities.
    /// </summary>
    public class UserRepository : IUserRespository
    {
        private readonly ApplicationDbContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/> instance for accessing the database.</param>
        public UserRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The <see cref="User"/> entity if found, otherwise <c>null</c>.</returns>
        public async Task<User?> GetUserByEmailAsync(Email email)
        {
            return await Context.Users.FirstOrDefaultAsync(user => user.Email.Value == email.Value);
        }

        /// <summary>
        /// Adds a new user to the database asynchronously.
        /// </summary>
        /// <param name="user">The <see cref="User"/> entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddUserAsync(User user)
        {
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing user in the database asynchronously.
        /// </summary>
        /// <param name="user">The <see cref="User"/> entity with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateUserAsync(User user)
        {
            Context.Update(user);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The <see cref="User"/> entity if found, otherwise <c>null</c>.</returns>
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await Context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
