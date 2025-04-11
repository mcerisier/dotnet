using Api.Data;
using Api.Dtos;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;

        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceDto>> GetAllAsync()
        {
            return await _context.Services
                .Select(s => new ServiceDto { Name = s.Name })
                .ToListAsync();
        }

        public async Task<ServiceDto?> GetByIdAsync(int id)
        {
            var s = await _context.Services.FindAsync(id);
            if (s == null) return null;

            return new ServiceDto { Name = s.Name };
        }

        public async Task<ServiceDto> CreateAsync(ServiceDto dto)
        {
            var service = new Service { Name = dto.Name };
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var s = await _context.Services.FindAsync(id);
            if (s == null) return false;

            _context.Services.Remove(s);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
