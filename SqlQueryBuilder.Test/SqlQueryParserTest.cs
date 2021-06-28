using System;
using Moq;
using SqlKata.Compilers;
using SqlQueryBuilder.QueryFactory;
using SqlQueryBuilder.SqlQueryParser;
using Xunit;

namespace SqlQueryBuilder.Test
{
    public class UnitTest1
    {
        [Fact]
        public void ParseJsonIntoSimpleSqlWithWhere()
        {
            var json = "{\"table\": \"Products\", \"columns\": [{ \"name\": \"Id\" }, { \"name\": \"Name\" }, { \"name\": \"Price\" }], \"wheres\": [ {\"operator\": \"In\", \"columnName\": \"Id\", \"columnValue\": \"10,25,35\" },{ \"operator\": \"Equals\", \"columnName\": \"Name\",\"columnValue\": \"Cellphone\"}]}";
            var expectedSql = "SELECT [Id], [Name], [Price] FROM [Products] WHERE ([Id] IN ('10', '25', '35')) AND ([Name] = 'Cellphone')";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);
            
            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }

        [Fact]
        public void ParseJsonIntoSqlWithJoins()
        {
            var json = "{\"table\": \"Suppliers\",\"columns\": [{\"name\": \"Id\"},{\"name\": \"Name\"},{\"name\": \"Products.Name\"}],\"joins\": [{\"joinOperator\": \"LeftJoin\",\"table\": \"Products\",\"fromColumn\": \"Suppliers.Id\",\"operator\": \"Equals\",\"toColumn\": \"Products.SupplierId\"}],\"wheres\": [{\"operator\": \"NotEqual\",\"columnName\": \"Id\",\"columnValue\": \"2530\"}]}";
            var expectedSql = "SELECT [Id], [Name], [Products].[Name] FROM [Suppliers] LEFT JOIN [Products] ON [Suppliers].[Id] = [Products].[SupplierId] WHERE ([Id] <> '2530')";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);

            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }
        
        [Fact]
        public void ParseJsonIntoSqlWithJoinsAndWheres()
        {
            var json = "{\"table\": \"Suppliers\",\"columns\": [{\"name\": \"Suppliers.Id\"},{\"name\": \"Suppliers.Name as Supplier\"},{\"name\": \"Countries.Name as CountryName\"},{\"name\": \"Products.Name as ProductName\"}],\"joins\": [{\"joinOperator\": \"LeftJoin\",\"table\": \"Products\",\"fromColumn\": \"Suppliers.Id\",\"operator\": \"Equals\",\"toColumn\": \"Products.SupplierId\"},{\"joinOperator\": \"InnerJoin\",\"table\": \"Countries\",\"fromColumn\": \"Suppliers.CountryId\",\"operator\": \"Equals\",\"toColumn\": \"Countries.Id\"}],\"wheres\": [{\"operator\": \"In\",\"columnName\": \"Products.Id\",\"columnValue\": \"1,2,3,4\",\"orClauses\": [{\"operator\": \"GreaterOrEqual\",\"columnName\": \"Products.Id\",\"columnValue\": \"4\"}]}],\"limit\": 3,\"offset\": 1}";
            var expectedSql = "SELECT * FROM (SELECT [Suppliers].[Id], [Suppliers].[Name] AS [Supplier], [Countries].[Name] AS [CountryName], [Products].[Name] AS [ProductName], ROW_NUMBER() OVER (ORDER BY (SELECT 0)) AS [row_num] FROM [Suppliers] LEFT JOIN [Products] ON [Suppliers].[Id] = [Products].[SupplierId]INNER JOIN [Countries] ON [Suppliers].[CountryId] = [Countries].[Id] WHERE ([Products].[Id] IN ('1', '2', '3', '4') OR [Products].[Id] >= '4')) AS [results_wrapper] WHERE [row_num] BETWEEN 2 AND 4";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);
            
            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }
        
        [Fact]
        public void ParseJsonIntoSqlWithoutColumns()
        {
            var json = "{\"table\": \"Suppliers\",\"wheres\": [{\"operator\": \"Equals\",\"columnName\": \"Id\",\"columnValue\": \"1\"}]}";
            var expectedSql = "SELECT * FROM [Suppliers] WHERE ([Id] = '1')";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);

            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }
        
        
        [Fact]
        public void ParseJsonIntoSqlWhereBetween()
        {
            var json = "{\"table\": \"Suppliers\",\"wheres\": [{\"operator\": \"Between\",\"columnName\": \"Id\",\"columnValue\": \"1\",\"columnValueMax\": \"10\"}]}";
            var expectedSql = "SELECT * FROM [Suppliers] WHERE ([Id] BETWEEN '1' AND '10')";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);

            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }
        
        [Fact]
        public void ParseJsonIntoSqlWhereNotBetween()
        {
            var json = "{\"table\": \"Suppliers\",\"wheres\": [{\"operator\": \"NotBetween\",\"columnName\": \"Id\",\"columnValue\": \"1\",\"columnValueMax\": \"10\"}]}";
            var expectedSql = "SELECT * FROM [Suppliers] WHERE ([Id] NOT BETWEEN '1' AND '10')";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);

            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }
        
        [Fact]
        public void ParseJsonIntoSqlWhereLike()
        {
            var json = "{\"table\": \"Suppliers\",\"wheres\": [{\"operator\": \"Like\",\"columnName\": \"Name\",\"columnValue\": \"Supp%\"}]}";
            var expectedSql = "SELECT * FROM [Suppliers] WHERE (LOWER([Name]) like 'supp%')";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);

            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }
        
        [Fact]
        public void ParseJsonIntoSqlWhereNotLike()
        {
            var json = "{\"table\": \"Suppliers\",\"wheres\": [{\"operator\": \"NotLike\",\"columnName\": \"Name\",\"columnValue\": \"Supp%\"}]}";
            var expectedSql = "SELECT * FROM [Suppliers] WHERE (NOT (LOWER([Name]) like 'supp%'))";
            
            Moq.Mock<ISqlQueryBuilderParser> mock = new Moq.Mock<ISqlQueryBuilderParser>();
            mock.Setup(x => x.ParseJsonIntoSqlQuery(json)).Returns(expectedSql);

            var compiler = new SqlServerCompiler().Whitelist("in", "like");
            var queryFactory = new SimpleQueryFactory(compiler);
            SqlQueryBuilderParser sqlQueryBuilderParser = new SqlQueryBuilderParser(queryFactory);
            
            var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(json).Replace("\n", string.Empty);

            Assert.Equal(expectedSql, sqlQuery);
        }
    }
}