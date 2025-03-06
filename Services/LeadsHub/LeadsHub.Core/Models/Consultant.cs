
using LeadsHub.Core.Dtos;

namespace LeadsHub.Core.Models
{
    public sealed class Consultant : BaseModel
    {
        /// <summary>
        /// Represents the Id from Application User
        /// </summary>
        public string IdentityId { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disabel consultant activities
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// The user/consultant full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        public DateTimeOffset TimeLastLeadAssigned { get; set; }

        /// <summary>
        /// The name that user wants to be called by the system
        /// </summary>
        public string NickName { get; set; } = string.Empty;

        /// <summary>
        /// Photo of the consultant/user
        /// </summary>
        public string PhotoUrl { get; set; } = string.Empty;        

        public ICollection<Company> Companies { get; set; } = [];

        public ApplicationUserDto UserIdentity { get; set; } = new();
    }
}
