using GamingCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GamingCenter.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the Reservation entity for the database.
    /// </summary>
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        /// <summary>
        /// Configures the properties of the Reservation entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            // Value converter to store DateTime in UTC format
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            builder.ToTable("Reservation");
            builder.HasKey(reservation => reservation.Id);
            builder.Property(reservation => reservation.Id).ValueGeneratedNever();

            builder.OwnsOne(reservation => reservation.TimeFrame, timeFrameBuilder =>
            {
                timeFrameBuilder
                    .Property(reservationDate => reservationDate.StartTime)
                    .HasColumnName("StartTime")
                    .IsRequired()
                    .HasConversion(dateTimeConverter);

                timeFrameBuilder
                    .Property(reservationDate => reservationDate.EndTime)
                    .HasColumnName("EndTime")
                    .IsRequired()
                    .HasConversion(dateTimeConverter);
            });

            builder.OwnsOne(reservation => reservation.Duration, duraitonBuilder =>
            {
                duraitonBuilder
                    .Property(duration => duration.Value)
                    .HasColumnName("Duration")
                    .IsRequired();
            });

            builder.Property(reservation => reservation.Price)
                .HasColumnName("Price")
                .IsRequired();

            builder.HasOne(reservation => reservation.User)
                .WithMany(user => user.Reservations)
                .HasForeignKey(reservation => reservation.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(reservation => reservation.PC)
                .WithMany(pc => pc.Reservations)
                .HasForeignKey(reservation => reservation.PCId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
