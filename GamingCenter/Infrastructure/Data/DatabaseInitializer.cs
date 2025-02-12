using GamingCenter.Application.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Enums;
using GamingCenter.Domain.ValueObjects.Membership;
using GamingCenter.Domain.ValueObjects.PC;
using GamingCenter.Domain.ValueObjects.User;
using GamingCenter.Infrastructure.Persistance;

namespace GamingCenter.Infrastructure.Data
{
    /// <summary>
    /// Static class responsible for initializing the database with sample data.
    /// It ensures that the required data (users, PCs, memberships) are present in the database if not already populated.
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initializes the database with users, PCs, and memberships if they are not already present.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/> to interact with the database.</param>
        /// <param name="passwordHasher">The <see cref="IPasswordHasher"/> to hash user passwords.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task InitializeAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            if (context.Users.Any())
                return;

            var adminUsers = new List<User>
            {
                new(Email.Create("hasan@admin.com"), Password.Create(passwordHasher.HashPassword("Admin123!@#")), Name.Create("Hasan Tummalei"), DoB.Create(new DateTime(1990, 1, 1)),null, UserRole.Admin),
                new(Email.Create("faris@admin.com"), Password.Create(passwordHasher.HashPassword("Admin123!@#")), Name.Create("Faris Kayyali"), DoB.Create(new DateTime(1990, 1, 2)),null, UserRole.Admin)


            };
            var normalUsers = new List<User>
            {
                new(Email.Create("hasan@user.com"), Password.Create(passwordHasher.HashPassword("User123!@#")), Name.Create("Hasan Tummalei"), DoB.Create(new DateTime(1990, 1, 1)),null, UserRole.User),
                new(Email.Create("faris@user.com"), Password.Create(passwordHasher.HashPassword("User123!@#")), Name.Create("Faris Kayyali"), DoB.Create(new DateTime(1990, 1, 2)),null, UserRole.User)


            };

            await context.Users.AddRangeAsync(adminUsers);
            await context.Users.AddRangeAsync(normalUsers);

            await context.SaveChangesAsync();

            if (context.PCs.Any())
                return;

            var initPcs = new List<PC>
            {
                new PC(new Cost(100, "USD"), new PerformanceRate(5)),
                new PC(new Cost(100, "USD"), new PerformanceRate(6)),
                new PC(new Cost(200, "USD"), new PerformanceRate(5)),
                new PC(new Cost(200, "USD"), new PerformanceRate(6)),
                new PC(new Cost(300, "USD"), new PerformanceRate(5)),
                new PC(new Cost(300, "USD"), new PerformanceRate(6)),
                new PC(new Cost(600, "USD"), new PerformanceRate(7)),
                new PC(new Cost(600, "USD"), new PerformanceRate(8)),
                new PC(new Cost(1200, "USD"), new PerformanceRate(9)),
                new PC(new Cost(1200, "USD"), new PerformanceRate(10)),



            };
            await context.PCs.AddRangeAsync(initPcs);
            await context.SaveChangesAsync();

            if (context.Memberships.Any())
                return;
            var initMemberships = new List<Membership>
            {
             new Membership(new MemberShipTitle("Gold Tier"), new DiscountPercentage(0.20m), new HoursThreshold(100m)),
             new Membership(new MemberShipTitle("Silver Tier"), new DiscountPercentage(0.10m), new HoursThreshold(50m)),
            };
            await context.Memberships.AddRangeAsync(initMemberships);
            await context.SaveChangesAsync();
        }
    }
}