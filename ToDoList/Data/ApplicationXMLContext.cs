using System.Xml;
using System.Xml.Linq;
namespace ToDoList.Data
{
    public class ApplicationXMLContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public XDocument Connection { get; }
        public ApplicationXMLContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("XMLConnection")!;
            Connection = XDocument.Load(_connectionString);
        }
        public void SaveChanges()
        {
            Connection.Save(_connectionString);
        }
    }
}
