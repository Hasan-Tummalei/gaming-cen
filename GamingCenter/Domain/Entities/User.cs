using GamingCenter.Domain.Enums;
using GamingCenter.Domain.ValueObjects.User;

namespace GamingCenter.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Name Name { get; private set; }
        public DoB DoB { get; private set; }
        public ProfileImage? ProfileImage { get; private set; }
        public UserRole Role { get; private set; }
        public Guid? MembershipId { get; private set; }
        public Membership? Membership { get; private set; }
        public TotalPlayTime TotalPlaytimeHours { get; private set; } = new TotalPlayTime(0);

        public ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();

        private User() { }

        public User(Email email, Password password, Name name, DoB dob, ProfileImage? profileImage, UserRole role)
        {
            Email = email;
            Password = password;
            Name = name;
            DoB = dob;
            ProfileImage = profileImage;
            Role = role;
        }

        public void UpdateTotalPlayTime(TotalPlayTime totalPlayTime) {
            TotalPlaytimeHours = totalPlayTime;
        }
        public void UpdateProfile(Name name, Password password, ProfileImage? profileImage)
        {
            Name = name;
            Password = password;
            ProfileImage = profileImage;
        }
        public void UpdateMemberShip(Guid membershipId, Membership membership)
        {
            MembershipId = membershipId;
            Membership = membership;
        }
    }


}
