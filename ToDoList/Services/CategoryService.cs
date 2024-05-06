using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Category category)
        {
            _context.categories.Add(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Category? category = Get(id);
            if (category != null)
            {
                _context.Remove(category);
                _context.SaveChanges();
            }
        }

        public Category? Get(int id)
        {
            return _context.categories.FirstOrDefault(c => c.id == id);
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.categories.ToListAsync();
        }

        public void Update(Category newCategory, int id)
        {
            Category? category = Get(id);
            if (category != null)
            {
                category = newCategory;
                _context.SaveChanges();
            }
        }
    }
}
