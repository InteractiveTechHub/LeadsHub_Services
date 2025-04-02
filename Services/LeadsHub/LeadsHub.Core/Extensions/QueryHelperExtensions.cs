
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Model;
using AdaptiveKitCore.Requests;

namespace LeadsHub.Core.Extentions
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
            string value = filter.Value != null && filter.Value.GetType().IsEnum ? 
                Convert.ToInt32(filter.Value).ToString() : filter.Value?.ToString() ?? string.Empty;

            string statement = filter.FilterOperator switch
            { 
                FilterOperatorEnum.Equals => $"= '{value}'",
                FilterOperatorEnum.NotEquals => $"<> '{value}'",
                FilterOperatorEnum.Contains => $"ILIKE '%{value}%'",
                FilterOperatorEnum.NotContains => $"NOT ILIKE '%{value}%'",
                FilterOperatorEnum.StartsWith => $"ILIKE '{value}%'",
                FilterOperatorEnum.EndsWith => $"ILIKE '%{value}'",
                FilterOperatorEnum.GreaterThan => $"> '{value}'",
                FilterOperatorEnum.LessThan => $"< '{value}'",
                FilterOperatorEnum.GreaterThanOrEquals => $">= '{value}'",
                FilterOperatorEnum.LessThanOrEquals => $"<= '{value}'",
                FilterOperatorEnum.In => $"IN ({value})",
                FilterOperatorEnum.NotIn => $"NOT IN ({value})",
                FilterOperatorEnum.IsNull => "IS NULL",
                FilterOperatorEnum.IsNotNull => "IS NOT NULL",                
                _ => $"= '{value}'"
            };

            return statement;
        }
    }
}
