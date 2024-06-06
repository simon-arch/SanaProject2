using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.GraphQL
{
    [GraphRoute("categories")]
    public class CategoriesController : GraphController
    {
        private ServiceFactory _serviceFactory;
        public CategoriesController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        [Query("getAll")]
        public IEnumerable<Category> GetAll()
        {
            return _serviceFactory.GetCategoryService()!.GetAll();
        }
        [Query("get")]
        public Category Get(int id)
        {
            return _serviceFactory.GetCategoryService()!.Get(id)!;
        }
        [Mutation("add")]
        public Category Add(string name)
        {
            Category category = new Category() { name = name };
            _serviceFactory.GetCategoryService()!.Add(category);
            return category;
        }
        [Mutation("delete")]
        public Category Delete(int id)
        {
            Category category = _serviceFactory.GetCategoryService()!.Get(id)!;
            _serviceFactory.GetCategoryService()!.Delete(id);
            return category;
        }
    }
}
