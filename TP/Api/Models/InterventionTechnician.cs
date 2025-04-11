namespace Api.Models
{
    public class InterventionTechnician
    {
        public int InterventionId { get; set; }
        public Intervention Intervention { get; set; } = null!;

        public string TechnicianId { get; set; } = string.Empty;
        public ApplicationUser Technician { get; set; } = null!;
    }
}
