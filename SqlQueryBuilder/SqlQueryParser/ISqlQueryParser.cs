namespace SqlQueryBuilder.SqlQueryParser
{
    public interface ISqlQueryBuilderParser
    {
        string ParseJsonIntoSqlQuery(string json);
    }
}