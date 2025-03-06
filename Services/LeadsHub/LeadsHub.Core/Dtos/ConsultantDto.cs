
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Dtos
{
    public class ConsultantDto
    {
        public long Id { get; set; }

        public string IdentityId { get; set; } = string.Empty;

        public List<Company> Companies { get; set; } = [];

        public string? Email { get; set; }

        /// <summary>
        /// If the user is active
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// The user/consultant full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// The name that user wants to be called by the system
        /// </summary>
        public string NickName { get; set; } = string.Empty;

        /// <summary>
        /// The user numbers
        /// </summary>
        public string? PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Photo of the consultant/user
        /// </summary>
        public string PhotoUrl { get; set; } = string.Empty;

        public string Roles { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
