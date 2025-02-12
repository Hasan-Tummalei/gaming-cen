using GamingCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamingCenter.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the Membership entity for the database.
    /// </summary>
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        /// <summary>
        /// Configures the properties of the Membership entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("Membership");
            builder.HasKey(membership => membership.Id);

            builder.OwnsOne(membership => membership.MembershipName, membershipNameBuilder =>
            {
                membershipNameBuilder
                    .Property(membershipTitle => membershipTitle.Value)
                    .HasColumnName("MemberShipTitle")
                    .IsRequired();
            });

            builder.OwnsOne(membership => membership.DiscountPercentage, discountPercentageBuilder =>
            {
                discountPercentageBuilder
                    .Property(discount => discount.Value)
                    .HasColumnName("DiscountPercentage")
                    .IsRequired();
            });

            builder.OwnsOne(membership => membership.HoursThreshold, hoursThresholdBuilder =>
            {
                hoursThresholdBuilder
                    .Property(threshold => threshold.Value)
                    .HasColumnName("HoursThreshold")
                    .IsRequired();
            });
        }
    }
}
