using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GamingCenter.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the User entity for the database.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the properties of the User entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).ValueGeneratedNever();

            builder.OwnsOne(user => user.Name, nameBuilder =>
            {
                nameBuilder
                    .Property(name => name.Value)
                    .HasColumnName("Name")
                    .IsRequired();
            });

            builder.OwnsOne(user => user.DoB, dobBuilder =>
            {
                dobBuilder
                    .Property(dob => dob.Value)
                    .HasColumnName("DateOfbirth")
                    .IsRequired();
            });

            builder.OwnsOne(user => user.Email, emailBuilder =>
            {
                emailBuilder
                    .Property(email => email.Value)
                    .HasColumnName("Email")
                    .IsRequired();

                emailBuilder
                    .HasIndex(email => email.Value)
                    .IsUnique();
            });

            builder.OwnsOne(user => user.Password, passwordBuilder =>
            {
                passwordBuilder
                    .Property(password => password.Value)
                    .HasColumnName("Password")
                    .IsRequired();
            });

            builder.OwnsOne(user => user.ProfileImage, profileImageBuilder =>
            {
                profileImageBuilder
                    .Property(profileImage => profileImage.Value)
                    .HasColumnName("ProfileImage")
                    .IsRequired(false);
            });

            builder.OwnsOne(user => user.TotalPlaytimeHours, totalTimePlayedBuilder =>
            {
                totalTimePlayedBuilder
                    .Property(totalTimePlayedBuilder => totalTimePlayedBuilder.Value)
                    .HasColumnName("TotalTimePlayed")
                    .IsRequired();
            });

            // Relationship with Membership
            builder.HasOne(user => user.Membership)
                .WithMany(membership => membership.Users)
                .HasForeignKey(user => user.MembershipId);
        }
    }
}
