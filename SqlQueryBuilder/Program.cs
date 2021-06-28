using System;
using System.IO;
using ConsoleTools;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Compilers;
using SqlQueryBuilder.QueryFactory;
using SqlQueryBuilder.SqlQueryParser;

namespace SqlQueryBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = ManageServices();

            var sqlQueryParser = serviceCollection.GetService<ISqlQueryBuilderParser>();
            if (sqlQueryParser == null)
            {
                Console.WriteLine("Sql parser not ready");
                Environment.Exit(1);
            }
            
            var queryBuilderMenu = new ConsoleMenu(args, 0)
                .Add("Parse JSON file", () => ParseJsonFile(sqlQueryParser))
                .Add("Parse JSON inline", () => ParseJsonInline(sqlQueryParser))
                .Add("Exit", () => Environment.Exit(0))
                .Configure(config =>
                    {
                        config.Title = "Json to Sql Query Builder";
                        config.EnableWriteTitle = true;
                    }
                );
            
            queryBuilderMenu.Show();
        }

        private static void ParseJsonFile(ISqlQueryBuilderParser sqlQueryBuilderParser)
        {   
            Console.WriteLine("Json file path: ");
            var jsonFilePath = Console.ReadLine();
            
            try
            {
                if (!File.Exists(jsonFilePath))
                    throw new IOException("JSON file not found");
                
                var jsonData = File.ReadAllText(jsonFilePath);
                var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(jsonData);
                
                Console.WriteLine($"Parsed query:\n{sqlQuery}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
        
        private static void ParseJsonInline(ISqlQueryBuilderParser sqlQueryBuilderParser)
        {
            Console.WriteLine("Write raw Json: ");
            var jsonData = Console.ReadLine();
            
            try
            {
                var sqlQuery = sqlQueryBuilderParser.ParseJsonIntoSqlQuery(jsonData);
                Console.WriteLine($"Parsed query:\n{sqlQuery}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
        
        private static IServiceProvider ManageServices()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<ISqlQueryBuilderParser, SqlQueryBuilderParser>()
                .AddSingleton(new SqlServerCompiler().Whitelist("in", "like"))
                .AddSingleton<IQueryFactory, SimpleQueryFactory>()
                .BuildServiceProvider();
            
            return serviceCollection;
        }
        
        
    }
}