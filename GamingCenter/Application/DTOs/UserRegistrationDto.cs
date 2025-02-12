namespace GamingCenter.Application.DTOs
{
    public class UserRegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? ProfileImage { get; set; }   
    }

}
