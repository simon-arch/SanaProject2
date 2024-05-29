using ToDoList.Models;
namespace ToDoList.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        void Add(Category category);
        void Delete(int id);
        Category? Get(int id);
    }
}
