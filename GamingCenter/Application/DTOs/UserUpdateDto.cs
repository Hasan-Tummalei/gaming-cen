namespace GamingCenter.Application.DTOs
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? ProfileImage { get; set; }
        public Guid? MembershipId { get ; set; }
    }
}
