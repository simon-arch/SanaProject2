using GraphQL.Types;
using ToDoList.GraphQL.Mutations;

namespace ToDoList.GraphQL
{
    public class GraphMutations : ObjectGraphType
    {
        public GraphMutations()
        {
            Name = "Mutation";
            Field<NoteMutations>("notes_M").Resolve(context => new { });
            Field<CategoryMutations>("categories_M").Resolve(context => new { });
        }
    }
}
