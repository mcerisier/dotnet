using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("interventions")]
    public class InterventionController : ControllerBase
    {
        private readonly IInterventionService _service;

        public InterventionController(IInterventionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] CreateInterventionDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            var interventions = await _service.GetAllAsync();
            return Ok(interventions);
        }

        [HttpGet("mine")]
        [Authorize(Roles = "technicien")]
        public async Task<IActionResult> GetMine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var interventions = await _service.GetForTechnicianAsync(userId);
            return Ok(interventions);
        }
    }
}
