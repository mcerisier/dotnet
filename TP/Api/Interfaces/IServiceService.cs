using Api.Dtos;

namespace Api.Interfaces
{
    public interface IServiceService
    {
        Task<List<ServiceDto>> GetAllAsync();
        Task<ServiceDto?> GetByIdAsync(int id);
        Task<ServiceDto> CreateAsync(ServiceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
