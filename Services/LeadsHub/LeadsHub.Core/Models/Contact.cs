
namespace LeadsHub.Core.Models
{
    public sealed class Contact : BaseModel
    {
        public DateTime? BirthDate { get; set; }

        public string CPF { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
