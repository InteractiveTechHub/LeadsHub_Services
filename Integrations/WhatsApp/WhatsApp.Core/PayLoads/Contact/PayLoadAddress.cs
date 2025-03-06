
using System.Text.Json.Serialization;

namespace WhatsApp.Core.PayLoads.Contact
{
    public sealed class PayLoadAddress
    {
        /// <summary>
        /// Street number and name
        /// </summary>
        public string Street { get; set; } = string.Empty;  

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State code
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Zip code
        /// </summary>
        public string Zip { get; set; } = string.Empty;

        /// <summary>
        /// Country name
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Country code
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Address type
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
