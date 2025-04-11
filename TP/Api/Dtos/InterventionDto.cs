namespace Api.Dtos
{
    public class InterventionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public List<string> TechnicianEmails { get; set; } = new();
    }
}
