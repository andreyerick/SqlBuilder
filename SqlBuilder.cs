using Microsoft.Data.SqlClient;

public class SqlBuilder
{
    public string Sql => GetSql();
    public SqlParameter[] SqlParameters => GetSqlParameters();
     
    protected string _template;
    protected List<SqlParameter> _parameters;

    protected List<string> _where;
    protected List<string> _join;
    protected List<string> _order;

    public SqlBuilder(string template)
    {
        _template = template;
        _parameters = new List<SqlParameter>();
        _where = new List<string>();
        _join = new List<string>();
        _order = new List<string>();
    }

    public void Where(string where, params SqlParameter[] parameters)
    {
        where = where.Trim();
        if (_where.Count > 0 && !where.ToUpper().StartsWith("AND ") && !where.ToUpper().StartsWith("OR "))
            where = "AND " + where;

        _where.Add(where);

        if (parameters != null)
        {
            _parameters.AddRange(parameters);
        }
    }

    public void InnerJoin(string innerJoin)
    {
        innerJoin = innerJoin.Trim();
        if (!innerJoin.ToUpper().StartsWith("INNER JOIN "))
            innerJoin = "INNER JOIN " + innerJoin;

        _join.Add(innerJoin);
    }

    public void LeftJoin(string leftJoin)
    {
        leftJoin = leftJoin.Trim();
        if (!leftJoin.ToUpper().StartsWith("LEFT JOIN "))
            leftJoin = "LEFT JOIN " + leftJoin;

        _join.Add(leftJoin);
    }

    public void RightJoin(string rightJoin)
    {
        rightJoin = rightJoin.Trim();
        if (!rightJoin.ToUpper().StartsWith("RIGHT JOIN"))
            rightJoin = "RIGHT JOIN " + rightJoin;

        _join.Add(rightJoin);
    }

    public void Order(string order)
    {
        order = order.Trim();
        _order.Add(order);
    }

    public string GetSql()
    {
        var sql = _template;

        if (_join.Count > 0)
        {
            sql = sql.Replace("%JOIN%", string.Join(" ", _join));
        }
        else
        {
            sql = sql.Replace("%JOIN%", "");
        }

        if (_where.Count > 0)
        {
            sql = sql.Replace("%WHERE%", "WHERE " + string.Join(" ", _where));
        }
        else
        {
            sql = sql.Replace("%WHERE%", "");
        }

        if (_order.Count > 0)
        {
            sql = sql.Replace("%ORDER%", "ORDER BY " + string.Join(",", _order));
        }
        else
        {
            sql = sql.Replace("%ORDER%", "");
        }

        return sql;
    }

    public SqlParameter[] GetSqlParameters()
    {
        return _parameters.ToArray();
    }
}