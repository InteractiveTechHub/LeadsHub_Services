using Microsoft.AspNetCore.Identity;

namespace LeadsHub.Core.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// This is set to intentionally not permit the user
        /// loggin in the application.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Date and time of the credential was created
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Date and time of the last time credential
        /// was updated
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
