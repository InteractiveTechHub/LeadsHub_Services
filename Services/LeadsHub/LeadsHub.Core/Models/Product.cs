using System.ComponentModel.DataAnnotations.Schema;

namespace LeadsHub.Core.Models
{
    public sealed class Product : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public string Category { get; set; } = string.Empty;
        
        public string ProductCode { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
    }
}
