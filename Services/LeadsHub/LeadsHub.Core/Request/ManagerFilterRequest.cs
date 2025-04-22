namespace LeadsHub.Core.Request
{
    public sealed class ManagerFilterRequest
    {
        public bool IsLeadCreatedAtDesc { get; set; }

        public bool IsLeadCreatedAtAsc { get; set; }

        public bool IsInteractionDesc { get; set; }

        public bool IsInteractionAsc { get; set; }

        public bool IsWinClosed { get; set; }

        public bool IsLostClosed { get; set; }

        public string GlobalFilter { get; set; } = string.Empty;
    }
}
