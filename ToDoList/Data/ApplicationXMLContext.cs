using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public XElement Serialize<T>(T obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(streamWriter, obj);
                    var result = XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                    result.RemoveAttributes();
                    return result;
                }
            }
        }
        public T Deserialize<T>(XElement xml) //T?
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(xml.CreateReader());
        }
    }
}
