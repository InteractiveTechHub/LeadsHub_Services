
namespace AdaptiveKitCore.Requests
{
    public sealed class SortExpression
    {
        public enum Direction
        {
            ASC,
            DESC
        }

        public string PropertyName { get; set; } = string.Empty;

        public List<string> PropertyValues { get; set; } = new();

        public Direction SortDirection { get; set; }

        public int Priority { get; set; }

        public string? TableAlias { get; set; }

        public string? ColumnAlias { get; set; }

        public SortExpression()
        {
        }

        public SortExpression(int priority)
        {
            this.Priority = priority;
        }

        public SortExpression(string propName, Direction sortDirection, int priority)
        {
            this.PropertyName = propName;
            this.SortDirection = sortDirection;
            this.Priority = priority;
        }

        public SortExpression(string propName, string tableAlias, string columnAlias, Direction sortDirection, int priority)
        {
            this.PropertyName = propName;
            this.TableAlias = tableAlias;
            this.ColumnAlias = columnAlias;
            this.SortDirection = sortDirection;
            this.Priority = priority;
        }

        public string ToSqlFragment()
        {
            return $" {PropertyName} {SortDirection} ";
        }

        public string ToSqlFragmentWithPrefix(string prefix)
        {
            return $" {prefix}.{PropertyName} {SortDirection} ";
        }

        public static SortExpression Parse(string sortBy, int priority = 1)
        {
            SortExpression expression = new()
            {
                Priority = priority
            };

            if (sortBy.StartsWith('-'))
                expression.SortDirection = Direction.DESC;
            else if (sortBy.StartsWith('+'))
                expression.SortDirection = Direction.ASC;

            if (sortBy.Contains('~'))
            {
                string properties = sortBy[1..];
                string[] splitProperties = properties.Split("-");
                string[] splitPropertyValues = splitProperties[1].Split("~");

                expression.PropertyName = splitProperties[0];
                expression.PropertyValues.AddRange(splitPropertyValues);
                expression.TableAlias = string.IsNullOrEmpty(splitProperties[2]) ? null : splitProperties[2];
                expression.ColumnAlias = string.IsNullOrEmpty(splitProperties[3]) ? null : splitProperties[3];
            }
            else
            {
                string properties = sortBy[1..];
                string[] splitProperties = properties.Split("-");
                expression.PropertyName = splitProperties[0];
                expression.TableAlias = string.IsNullOrEmpty(splitProperties[1]) ? null : splitProperties[1];
                expression.ColumnAlias = string.IsNullOrEmpty(splitProperties[2]) ? null : splitProperties[2];
            }

            return expression;
        }
    }
}
