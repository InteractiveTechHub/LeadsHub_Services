

using AdaptiveKitCore.Enums;

namespace AdaptiveKitCore.Model
{
    public partial class FilterDescriptor
    {
        /// <summary>
        /// Empty constructor, default
        /// </summary>
        public FilterDescriptor()
        {
        }

        public string PropertyName { get; set; } = string.Empty;

        public FilterOperatorEnum FilterOperator { get; set; }

        public FilterConnectorEnum FilterConnector { get; set; }

        public object? Value { get; set; }

        public string AliasName { get; set; } = "";

        public bool IgnoreAccent { get; set; } = false;
    }
}
