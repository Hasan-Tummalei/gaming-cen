using GamingCenter.Application.DTOs;
using GamingCenter.Domain.Entities;

namespace GamingCenter.Application.Interfaces
{
    public interface IMembershipService
    {
        Task<List<Membership>> GetAllMembershipsAsync();
        Task<Membership> GetMembershipByIdAsync(Guid id);
        Task AddMembershipAsync(MembershipDto membershipDto);
        Task UpdateMembershipAsync(Guid id, MembershipDto membershipDto);
        Task DeleteMembershipAsync(Guid id);
    }
}