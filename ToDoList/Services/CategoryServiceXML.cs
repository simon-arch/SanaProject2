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
            var idElem = _context.Connection.Root!
                .Element("NextID")!
                .Element("category")!;
            category.id = int.Parse(idElem.Value);
            idElem.Value = (category.id + 1).ToString();
            var target = _context.Serialize(category);
            _context.Connection.Root?
                .Element("Categories")!
                .AddFirst(target);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var element = _context.Connection.Root?
                .Element("Categories")!
                .Elements(nameof(Category))
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
                .Element("Categories")!
                .Elements(nameof(Category))
                .FirstOrDefault(e => (int)e.Element("id")! == id);
            return GetElem(element!);
        }
        private Category? GetElem(XElement elem)
        {
            Category? category = null;
            if (elem != null)
            {
                category = _context.Deserialize<Category>(elem);
            }
            return category;
        }
        public IEnumerable<Category> GetAll()
        {
            List<Category> categories = new List<Category>();
            foreach (var elem in _context.Connection.Root?
                .Element("Categories")!
                .Elements()!)
            {
                Category category = GetElem(elem)!;
                if (category != null)
                {
                    categories.Add(category);
                }
            }
            return categories;
        }
    }
}
