
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Model;
using AdaptiveKitCore.Requests;

namespace WhatsApp.Core.Extentions
{
    public static class QueryHelperExtensions
    {
        public static string BuildWhereClause(this FilterRequest filterRequest)
        {
            string clausuleWhere = filterRequest.FilterDescriptors.Any() ? "WHERE " : string.Empty;

            bool isFirst = true;
            foreach (FilterDescriptor filter in filterRequest.FilterDescriptors)
            {
                bool hasAlias = !string.IsNullOrWhiteSpace(filter.AliasName);

                string property = hasAlias ? $"{filter.AliasName}.\"{filter.PropertyName}\"" : $"\"{filter.PropertyName}\"";

                if (isFirst) 
                {
                    clausuleWhere += $"{property} {BuildComparisonStatement(filter)} ";                    
                }
                
                if (!isFirst)
                {
                    clausuleWhere += $"{filter.FilterConnector} {property} {BuildComparisonStatement(filter)} ";
                }

                isFirst = false;
            }

            return clausuleWhere;
        }

        private static string BuildComparisonStatement(FilterDescriptor filter)
        {
            string statement = filter.FilterOperator switch
            { 
                FilterOperatorEnum.Equals => $"= '{filter.Value}'",
                FilterOperatorEnum.NotEquals => $"<> '{filter.Value}'",
                FilterOperatorEnum.Contains => $"ILIKE '%{filter.Value}%'",
                FilterOperatorEnum.NotContains => $"NOT ILIKE '%{filter.Value}%'",
                FilterOperatorEnum.StartsWith => $"ILIKE '{filter.Value}%'",
                FilterOperatorEnum.EndsWith => $"ILIKE '%{filter.Value}'",
                FilterOperatorEnum.GreaterThan => $"> '{filter.Value}'",
                FilterOperatorEnum.LessThan => $"< '{filter.Value}'",
                FilterOperatorEnum.GreaterThanOrEquals => $">= '{filter.Value}'",
                FilterOperatorEnum.LessThanOrEquals => $"<= '{filter.Value}'",
                FilterOperatorEnum.In => $"IN ({filter.Value})",
                FilterOperatorEnum.NotIn => $"NOT IN ({filter.Value})",
                FilterOperatorEnum.IsNull => "IS NULL",
                FilterOperatorEnum.IsNotNull => "IS NOT NULL",                
                _ => $"= '{filter.Value}'"
            };

            return statement;
        }
    }
}
