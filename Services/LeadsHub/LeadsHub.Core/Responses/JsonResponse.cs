
using AdaptiveKitCore.Responses;

namespace LeadsHub.Core.Responses
{
    public sealed class JsonResponse : BaseResponse
    {
        public string DataJson { get; set; } = string.Empty;
    }
}
