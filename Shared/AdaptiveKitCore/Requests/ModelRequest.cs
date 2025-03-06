
namespace AdaptiveKitCore.Requests
{
    /// <summary>
    /// This class is used to any type of operation request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ModelRequest<T>
    {
        public ModelRequest(T model)
        {
            Model = model;
        }

        public T Model { get; }
    }
}
