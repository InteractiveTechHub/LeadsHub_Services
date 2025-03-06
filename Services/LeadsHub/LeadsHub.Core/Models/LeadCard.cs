
namespace LeadsHub.Core.Models
{
    public sealed class LeadCard
    {
        /// <summary>
        /// Id to identify the consultant
        /// </summary>
        public long? ConsultantId { get; private set; }

        /// <summary>
        /// Consultant Name
        /// </summary>
        public string ConsultantName { get; private set; } = string.Empty;

        /// <summary>
        /// Company Id
        /// </summary>
        public long CompanyId { get; private set; }

        /// <summary>
        /// Date that it was registered in the system
        /// </summary>
        public DateTimeOffset CreatedAt { get; private set; }

        /// <summary>
        /// Id to identify the lead
        /// </summary>
        public long LeadId { get; private set; }

        /// <summary>
        /// The Name of the Lead
        /// </summary>
        public string LeadName { get; private set; } = string.Empty;

        /// <summary>
        /// The Total of new messages in the lead chat
        /// </summary>
        public long TotalNewMessages { get; private set; }

        /// <summary>
        /// The status of the lead
        /// </summary>
        public string Status { get; private set; } = string.Empty;

        /// <summary>
        /// Last message time in the Message chat
        /// </summary>
        public DateTimeOffset LastMessageDate { get; private set; }

        public string LastMessage { get; set; } = string.Empty;

        public int TimelineId { get; set; }

        public string UserIdentityId { get; private set; } = string.Empty;
    }
}
