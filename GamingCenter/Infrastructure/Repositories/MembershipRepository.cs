using GamingCenter.Core.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GamingCenter.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for handling CRUD operations on Membership entities.
    /// </summary>
    public class MembershipRepository : IMembershipRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/> instance for accessing the database.</param>
        public MembershipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all memberships from the database asynchronously.
        /// </summary>
        /// <returns>A list of <see cref="Membership"/> entities.</returns>
        public async Task<List<Membership>> GetAllMembershipsAsync()
        {
            return await _context.Memberships.ToListAsync();
        }

        /// <summary>
        /// Retrieves a membership by its ID from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the membership.</param>
        /// <returns>A <see cref="Membership"/> entity, or <c>null</c> if not found.</returns>
        public async Task<Membership?> GetMembershipByIdAsync(Guid id)
        {
            return await _context.Memberships.FindAsync(id);
        }

        /// <summary>
        /// Adds a new membership to the database asynchronously.
        /// </summary>
        /// <param name="membership">The <see cref="Membership"/> entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddMembershipAsync(Membership membership)
        {
            await _context.Memberships.AddAsync(membership);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing membership in the database asynchronously.
        /// </summary>
        /// <param name="membership">The <see cref="Membership"/> entity with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateMembershipAsync(Membership membership)
        {
            _context.Memberships.Update(membership);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a membership by its ID from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the membership to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteMembershipAsync(Guid id)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
                await _context.SaveChangesAsync();
            }
        }
    }
}
