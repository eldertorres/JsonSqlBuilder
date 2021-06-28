using System.Text.Json.Serialization;

namespace SqlQueryBuilder.QueryBuilder
{
    public class Join
    {
        [JsonPropertyName("joinOperator")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public JoinOperator JoinOperator { get; set; }
        
        [JsonPropertyName("table")]
        public string Table { get; set; }
        
        [JsonPropertyName("fromColumn")]
        public string FromColumn { get; set; }
        
        [JsonPropertyName("operator")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ComparisonOperator ComparisonOperator { get; set; }
        
        [JsonPropertyName("toColumn")]
        public string ToColumn { get; set; }
        
    }
}