using Microsoft.Data.SqlClient;
using Dapper;
using Todolist.Exceptions;

namespace Todolist.Data;

public class DataContextDapper
{
    private readonly string _connectionString;
    
    public DataContextDapper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new DatabaseConfigurationException("Connection string 'DefaultConnection' not found in appsettings.");
    }
    
    public IEnumerable<T> Query<T>(string sql, object? param = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<T>(sql, param);
        }
        catch (SqlException ex) when (ex.Number == -2 || ex.Number == 53 || ex.Number == 40 || ex.Number == 10060)
        {
            // -2 = timeout, 53/40/10060 = server not found / connection refused
            throw new DatabaseConnectionException($"[{ex.Number}] {ex.Message}");
        }
        catch (SqlException ex)
        {
            throw new DatabaseQueryException($"[{ex.Number}] {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            throw new DatabaseConfigurationException(ex.Message);
        }
    }

    public T? QuerySingle<T>(string sql, object? param = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QuerySingleOrDefault<T>(sql, param);
        }
        catch (SqlException ex) when (ex.Number == -2 || ex.Number == 53 || ex.Number == 40 || ex.Number == 10060)
        {
            throw new DatabaseConnectionException($"[{ex.Number}] {ex.Message}");
        }
        catch (SqlException ex)
        {
            throw new DatabaseQueryException($"[{ex.Number}] {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            throw new DatabaseConfigurationException(ex.Message);
        }
    }

    public bool Execute(string sql, object? param = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Execute(sql, param) > 0;
        }
        catch (SqlException ex) when (ex.Number == -2 || ex.Number == 53 || ex.Number == 40 || ex.Number == 10060)
        {
            throw new DatabaseConnectionException($"[{ex.Number}] {ex.Message}");
        }
        catch (SqlException ex)
        {
            throw new DatabaseQueryException($"[{ex.Number}] {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            throw new DatabaseConfigurationException(ex.Message);
        }
    }

    public int ExecuteWithRowCount(string sql, object? param = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Execute(sql, param);
        }
        catch (SqlException ex) when (ex.Number == -2 || ex.Number == 53 || ex.Number == 40 || ex.Number == 10060)
        {
            throw new DatabaseConnectionException($"[{ex.Number}] {ex.Message}");
        }
        catch (SqlException ex)
        {
            throw new DatabaseQueryException($"[{ex.Number}] {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            throw new DatabaseConfigurationException(ex.Message);
        }
    }
}

