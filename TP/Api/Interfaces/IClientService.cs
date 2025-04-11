using Api.Dtos;

namespace Api.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAllAsync();
        Task<ClientDto?> GetByIdAsync(int id);
        Task<ClientDto> CreateAsync(ClientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
