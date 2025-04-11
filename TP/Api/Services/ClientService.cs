using Api.Data;
using Api.Dtos;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            return await _context.Clients
                .Select(c => new ClientDto
                {
                    Name = c.Name,
                    Address = c.Address,
                    Phone = c.Phone,
                    Email = c.Email
                })
                .ToListAsync();
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            var c = await _context.Clients.FindAsync(id);
            if (c == null) return null;

            return new ClientDto
            {
                Name = c.Name,
                Address = c.Address,
                Phone = c.Phone,
                Email = c.Email
            };
        }

        public async Task<ClientDto> CreateAsync(ClientDto dto)
        {
            var client = new Client
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
