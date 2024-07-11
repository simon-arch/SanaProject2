using Microsoft.Data.SqlClient;
using System.Data;
namespace ToDoList.Data
{
    public class ApplicationSQLContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public SqlConnection Connection { get; }
        public ApplicationSQLContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SQLConnection")!;
            Connection = new SqlConnection(_connectionString);
        }
        public IDbConnection NewConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
