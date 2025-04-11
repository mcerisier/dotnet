namespace Api.Dtos
{
    public class CreateInterventionDto
    {
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public List<string> TechnicianIds { get; set; } = new();
    }
}
