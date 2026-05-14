namespace SocietiesManagementSystem.Models
{
    public class EventRegistration
    {
        public int RegistrationId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string Status { get; set; }
    }
}
