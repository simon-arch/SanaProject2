using Dapper.Contrib.Extensions;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class CategoryServiceSQL : ICategoryService
    {
        private readonly ApplicationSQLContext _context;
        public CategoryServiceSQL(ApplicationSQLContext context)
        {
            _context = context;
        }
        public void Add(Category category)
        {
            _context.Connection.Insert(category);
        }

        public void Delete(int id)
        {
            Category? category = Get(id);
            if (category != null)
            {
                _context.Connection.Delete(category);
            }
        }

        public Category? Get(int id)
        {
            return _context.Connection.Get<Category>(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Connection.GetAll<Category>();
        }
    }
}
