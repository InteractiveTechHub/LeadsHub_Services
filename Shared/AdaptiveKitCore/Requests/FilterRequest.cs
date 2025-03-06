
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Model;

namespace AdaptiveKitCore.Requests
{
    public sealed class FilterRequest : InquiryRequest
    {
        public IList<FilterDescriptor> FilterDescriptors { get; set; } = [];

        public FilterConnectorEnum FilterConnetor { get; set; } = new();

        public void AddFilter(string propertyName, FilterOperatorEnum filterOperator)
        {
            FilterDescriptors.Add(new FilterDescriptor()
            {
                PropertyName = propertyName,
                FilterOperator = filterOperator,
            });
        }

        public void AddFilter(string propertyName, FilterOperatorEnum filterOperator, object value, string alias = "") 
        {
            FilterDescriptors.Add(new FilterDescriptor()
            {
                PropertyName = propertyName,
                FilterOperator = filterOperator,
                Value = value,
                AliasName = alias
            });
        }

        public void AddFilter(string propertyName, FilterOperatorEnum filterOperator, FilterConnectorEnum connector, object value, string alias = "")
        {
            FilterDescriptors.Add(new FilterDescriptor()
            {
                PropertyName = propertyName,
                FilterOperator = filterOperator,
                FilterConnector = connector,
                Value = value,
                AliasName = alias
            });
        }
    }
}
