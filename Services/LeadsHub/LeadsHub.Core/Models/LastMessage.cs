using System.ComponentModel.DataAnnotations.Schema;

namespace LeadsHub.Core.Models
{
    public sealed class LastMessage
    {
        public long LeadId { get; set; }
        
        public long TimelineId { get; set; }
        
        [Column("LastMessage")]
        public string LastMessageText { get; set; } = string.Empty;
        
        public DateTimeOffset LastMessageDate { get; set; }
        
        public short Status { get; set; } = 1;
        
        public Lead Lead { get; set; } = new();
        
        public Timeline Timeline { get; set; } = new();
    }
}
