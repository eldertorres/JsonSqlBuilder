using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SqlQueryBuilder.QueryBuilder
{
    public class Where
    {
        public Where()
        {
            OrClauses = new List<Where>();
        }
        
        [JsonPropertyName("operator")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ComparisonOperator ComparisonOperator { get; set; }
        
        [JsonPropertyName("columnName")] 
        public string ColumnName { get; set; }
        
        [JsonPropertyName("columnValue")]
        public string ColumnValue { get; set; }
        
        [JsonPropertyName("columnValueMax")]
        public string ColumnValueMax { get; set; }
        
        [JsonPropertyName("orClauses")]
        public List<Where> OrClauses { get; set; }
    }
}