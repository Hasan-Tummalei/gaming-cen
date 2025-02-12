namespace GamingCenter.Application.Interfaces
{
    public interface IPasswordHasher
    {

        string HashPassword(string toBeHasedPassword);

        bool VerifyPassword(string password, string hashedPassowrd);
    }
}
