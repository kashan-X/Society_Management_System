namespace SocietiesManagementSystem.Models
{
    public class Membership
    {
        public int MembershipId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } // For display
        public int SocietyId { get; set; }
        public string SocietyName { get; set; } // For display
        public string Status { get; set; } // 'Pending', 'Approved', 'Rejected'
    }
}
