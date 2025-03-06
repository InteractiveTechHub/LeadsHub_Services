using AdaptiveKitCore.Responses;

namespace LeadsHub.Core.Responses
{
    /// <summary>
    /// It represents a Response. 
    /// It helps easily delivery response throught method signatures and http bodies and headers
    /// It should contain only parameters and methods that define a response.
    /// </summary>
    public class BaseResponse<T> : Response<T> where T : class
    {
    }
}
