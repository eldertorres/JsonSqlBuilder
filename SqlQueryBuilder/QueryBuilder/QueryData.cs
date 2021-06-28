using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SqlQueryBuilder.QueryBuilder
{
    public class QueryData
    {
        public QueryData()
        {
            Columns = new List<Column>();
            Joins = new List<Join>();
            Wheres = new List<Where>();
        }

        [JsonPropertyName("columns")]
        public List<Column> Columns { get; set; }

        [JsonPropertyName("table")]
        public string Table { get; set; }
        
        [JsonPropertyName("joins")]
        public List<Join> Joins { get; set; }
        
        [JsonPropertyName("wheres")]
        public List<Where> Wheres { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        public string ParsedSql { get; set; }
    }
}