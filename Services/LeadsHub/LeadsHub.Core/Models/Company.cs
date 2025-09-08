
using System.Text.Json.Serialization;

namespace LeadsHub.Core.Models
{
    public sealed class Company : BaseModel
    {
        /// <summary>
        /// Postgres Default UUID 
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// The commercial name
        /// </summary>
        public string BrandName { get; set; } = string.Empty;

        /// <summary>
        /// The name legally registered
        /// </summary>
        public string LegalName { get; set; } = string.Empty;

        /// <summary>
        /// The best email of the company
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Active and inactive the company
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Ex.: CNPJ
        /// </summary>
        public string IdentificationNumber { get; set; } = string.Empty;

        /// <summary>
        /// The main phone number of the company
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        public Address Address { get; set; } = new();
    }
}
