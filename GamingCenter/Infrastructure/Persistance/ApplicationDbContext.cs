using GamingCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamingCenter.Infrastructure.Persistance
{
    /// <summary>
    /// The database context class for interacting with the database.
    /// It manages the `User`, `PC`, `Reservation`, and `Membership` entities.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the `Users` table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the `PCs` table in the database.
        /// </summary>
        public DbSet<PC> PCs { get; set; }

        /// <summary>
        /// Gets or sets the `Reservations` table in the database.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Gets or sets the `Memberships` table in the database.
        /// </summary>
        public DbSet<Membership> Memberships { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the context.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the model and applies configurations from the assembly.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
