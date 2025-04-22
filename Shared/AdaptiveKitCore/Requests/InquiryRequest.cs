
using System.Text;

namespace AdaptiveKitCore.Requests
{
    public class InquiryRequest
    {
        /// <summary>
        /// Represent page to skip or page number
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Represent the requested number of rows.
        /// </summary>
        public int PageSize { get; set; }

        public IEnumerable<SortExpression> SortExpressions
        {
            get
            {
                List<SortExpression> sortExpressions = new();

                int priority = 1;
                foreach (string orderBy in ShorthandSortExpressions)
                {
                    sortExpressions.Add(SortExpression.Parse(orderBy, priority));
                    priority++;
                }

                sortExpressions.Sort((x, y) => x.Priority.CompareTo(y.Priority));

                return sortExpressions;
            }
        }

        public List<string> ShorthandSortExpressions { get; set; } = new();

        public InquiryRequest AddSortExpressionAscending(string propertyName, string columnAlias = "", string tableAlias = "")
        {
            ShorthandSortExpressions.Add("+" + propertyName + "-" + tableAlias + "-" + columnAlias);
            return this;
        }

        public InquiryRequest AddSortExpressionDescending(string propertyName, string columnAlias = "", string tableAlias = "")
        {
            ShorthandSortExpressions.Add("-" + propertyName + "-" + tableAlias + "-" + columnAlias);
            return this;
        }

        public string BuildSortExpressionForSql(string? tablePrefix = null)
        {
            StringBuilder sb = new();

            foreach (SortExpression exp in SortExpressions)
            {
                sb.Append((tablePrefix == null) ?
                    exp.ToSqlFragment().Trim() + ", " :
                    exp.ToSqlFragmentWithPrefix(tablePrefix).Trim() + ", ");
            }

            //remove last comma and space
            sb.Length -= 2;
            return sb.ToString() + " ";
        }
    }
}
