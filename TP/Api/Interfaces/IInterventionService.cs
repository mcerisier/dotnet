using Api.Dtos;

namespace Api.Interfaces
{
    public interface IInterventionService
    {
        Task<InterventionDto> CreateAsync(CreateInterventionDto dto);
        Task<List<InterventionDto>> GetAllAsync();
        Task<List<InterventionDto>> GetForTechnicianAsync(string technicianId);
    }
}
