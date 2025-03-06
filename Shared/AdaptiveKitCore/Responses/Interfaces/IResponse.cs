
namespace AdaptiveKitCore.Responses.Interfaces
{
    /// <summary>
    /// The definition of Response type
    /// </summary>
    /// <typeparam name="T">Object to be instanced</typeparam>
    public interface IResponse<T>
    {
        public List<T> ResponseData { get; set; }
    }
}
