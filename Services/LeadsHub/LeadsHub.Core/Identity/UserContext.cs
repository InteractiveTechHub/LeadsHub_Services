
using AdaptiveKitCore.Requests;

namespace LeadsHub.Core.Identity
{
    public sealed class UserContext
    {
        public long ConsultantId { get; set; }
        public string IdentityId { get; set; } = string.Empty;
        public long[] CompanyIds { get; set; } = [];
        public List<string> Roles { get; set; } = [];
        public FilterRequest FilterRequest { get; set; } = new();

        public bool IsSysAdmin { get; set; }

        public bool IsSupport { get; set; }

        public bool IsOwner { get; set; }

        public bool IsManager { get; set; }

        public bool IsConsultant { get; set; }
    }
}
