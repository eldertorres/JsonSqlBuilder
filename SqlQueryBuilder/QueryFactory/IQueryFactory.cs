using System;
using SqlKata;
using SqlKata.Compilers;

namespace SqlQueryBuilder.QueryFactory
{
    public interface IQueryFactory : IDisposable
    {
        public Compiler Compiler { get; set; }
        public Query Query(string table);
    }
}