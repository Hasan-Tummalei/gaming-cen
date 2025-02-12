using GamingCenter.Application.DTOs;
using GamingCenter.Application.Interfaces;
using GamingCenter.Application.Interfaces.GamingCenter.Application.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Interfaces;
using GamingCenter.Domain.ValueObjects.PC;

namespace GamingCenter.Application.Services
{
    /// <summary>
    /// Provides services to manage PCs, including CRUD operations.
    /// </summary>
    public class PCService : IPCService
    {
        private readonly IPCRepository _pcRepository;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PCService"/> class.
        /// </summary>
        /// <param name="pcRepository">Repository for PC data access.</param>
        /// <param name="loggerService">Service for logging operations.</param>
        public PCService(IPCRepository pcRepository, ILoggerService loggerService)
        {
            _pcRepository = pcRepository;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Retrieves all PCs from the repository.
        /// </summary>
        /// <returns>A list of all PCs.</returns>
        public async Task<List<PCResponseDto>> GetAllPCsAsync()
        {
            _loggerService.LogInformation("Fetching all PCs.");
            var pcs = await _pcRepository.GetAllPCsAsync();
            return pcs.Select(pc => new PCResponseDto
            {
                Id = pc.Id,
                Cost = pc.Cost.Value,
                PerformanceRate = pc.Performance.Value
            }).ToList();
        }

        /// <summary>
        /// Retrieves a PC by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the PC.</param>
        /// <returns>The PC associated with the specified ID.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the PC is not found.</exception>
        public async Task<PCResponseDto> GetPCByIdAsync(Guid id)
        {
            _loggerService.LogInformation($"Fetching PC with ID: {id}.");
            var pc = await _pcRepository.GetPCByIdAsync(id);
            if (pc == null)
                throw new KeyNotFoundException("PC not found.");

            return new PCResponseDto
            {
                Id = pc.Id,
                Cost = pc.Cost.Value,
                PerformanceRate = pc.Performance.Value
            };
        }

        /// <summary>
        /// Adds a new PC to the repository.
        /// </summary>
        /// <param name="pcDto">Data transfer object containing PC details.</param>
        /// <exception cref="ArgumentException">Thrown when the performance rate is less than or equal to 4.</exception>
        public async Task AddPCAsync(PCDto pcDto)
        {
            if (pcDto.PerformanceRate <= 4)
                throw new ArgumentException("Performance rate must be greater than 4.");

            var pc = new PC(
                new Cost(pcDto.Cost, "USD"),
                new PerformanceRate(pcDto.PerformanceRate)
            );

            await _pcRepository.AddPCAsync(pc);
            _loggerService.LogInformation($"Added new PC with ID: {pc.Id}.");
        }

        /// <summary>
        /// Updates an existing PC.
        /// </summary>
        /// <param name="id">The unique identifier of the PC to update.</param>
        /// <param name="pcDto">Data transfer object containing updated PC details.</param>
        /// <exception cref="ArgumentException">Thrown when the performance rate is less than or equal to 4.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the PC is not found.</exception>
        public async Task UpdatePCAsync(Guid id, PCDto pcDto)
        {
            if (pcDto.PerformanceRate <= 4)
                throw new ArgumentException("Performance rate must be greater than 4.");

            var pc = await _pcRepository.GetPCByIdAsync(id);
            if (pc == null)
                throw new KeyNotFoundException("PC not found.");

            pc.UpdatePc(new Cost(pcDto.Cost, "USD"), new PerformanceRate(pcDto.PerformanceRate));

            await _pcRepository.UpdatePCAsync(pc);
            _loggerService.LogInformation($"Updated PC with ID: {pc.Id}.");
        }

        /// <summary>
        /// Deletes a PC by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the PC to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the PC is not found.</exception>
        public async Task DeletePCAsync(Guid id)
        {
            var pc = await _pcRepository.GetPCByIdAsync(id);
            if (pc == null)
                throw new KeyNotFoundException("PC not found.");

            await _pcRepository.DeletePCAsync(id);
            _loggerService.LogInformation($"Deleted PC with ID: {id}.");
        }
    }
}
