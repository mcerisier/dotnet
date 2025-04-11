using Api.Data;
using Api.Dtos;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class InterventionService : IInterventionService
    {
        private readonly ApplicationDbContext _context;

        public InterventionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterventionDto> CreateAsync(CreateInterventionDto dto)
        {
            // validation simple (tu peux ajouter + tard du vrai model validation localisé)
            if (dto.Date < DateTime.Now)
                throw new ArgumentException("La date de l'intervention ne peut pas être dans le passé.");

            var intervention = new Intervention
            {
                Date = dto.Date,
                ClientId = dto.ClientId,
                ServiceId = dto.ServiceId,
            };

            foreach (var techId in dto.TechnicianIds)
            {
                intervention.Technicians.Add(new InterventionTechnician
                {
                    TechnicianId = techId
                });
            }

            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return await MapToDtoAsync(intervention.Id);
        }

        public async Task<List<InterventionDto>> GetAllAsync()
        {
            return await _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Service)
                .Include(i => i.Technicians)
                    .ThenInclude(it => it.Technician)
                .Select(i => new InterventionDto
                {
                    Id = i.Id,
                    Date = i.Date,
                    ClientName = i.Client.Name,
                    ServiceName = i.Service.Name,
                    TechnicianEmails = i.Technicians.Select(t => t.Technician.Email).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<InterventionDto>> GetForTechnicianAsync(string technicianId)
        {
            return await _context.InterventionTechnicians
                .Where(it => it.TechnicianId == technicianId)
                .Include(it => it.Intervention)
                    .ThenInclude(i => i.Client)
                .Include(it => it.Intervention)
                    .ThenInclude(i => i.Service)
                .Include(it => it.Technician)
                .Select(it => new InterventionDto
                {
                    Id = it.Intervention.Id,
                    Date = it.Intervention.Date,
                    ClientName = it.Intervention.Client.Name,
                    ServiceName = it.Intervention.Service.Name,
                    TechnicianEmails = it.Intervention.Technicians
                        .Select(t => t.Technician.Email)
                        .ToList()
                })
                .ToListAsync();
        }

        private async Task<InterventionDto> MapToDtoAsync(int id)
        {
            var i = await _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Service)
                .Include(i => i.Technicians)
                    .ThenInclude(t => t.Technician)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (i == null) throw new Exception("Intervention introuvable");

            return new InterventionDto
            {
                Id = i.Id,
                Date = i.Date,
                ClientName = i.Client.Name,
                ServiceName = i.Service.Name,
                TechnicianEmails = i.Technicians.Select(t => t.Technician.Email).ToList()
            };
        }
    }
}
