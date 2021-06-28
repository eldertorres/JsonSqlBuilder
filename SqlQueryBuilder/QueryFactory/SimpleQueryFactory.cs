using System;
using SqlKata;
using SqlKata.Compilers;

namespace SqlQueryBuilder.QueryFactory
{
    public class SimpleQueryFactory : IQueryFactory
    {
        public Compiler Compiler { get; set; }
        
        public SimpleQueryFactory(Compiler compiler)
        {
            Compiler = compiler;
        }

        public Query Query(string table)
        {
            return new Query(table);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}