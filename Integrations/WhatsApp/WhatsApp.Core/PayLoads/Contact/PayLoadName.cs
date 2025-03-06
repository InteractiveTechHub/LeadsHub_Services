
using System.Text.Json.Serialization;

namespace WhatsApp.Core.PayLoads.Contact
{
    public sealed class PayLoadName
    {
        /// <summary>
        /// Full Name
        /// </summary>
        [JsonPropertyName("formatted_name")]
        public string FormattedName { get; set; } = string.Empty;

        /// <summary>
        /// First Name
        /// </summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last Name
        /// </summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Middle name
        /// </summary>
        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix { get; set; } = string.Empty;

        /// <summary>
        /// Preffix
        /// </summary>
        public string Preffix { get; set; } = string.Empty;
    }
}
