using System;

namespace SocietiesManagementSystem.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int SocietyId { get; set; }
        public string SocietyName { get; set; } // For display
        public string Status { get; set; } // 'Pending', 'Approved', 'Cancelled'
        public int MaxTickets { get; set; }
    }
}
