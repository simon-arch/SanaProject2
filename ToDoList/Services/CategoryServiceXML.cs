using System.Reflection;
using System.Xml.Linq;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class CategoryServiceXML : ICategoryService
    {
        private readonly ApplicationXMLContext _context;
        public CategoryServiceXML(ApplicationXMLContext context)
        {
            _context = context;
        }
        public void Add(Category category)
        {
            var max = _context.Connection.Root?
                .Elements("category")
                .Select(elem => (int)elem.Element("id")!)
                .DefaultIfEmpty(-1)
                .Max();

            XElement element = new XElement("category",
                new XElement("id", max + 1)
            );

            foreach (PropertyInfo property in typeof(Category).GetProperties())
            {
                if (property.Name != nameof(Category.id))
                {
                    element.Add(new XElement(property.Name.ToLower(), property.GetValue(category)));
                }
            }

            _context.Connection.Root?.AddFirst(element);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var element = _context.Connection.Root?
                .Elements("category")
                .FirstOrDefault(elem => (int)elem.Element("id")! == id);
            if (element != null)
            {
                element.Remove();
                _context.SaveChanges();
            }
        }
        public Category? Get(int id)
        {
            var element = _context.Connection.Root?
                .Elements("category")
                .FirstOrDefault(e => (int)e.Element("id")! == id);

            if (element != null)
            {
                Category category = new Category();
                foreach (PropertyInfo property in typeof(Category).GetProperties())
                {
                    property.SetValue(category, 
                        Convert.ChangeType(element.Element(property.Name)!.Value, 
                        property.PropertyType));
                }
                return category;
            }
            return null;
        }
        public IEnumerable<Category> GetAll()
        {
            List<Category> categories = new List<Category>();
            foreach (var note in _context.Connection.Root?.Elements("category")!)
                categories.Add(Get((int)note.Element("id")!)!);
            return categories;
        }
    }
}
