using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Enums;
using GamingCenter.Domain.ValueObjects.User;

namespace GamingCenter.Application.DTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? ProfileImage { get; set; }
        public UserRole Role { get; set; }
        public TotalPlayTime TotalPlaytimeHours { get; set; }

        public UserResponseDTO(User user)
        {
            Id = user.Id;
            Email = user.Email.Value;
            Name = user.Name.Value;
            ProfileImage = user.ProfileImage?.Value;
            Role = user.Role;
            TotalPlaytimeHours = user.TotalPlaytimeHours;
        }
    }

}
