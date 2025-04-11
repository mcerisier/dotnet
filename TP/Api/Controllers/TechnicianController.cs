using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("techniciens")]
    [Authorize(Roles = "admin")]
    public class TechnicianController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TechnicianController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTechnician([FromBody] RegisterTechnicianDto dto)
        {
            var result = await _authService.RegisterTechnicianAsync(dto);
            if (!result.Success)
                return BadRequest(new { Errors = result.Errors });

            return Ok(new { Message = "Technicien créé avec succès" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var technicians = await _authService.GetAllTechniciansAsync();
            return Ok(technicians);
        }
    }
}
