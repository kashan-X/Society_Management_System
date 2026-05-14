namespace SocietiesManagementSystem.Models
{
    public class Society
    {
        public int SocietyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // 'Pending', 'Approved', 'Suspended'
        public int? HeadId { get; set; }
    }
}