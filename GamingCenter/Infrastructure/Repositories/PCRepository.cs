using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Interfaces;
using GamingCenter.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GamingCenter.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for handling CRUD operations on PC entities.
    /// </summary>
    public class PCRepository : IPCRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PCRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/> instance for accessing the database.</param>
        public PCRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all PCs from the database asynchronously.
        /// </summary>
        /// <returns>A list of <see cref="PC"/> entities.</returns>
        public async Task<List<PC>> GetAllPCsAsync()
        {
            return await _context.PCs.ToListAsync();
        }

        /// <summary>
        /// Retrieves a PC by its ID from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the PC.</param>
        /// <returns>A <see cref="PC"/> entity, or <c>null</c> if not found.</returns>
        public async Task<PC?> GetPCByIdAsync(Guid id)
        {
            return await _context.PCs.FindAsync(id);
        }

        /// <summary>
        /// Adds a new PC to the database asynchronously.
        /// </summary>
        /// <param name="pc">The <see cref="PC"/> entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddPCAsync(PC pc)
        {
            await _context.PCs.AddAsync(pc);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing PC in the database asynchronously.
        /// </summary>
        /// <param name="pc">The <see cref="PC"/> entity with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdatePCAsync(PC pc)
        {
            _context.PCs.Update(pc);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a PC by its ID from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the PC to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeletePCAsync(Guid id)
        {
            var pc = await _context.PCs.FindAsync(id);
            if (pc != null)
            {
                _context.PCs.Remove(pc);
                await _context.SaveChangesAsync();
            }
        }
    }
}
