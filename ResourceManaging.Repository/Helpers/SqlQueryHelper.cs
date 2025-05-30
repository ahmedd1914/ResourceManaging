using System.Text;

namespace ResourceManaging.Repository.Helpers
{
    public static class SqlQueryHelper
    {
        public static class QueryTemplates
        {
            public const string SelectAll = "SELECT {0} FROM {1}";
            public const string SelectById = "SELECT {0} FROM {1} WHERE {2} = @{2}";
            public const string Insert = "INSERT INTO {0} ({1}) VALUES ({2}); SELECT SCOPE_IDENTITY();";
            public const string Update = "UPDATE {0} SET {1} WHERE {2} = @{2}";
            public const string Delete = "DELETE FROM {0} WHERE {1} = @{1}";
            public const string Where = " WHERE {0}";
            public const string And = " AND ";
            public const string Or = " OR ";
            public const string Pagination = " OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY";
            public const string OrderBy = " ORDER BY {0} {1}";
            public const string Count = "SELECT COUNT(*) FROM {0}";
            public const string Exists = "SELECT 1 FROM {0} WHERE {1}";
        }
        public static class QueryParts
        {
            public static string BuildColumnList(string columns, string idColumn) =>
                $"{columns}, {idColumn}";

            public static string BuildColumnParameters(IEnumerable<string> columns) =>
                string.Join(", ", columns.Select(c => "@" + c));

            public static string BuildUpdateSet(IEnumerable<string> columns) =>
                string.Join(", ", columns.Select(c => $"{c} = @{c}"));
            public static string BuildWhereConditions(IEnumerable<string> keys, string logicalOperator = "AND") =>
                string.Join(logicalOperator == "AND" ? QueryTemplates.And : QueryTemplates.Or,
                 keys.Select(k => $"{k} = @{k}"));
            public static string BuildOrderBy(string column, bool ascending = true) =>
                string.Format(QueryTemplates.OrderBy, column, ascending ? "ASC" : "DESC");
        }
        public static class Select
        {
            public static string Build(string table, string columns, string idColumn) =>
                string.Format(QueryTemplates.SelectById, columns, table, idColumn);

            public static string BuildAll(string table, string columns, string orderByColumn = null, bool ascending = true)
            {
                var query = new StringBuilder();
                query.AppendFormat(QueryTemplates.SelectAll, columns, table);

                if (!string.IsNullOrEmpty(orderByColumn))
                {
                    query.Append(QueryParts.BuildOrderBy(orderByColumn, ascending));
                }

                return query.ToString();
            }

            public static string BuildCount(string table, Filter filter = null)
            {
                var query = new StringBuilder();
                query.AppendFormat(QueryTemplates.Count, table);
                if (filter?.Count > 0)
                {
                    query.AppendFormat(QueryTemplates.Where, QueryParts.BuildWhereConditions(filter.GetParameters(), "AND"));
                }
                return query.ToString();
            }
            public static string BuildExists(string table, string condition)
            {
                return string.Format(QueryTemplates.Exists, table, condition);
            }
        }
        public static class Insert
        {
            public static string Build(string table, string columns)
            {
                var columnList = columns.Split(',').Select(c => c.Trim());
                var parameterList = QueryParts.BuildColumnParameters(columnList);
                return string.Format(QueryTemplates.Insert, table, columns, parameterList);
            }
        }
        public static class Update
        {
            public static string Build(string table, string columns, string idColumn)
            {
                var columnList = columns.Split(',').Select(c => c.Trim());
                var updateSet = QueryParts.BuildUpdateSet(columnList);
                return string.Format(QueryTemplates.Update, table, updateSet, idColumn);
            }
        }
        public static class Delete
        {
            public static string Build(string table, string idColumn) =>
                string.Format(QueryTemplates.Delete, table, idColumn);
        }
    }
}