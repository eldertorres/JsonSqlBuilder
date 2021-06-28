
# JSON SQL Query Builder (JSQB)

JSQB is a C# (.NET  Core 3.1) program to build SQL Queries from JSON objects.
Currently, there is a command line interface with two options to read the JSON, via file or writing.


## Usage

Just download and build the SqlQueryBuilder Project.

Command Line Options:
```bash
>> Parse JSON File:
>> Parse JSON Inline:
```

Simple JSON example:

```json
{"table": "Products", "columns": [{ "name": "Id" }, { "name": "Name" }, { "name": "Price" }], "wheres": [ {"operator": "Equals", "columnName": "Id", "columnValue": "10" }]}
```
Result:
```sql
SELECT [Id], [Name], [Price] FROM [Products] WHERE ([Id] = '10')
```

Complete JSON example:

```json
{
    "table": "Suppliers",
    "columns": [
        {
            "name": "Suppliers.Id"
        },
        {
            "name": "Suppliers.Name as Supplier"
        },
        {
            "name": "Countries.Name as CountryName"
        },
        {
            "name": "Products.Name as ProductName"
        }
    ],
    "joins": [
        {
            "joinOperator": "LeftJoin",
            "table": "Products",
            "fromColumn": "Suppliers.Id",
            "operator": "Equals",
            "toColumn": "Products.SupplierId"
        },
        {
            "joinOperator": "InnerJoin",
            "table": "Countries",
            "fromColumn": "Suppliers.CountryId",
            "operator": "Equals",
            "toColumn": "Countries.Id"
        }
    ],
    "wheres": [
        {
            "operator": "In",
            "columnName": "Products.Id",
            "columnValue": "1,2,3,4",
            "orClauses": [
                {
                    "operator": "GreaterOrEqual",
                    "columnName": "Products.Id",
                    "columnValue": "4"
                }
            ]
        },
        {
            "operator": "Like",
            "columnName": "Products.Name",
            "columnValue": "S%"
        }
    ],
    "orders": [
        {
            "orderType": "Desc",
            "columnName": "Products.Name"
        },
        {
            "orderType": "Asc",
            "columnName": "Products.Id"
        }
    ],
    "limit": 4,
    "offset": 1
}
```

Result:
```sql
SELECT  *
FROM
    (
        SELECT  [Suppliers].[Id]
                ,[Suppliers].[Name]                                                  AS [Supplier]
                ,[Countries].[Name]                                                  AS [CountryName]
                ,[Products].[Name]                                                   AS [ProductName]
                ,ROW_NUMBER() OVER (ORDER BY [Products].[Name] DESC,[Products].[Id]) AS [row_num]
        FROM [Suppliers]
            LEFT  JOIN [Products]  ON [Suppliers].[Id] = [Products].[SupplierId]
            INNER JOIN [Countries] ON [Suppliers].[CountryId] = [Countries].[Id]
        WHERE ([Products].[Id] IN ('1', '2', '3', '4') OR [Products].[Id] >= '4')
          AND (LOWER([Products].[Name]) like 's%')
    ) AS [results_wrapper]
WHERE [row_num] BETWEEN 2 AND 5 
```


### Supported Comparison Operators
```bash
Equals
GreaterThan
LessThan
GreaterOrEqual
LessOrEqual
NotEqual
Like
NotLike
In
NotIn
Between
NotBetween
```
### Supported Join Operators
```bash
InnerJoin
LeftJoin
RightJoin
FullOuterJoin
```

#

Based on [SQLKata](https://github.com/sqlkata/querybuilder) query builder and supporting the same database compilers. Used MS SQL Server for test purpose.
