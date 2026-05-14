namespace SocietiesManagementSystem.Models
{
    public class SocietyTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public int SocietyId { get; set; }
        public string Status { get; set; } // 'Pending', 'Completed'
    }
}
