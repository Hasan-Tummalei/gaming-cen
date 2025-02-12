using GamingCenter.Domain.Entities;

namespace GamingCenter.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
