using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Intervention> Interventions => Set<Intervention>();
        public DbSet<InterventionTechnician> InterventionTechnicians => Set<InterventionTechnician>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InterventionTechnician>()
                .HasKey(it => new { it.InterventionId, it.TechnicianId });

            builder.Entity<InterventionTechnician>()
                .HasOne(it => it.Intervention)
                .WithMany(i => i.Technicians)
                .HasForeignKey(it => it.InterventionId);

            builder.Entity<InterventionTechnician>()
                .HasOne(it => it.Technician)
                .WithMany()
                .HasForeignKey(it => it.TechnicianId);
        }
    }
}
