using System.Text.Json.Serialization;

namespace SqlQueryBuilder.QueryBuilder
{
    public class Order
    {
        [JsonPropertyName("orderType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderType OrderType { get; set; }
        
        [JsonPropertyName("columnName")] 
        public string ColumnName { get; set; }
    }
}