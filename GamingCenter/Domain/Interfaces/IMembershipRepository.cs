using GamingCenter.Domain.Entities;

namespace GamingCenter.Core.Interfaces
{
    public interface IMembershipRepository
    {
        Task<List<Membership>> GetAllMembershipsAsync();
        Task<Membership?> GetMembershipByIdAsync(Guid id);
        Task AddMembershipAsync(Membership membership);
        Task UpdateMembershipAsync(Membership membership);
        Task DeleteMembershipAsync(Guid id);
        //Task GetMembershipByIdAsync(Guid membershipId);
    }
}