# SqlBuilder
Minimal secure SQL query builder

## Examples
```csharp
var builder = new SqlBuilder("SELECT ExampleEntity.* FROM ExampleEntity %JOIN% %WHERE% %ORDER%");

if (searchModel.Id != null)
{
    builder.Where($"ExampleEntity.Id=@id", new SqlParameter("@id", searchModel.Id));
}

if (searchModel.OtherId != null)
{
    builder.InnerJoin("OtherEntity ON OtherEntity.ExampleId=ExampleEntity.Id");
    builder.Where($"ExampleEntity.OtherId=@otherId", new SqlParameter("@otherId", searchModel.OtherId));
}

if (!string.IsNullOrWhiteSpace(searchModel.Query))
{
    builder.Where($"ExampleEntity.Name LIKE '%' + @name + '%'", new SqlParameter("@name", searchModel.Query));
    builder.Where($"OR ExampleEntity.Email LIKE '%' + @email + '%'", new SqlParameter("@email", searchModel.Query));
}

switch (model.Order)
{
    default:
        builder.Order("ExampleEntity.Id DESC");
        break;
}

var exampleEntities = _context.ExampleEntities.FromSqlRaw(builder.Sql, builder.SqlParameters).ToList();
```

## Documentation
To prevent SQL injection attacks, always pass parameters to the builder instead of concatenating strings, unless you are 100% sure that the string is safe.

### Constructor
`SqlBuilder(string template)`

Use `%JOIN%`, `%WHERE%`, `%ORDER%` in your template to replace with joins, where and order by clauses.
Unused clauses will be removed from the final query.

Example:
```csharp
var builder = new SqlBuilder("SELECT ExampleEntity.* FROM ExampleEntity %JOIN% %WHERE% %ORDER%");
```

### Join
Use `builder.InnerJoin(string innerJoin)`, `builder.LeftJoin(string leftJoin)` and `builder.RightJoin(string rightJoin)`

Example: 
```csharp
builder.InnerJoin("OtherEntity ON OtherEntity.ExampleId=ExampleEntity.Id")
```

### Where
Use `builder.Where(string where, params SqlParameter[] sqlParameters)`

Example:
```csharp
builder.Where("Id=@id", new SqlParameter("@id", searchModel.Id))
```

### Order
Use `builder.Order(string order)`

Example:
```csharp
builder.Order("Id DESC")
```

### Sql
Use `builder.Sql` or `builder.GetSql()` to get the final query string

### SqlParameters
Use `builder.SqlParameters` or `builder.GetSqlParameters()` to get the final query parameters

### Execute SQL
Call `DbContext.DbSet<TEntity>.FromSqlRaw()` with `builder.Sql` and `builder.SqlParameters` to execute the query

Example:
```csharp
var exampleEntities = _context.ExampleEntities.FromSqlRaw(builder.Sql, builder.SqlParameters).ToList();
```
