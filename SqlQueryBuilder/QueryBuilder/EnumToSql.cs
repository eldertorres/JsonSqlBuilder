using System;

namespace SqlQueryBuilder.QueryBuilder
{
    public static class EnumToSql
    {
        public static string ComparisonOperatorToSqlOperator(ComparisonOperator comparisonOperator)
        {
            return comparisonOperator switch
            {
                ComparisonOperator.Equals => "=",
                ComparisonOperator.GreaterThan => ">",
                ComparisonOperator.LessThan => "<",
                ComparisonOperator.GreaterOrEqual => ">=",
                ComparisonOperator.LessOrEqual => "<=",
                ComparisonOperator.NotEqual => "<>",
                ComparisonOperator.Like => "like",
                ComparisonOperator.NotLike => "not like",
                ComparisonOperator.In => "in",
                ComparisonOperator.NotIn => "not in",
                ComparisonOperator.Between => "between",
                ComparisonOperator.NotBetween => "not between",
                _ => "="
            };
        }

        public static string JoinOperatorToSqlJoin(JoinOperator joinOperator)
        {
            return joinOperator switch
            {
                JoinOperator.InnerJoin => "inner join",
                JoinOperator.LeftJoin => "left join",
                JoinOperator.RightJoin => "right join",
                JoinOperator.FullOuterJoin => "full outer join",
                _ => "inner join"
            };
        }
    }
}