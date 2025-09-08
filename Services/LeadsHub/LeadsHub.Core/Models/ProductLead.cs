using System.ComponentModel.DataAnnotations.Schema;

namespace LeadsHub.Core.Models
{
    public sealed class ProductLead : BaseModel
    {
        public long ProductId { get; set; }
        
        public long LeadId { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        public DateTimeOffset RelationshipDate { get; set; } = DateTimeOffset.UtcNow;
        
        public string Note { get; set; } = string.Empty;
        
        public Product Product { get; set; } = new();
        
        public Lead Lead { get; set; } = new();
    }
}
