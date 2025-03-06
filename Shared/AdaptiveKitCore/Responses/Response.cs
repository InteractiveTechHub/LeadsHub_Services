
using AdaptiveKitCore.Responses.Interfaces;

namespace AdaptiveKitCore.Responses
{
    /// <summary>
    /// This is a type of <see cref="Response"/ class that wraps the content of the response data>
    /// </summary>
    /// <typeparam name="T">Object to be instanced</typeparam>
    public class Response<T> : BaseResponse, IResponse<T> where T : class
    {
        /// <summary>
        /// Default empty response constructor
        /// </summary>
        public Response()
        {            
        }

        /// <summary>
        /// Constructor to create a response with a single item
        /// </summary>
        /// <param name="item"></param>
        public Response(T item)
        {
            if (item != null)
            {
                this.ResponseData.Add(item);
            }            
        }

        /// <summary>
        /// Constructor for creating a response with a collection of objects.
        /// </summary>
        /// <param name="items">Collection of object</param>
        public Response(List<T> items)
        {
            this.ResponseData = items;
        }

        /// <summary>
        /// Collection to return objects based on the request criteria.
        /// </summary>
        public List<T> ResponseData { get; set; } = [];

        public int TotalAvailableItems { get; set; }
    }
}
