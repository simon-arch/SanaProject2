using Dapper;
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
            string sql = @"INSERT INTO Category (name) VALUES (@name)";
            _context.Connection.Execute(sql, category);
        }

        public void Delete(int id)
        {
            string sql = @$"DELETE FROM Category WHERE Id = {id}";
            Category? category = Get(id);
            if (category != null)
            {
                _context.Connection.Execute(sql);
            }
        }

        public Category? Get(int id)
        {
            string sql = @$"SELECT id, name FROM Category WHERE Id = {id}";
            return _context.Connection.QueryFirstOrDefault<Category>(sql);
        }

        public IEnumerable<Category> GetAll()
        {
            string sql = @$"SELECT id, name FROM Category";
            return _context.Connection.Query<Category>(sql);
        }
    }
}
