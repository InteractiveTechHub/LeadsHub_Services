
using System.Text.Json.Serialization;

namespace LeadsHub.Core.Models
{
    public class BaseModel
    {
        public virtual long Id { get; set; }

        [JsonIgnore]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonIgnore]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
