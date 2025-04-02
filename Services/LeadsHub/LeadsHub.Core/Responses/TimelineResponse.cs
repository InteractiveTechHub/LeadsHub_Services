
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Responses
{
    public sealed class TimelineResponse : BaseResponse<Timeline>
    {
        public bool CanSendMessage { get; set; }
    }
}
