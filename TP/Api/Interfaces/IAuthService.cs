using Api.Dtos;

namespace Api.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, string Token, string RefreshToken, List<string> Errors)> LoginAsync(LoginDto dto);
        Task<(bool Success, string Token, List<string> Errors)> RefreshTokenAsync(RefreshDto dto);
        Task<(bool Success, List<string> Errors)> RegisterTechnicianAsync(RegisterTechnicianDto dto);
        Task<List<string>> GetAllTechniciansAsync();
    }
}
