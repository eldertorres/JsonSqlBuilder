using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SqlKata;
using SqlQueryBuilder.QueryBuilder;
using SqlQueryBuilder.QueryFactory;
using Join = SqlQueryBuilder.QueryBuilder.Join;

namespace SqlQueryBuilder.SqlQueryParser
{
    public class SqlQueryBuilderParser : ISqlQueryBuilderParser
    {

        private readonly IQueryFactory _queryFactory;
        
        public SqlQueryBuilderParser(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }
        
        public string ParseJsonIntoSqlQuery(string json)
        {
            var queryData = JsonSerializer.Deserialize<QueryData>(json);
            var query = ParseSelect(queryData);
            
            var result = _queryFactory.Compiler.Compile(query);
           
            // To execute in production use result.Sql + result.Bindings to avoid sql injection and for optimal and cached execution plan
            var sql = result.ToString();

            return sql;
        }

        private Query ParseSelect(QueryData queryData)
        {
            var query = _queryFactory.Query(queryData.Table);
            if (queryData.Columns.Any())
                query.Select(queryData.Columns.Select(column => column.Name).ToArray());

            ParseJoins(queryData.Joins, query);

            ParseWheres(queryData.Wheres, query);

            query.Limit(queryData.Limit);
            query.Offset(queryData.Offset);
            
            return query;
        }

        private void ParseJoins(List<Join> joins, Query query)
        {
            if (!joins.Any()) return;
            foreach (var @join in joins)
            {
                query.Join(join.Table, join.FromColumn, join.ToColumn,
                    EnumToSql.ComparisonOperatorToSqlOperator(join.ComparisonOperator), 
                    EnumToSql.JoinOperatorToSqlJoin(join.JoinOperator));
            }
        }

        private void ParseWheres(List<Where> wheres, Query query)
        {
            if (!wheres.Any()) return;
            foreach (var where in wheres)
            {
                query.Where(wrap =>
                {
                    switch (where.ComparisonOperator)
                    {
                        case ComparisonOperator.In:
                            wrap.WhereIn(where.ColumnName, where.ColumnValue.ToString()?.Split(','));
                            break;
                        case ComparisonOperator.NotIn:
                            wrap.WhereNotIn(where.ColumnName, where.ColumnValue.ToString()?.Split(','));
                            break;
                        case ComparisonOperator.Like:
                            wrap.WhereLike(where.ColumnName, where.ColumnValue);
                            break;
                        case ComparisonOperator.NotLike:
                            wrap.WhereNotLike(where.ColumnName, where.ColumnValue);
                            break;
                        case ComparisonOperator.Between:
                            wrap.WhereBetween(where.ColumnName, where.ColumnValue, where.ColumnValueMax);
                            break;
                        case ComparisonOperator.NotBetween:
                            wrap.WhereNotBetween(where.ColumnName, where.ColumnValue, where.ColumnValueMax);
                            break;
                        default:
                            wrap.Where(where.ColumnName,
                                EnumToSql.ComparisonOperatorToSqlOperator(where.ComparisonOperator), where.ColumnValue);
                            break;
                    }

                    ParseOrClauses(where.OrClauses, wrap);
                    return wrap;
                });
            }
        }
        
        private void ParseOrClauses(List<Where> wheres, Query query)
        {
            if (!wheres.Any()) return;
            foreach (var where in wheres)
            {
                switch (where.ComparisonOperator)
                {
                    case ComparisonOperator.In:
                        query.OrWhereIn(where.ColumnName, where.ColumnValue.ToString()?.Split(','));
                        break;
                    case ComparisonOperator.NotIn:
                        query.OrWhereNotIn(where.ColumnName, where.ColumnValue.ToString()?.Split(','));
                        break;
                    case ComparisonOperator.Like:
                        query.OrWhereLike(where.ColumnName, where.ColumnValue);
                        break;
                    case ComparisonOperator.NotLike:
                        query.OrWhereNotLike(where.ColumnName, where.ColumnValue);
                        break;
                    case ComparisonOperator.Between:
                        query.OrWhereBetween(where.ColumnName, where.ColumnValue, where.ColumnValueMax);
                        break;                   
                    case ComparisonOperator.NotBetween:
                        query.OrWhereNotBetween(where.ColumnName, where.ColumnValue, where.ColumnValueMax);
                        break;
                    default:
                        query.OrWhere(where.ColumnName, EnumToSql.ComparisonOperatorToSqlOperator(where.ComparisonOperator), where.ColumnValue);        
                        break;
                }
            }
        }

    }
}