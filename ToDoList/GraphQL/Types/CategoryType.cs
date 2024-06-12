using GraphQL.Types;
using ToDoList.Models;

namespace ToDoList.GraphQL
{
    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType()
        {
            Field(x => x.id);
            Field(x => x.name);
        }
    }
}
