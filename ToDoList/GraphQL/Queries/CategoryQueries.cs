using GraphQL;
using GraphQL.Types;
using ToDoList.Services;
using static ToDoList.Services.ServiceFactory;

namespace ToDoList.GraphQL.Queries
{
    public class CategoryQueries : ObjectGraphType
    {
        public CategoryQueries(ServiceFactory serviceFactory, IHttpContextAccessor accessor) 
        {
            Name = "Categories_Q";
            Field<ListGraphType<CategoryType>>("GetAll").Resolve(x => serviceFactory.GetCategoryService(GetServiceID(accessor))!.GetAll());
            Field<CategoryType>("Get")
                .Arguments(new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }))
                .Resolve(context => serviceFactory.GetCategoryService(GetServiceID(accessor))!.Get(context.GetArgument<int>("id"))!);
        }
        private int GetServiceID(IHttpContextAccessor accessor)
        {
            return Enum.TryParse(accessor.HttpContext!.Request.Headers["CurrentDatabase"], out CurrentDatabase service) ? (int)service : 0;
        }
    }
}
