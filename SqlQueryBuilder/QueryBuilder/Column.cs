using System.Text.Json.Serialization;

namespace SqlQueryBuilder.QueryBuilder
{
    public class Column
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}