using Dapper;
using System.Data;
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
        public int Add(Category category)
        {
            IDbConnection connection = _context.Connection;
            string sql = @"INSERT INTO Category (name) OUTPUT Inserted.ID VALUES (@name)";
            int id = connection.ExecuteScalar<int>(sql, category);
            return id;
        }

        public void Delete(int id)
        {
            IDbConnection connection = _context.Connection;
            string sql = @$"DELETE FROM Category WHERE Id = {id}";
            Category? category = Get(id);
            if (category != null)
            {
                connection.Execute(sql);
            }
        }

        public Category? Get(int id)
        {
            IDbConnection connection = _context.Connection;
            string sql = @$"SELECT id, name FROM Category WHERE Id = {id}";
            return connection.QueryFirstOrDefault<Category>(sql);
        }

        public IEnumerable<Category> GetAll()
        {
            IDbConnection connection = _context.Connection;
            string sql = @$"SELECT id, name FROM Category";
            return connection.Query<Category>(sql);
        }
    }
}
