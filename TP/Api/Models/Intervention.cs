namespace Api.Models
{
    public class Intervention
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public ICollection<InterventionTechnician> Technicians { get; set; } = new List<InterventionTechnician>();
    }
}
