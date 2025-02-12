using GamingCenter.Domain.Entities;

namespace GamingCenter.Domain.Interfaces
{
    public interface IPCRepository
    {
        Task<List<PC>> GetAllPCsAsync();
        Task<PC?> GetPCByIdAsync(Guid id);
        Task AddPCAsync(PC pc);
        Task UpdatePCAsync(PC pc);
        Task DeletePCAsync(Guid id);
    }
}
