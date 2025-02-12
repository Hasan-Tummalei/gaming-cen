using GamingCenter.Application.DTOs;
using GamingCenter.Application.Interfaces;
using GamingCenter.Core.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Domain.ValueObjects.Membership;

namespace GamingCenter.Application.Services
{
    /// <summary>
    /// Provides services to manage memberships, including CRUD operations.
    /// </summary>
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipService"/> class.
        /// </summary>
        /// <param name="membershipRepository">Repository for membership data access.</param>
        /// <param name="loggerService">Service for logging operations.</param>
        public MembershipService(IMembershipRepository membershipRepository, ILoggerService loggerService)
        {
            _membershipRepository = membershipRepository;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Retrieves all memberships from the repository.
        /// </summary>
        /// <returns>A list of all memberships.</returns>
        public async Task<List<Membership>> GetAllMembershipsAsync()
        {
            _loggerService.LogInformation("Fetching all memberships.");
            return await _membershipRepository.GetAllMembershipsAsync();
        }

        /// <summary>
        /// Retrieves a membership by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the membership.</param>
        /// <returns>The membership associated with the specified ID.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the membership is not found.</exception>
        public async Task<Membership> GetMembershipByIdAsync(Guid id)
        {
            _loggerService.LogInformation($"Fetching membership with ID: {id}.");
            var membership = await _membershipRepository.GetMembershipByIdAsync(id);
            if (membership == null)
                throw new KeyNotFoundException("Membership not found.");
            return membership;
        }

        /// <summary>
        /// Adds a new membership to the repository.
        /// </summary>
        /// <param name="membershipDto">Data transfer object containing membership details.</param>
        public async Task AddMembershipAsync(MembershipDto membershipDto)
        {
            var membership = new Membership(
                new MemberShipTitle(membershipDto.Name),
                new DiscountPercentage(membershipDto.DiscountPercentage),
                new HoursThreshold(membershipDto.HoursThreshold));

            await _membershipRepository.AddMembershipAsync(membership);
            _loggerService.LogInformation($"Added new membership with ID: {membership.Id}.");
        }

        /// <summary>
        /// Updates an existing membership.
        /// </summary>
        /// <param name="id">The unique identifier of the membership to update.</param>
        /// <param name="membershipDto">Data transfer object containing updated membership details.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the membership is not found.</exception>
        public async Task UpdateMembershipAsync(Guid id, MembershipDto membershipDto)
        {
            var membership = await _membershipRepository.GetMembershipByIdAsync(id);
            if (membership == null)
                throw new KeyNotFoundException("Membership not found.");

            membership.UpdateMembership(
                new MemberShipTitle(membershipDto.Name),
                new DiscountPercentage(membershipDto.DiscountPercentage),
                new HoursThreshold(membershipDto.HoursThreshold));

            await _membershipRepository.UpdateMembershipAsync(membership);
            _loggerService.LogInformation($"Updated membership with ID: {membership.Id}.");
        }

        /// <summary>
        /// Deletes a membership by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the membership to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the membership is not found.</exception>
        public async Task DeleteMembershipAsync(Guid id)
        {
            var membership = await _membershipRepository.GetMembershipByIdAsync(id);
            if (membership == null)
                throw new KeyNotFoundException("Membership not found.");

            await _membershipRepository.DeleteMembershipAsync(id);
            _loggerService.LogInformation($"Deleted membership with ID: {id}.");
        }
    }
}
