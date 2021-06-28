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

            var queryBuilderMenu = new ConsoleMenu(args, 0)
                .Add("Parse JSON file", () => ParseJsonFile(serviceCollection))
                .Add("Parse JSON inline", () => ParseJsonInline(serviceCollection))
                .Add("Exit", () => Environment.Exit(0))
                .Configure(config =>
                    {
                        config.Title = "Json to Sql Query Builder";
                        config.EnableWriteTitle = true;
                    }
                );
            
            queryBuilderMenu.Show();
        }

        private static void ParseJsonFile(IServiceProvider serviceProvider)
        {
            var sqlQueryParser = serviceProvider.GetService<ISqlQueryBuilderParser>();
            if (sqlQueryParser == null)
            {
                Console.WriteLine("Sql parser not ready");
                return;
            }
            
            Console.WriteLine("Json file path: ");
            var jsonFilePath = Console.ReadLine();
            
            try
            {
                if (!File.Exists(jsonFilePath))
                    throw new IOException("File not found");
                
                var jsonFile = File.ReadAllText(jsonFilePath);
                var sqlQuery = sqlQueryParser.ParseJsonIntoSqlQuery(jsonFile);
                
                Console.WriteLine($"Parsed query:\n{sqlQuery}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
        
        private static void ParseJsonInline(IServiceProvider serviceProvider)
        {
            var sqlQueryParser = serviceProvider.GetService<ISqlQueryBuilderParser>();
            if (sqlQueryParser == null)
            {
                Console.WriteLine("Sql parser not ready");
                return;
            }
            
            Console.WriteLine("Write raw Json: ");
            var jsonData = Console.ReadLine();
            
            try
            {
                var sqlQuery = sqlQueryParser.ParseJsonIntoSqlQuery(jsonData);
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