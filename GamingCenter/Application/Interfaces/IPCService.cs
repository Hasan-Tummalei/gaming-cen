using GamingCenter.Application.DTOs;

namespace GamingCenter.Application.Interfaces
{
    namespace GamingCenter.Application.Interfaces
    {
        public interface IPCService
        {
            Task<List<PCResponseDto>> GetAllPCsAsync();
            Task<PCResponseDto> GetPCByIdAsync(Guid id);
            Task AddPCAsync(PCDto pcDto);
            Task UpdatePCAsync(Guid id, PCDto pcDto);
            Task DeletePCAsync(Guid id);
        }
    }
}
