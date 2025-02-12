using GamingCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamingCenter.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the PC entity for the database.
    /// </summary>
    public class PCConfiguration : IEntityTypeConfiguration<PC>
    {
        /// <summary>
        /// Configures the properties of the PC entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<PC> builder)
        {
            builder.ToTable("PC");
            builder.HasKey(pc => pc.Id);

            builder.OwnsOne(pc => pc.Performance, performanceBuilder =>
            {
                performanceBuilder
                    .Property(performance => performance.Value)
                    .HasColumnName("Performance")
                    .IsRequired();
            });

            builder.OwnsOne(pc => pc.Cost, costBuilder =>
            {
                costBuilder
                    .Property(cost => cost.Value)
                    .HasColumnName("Cost")
                    .IsRequired();

                costBuilder
                    .Property(cost => cost.currency)
                    .HasColumnName("Currency")
                    .IsRequired();
            });

            // Optional relationship configuration for reservations (currently commented out).
            // builder.HasMany(pc => pc.Reservations)
            //        .WithOne(reservation => reservation.PC)
            //        .HasForeignKey(reservation => reservation.PCId)
            //        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
