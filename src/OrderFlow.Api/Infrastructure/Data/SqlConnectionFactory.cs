using Microsoft.Data.SqlClient;

namespace OrderFlow.Api.Infrastructure.Data;

public class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
             ?? throw new InvalidOperationException("Connction string 'DefaulConnection' não encontrada.");
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}