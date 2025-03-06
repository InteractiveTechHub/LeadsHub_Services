
namespace WhatsApp.Core.PayLoads.Contact
{
    public sealed class PayLoadOrg
    {
        /// <summary>
        /// Company or organization name
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Department name
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Job title
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}
