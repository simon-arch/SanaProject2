using GraphQL.Types;
using ToDoList.GraphQL.Queries;

namespace ToDoList.GraphQL
{
    public class GraphQueries : ObjectGraphType
    {
        public GraphQueries()
        {
            Name = "Query";
            Field<NoteQueries>("notes_Q").Resolve(context => new { });
            Field<CategoryQueries>("categories_Q").Resolve(context => new { });
        }
    }
}
