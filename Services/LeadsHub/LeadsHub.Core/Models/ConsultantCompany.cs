
namespace LeadsHub.Core.Models
{
    public class ConsultantCompany : BaseModel
    {
        /// <summary>
        /// AspNetUser Identity id
        /// </summary>
        public string IdentityId { get; set; } = string.Empty;

        /// <summary>
        /// Sequential Id
        /// </summary>
        public long ConsultantId { get; set; }

        /// <summary>
        /// Company Id
        /// </summary>
        public long CompanyId { get; set; }
    }
}
