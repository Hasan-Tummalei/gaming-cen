using GamingCenter.Domain.Entities;
using GamingCenter.Domain.ValueObjects.User;

namespace GamingCenter.Domain.Interfaces
{
    public interface IUserRespository
    {

        Task<User?> GetUserByEmailAsync(Email email);
        Task<User?> GetUserByIdAsync(Guid id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);

    }
}
