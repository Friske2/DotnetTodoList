using Microsoft.Data.SqlClient;
using Dapper;
namespace Todolist.Data;

public class DataContextDapper
{
    private readonly string _connectionString;
    
    public DataContextDapper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
    
    public IEnumerable<T> Query<T>(string sql, object? param = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<T>(sql, param);
    }

    public T? QuerySingle<T>(string sql, object? param = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.QuerySingleOrDefault<T>(sql, param);
    }

    public bool Execute(string sql, object? param = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Execute(sql, param) > 0;
    }

    public int ExecuteWithRowCount(string sql, object? param = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Execute(sql, param);
    }
}