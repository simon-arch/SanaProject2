using GraphQL;
using GraphQL.Types;
using ToDoList.Models;
using ToDoList.Services;
using static ToDoList.Services.ServiceFactory;

namespace ToDoList.GraphQL.Mutations
{
    public class CategoryMutations : ObjectGraphType
    {
        public CategoryMutations(ServiceFactory serviceFactory, IHttpContextAccessor accessor)
        {
            Name = "Categories_M";
            Field<CategoryType>("Add")
                .Arguments(new QueryArgument<StringGraphType> { Name = "name" })
                .Resolve(context => {
                    Category category = new Category()
                    {
                        name = context.GetArgument<string>("name")
                    };
                    serviceFactory.GetCategoryService(GetServiceID(accessor))!.Add(category);
                    return category;
                });
            Field<CategoryType>("Delete")
                .Arguments(new QueryArgument<IntGraphType> { Name = "id" })
                .Resolve(context => {
                    Category category = serviceFactory.GetCategoryService(GetServiceID(accessor))!.Get(context.GetArgument<int>("id"))!;
                    serviceFactory.GetCategoryService(GetServiceID(accessor))!.Delete(context.GetArgument<int>("id"));
                    return category;
                });
        }
        private int GetServiceID(IHttpContextAccessor accessor)
        {
            return Enum.TryParse(accessor.HttpContext!.Request.Headers["CurrentDatabase"], out CurrentDatabase service) ? (int)service : 0;
        }
    }
}
