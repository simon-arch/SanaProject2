using GraphQL.Types;

namespace ToDoList.GraphQL
{
    public class GraphSchema : Schema
    {
        public GraphSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<GraphQueries>();
            Mutation = serviceProvider.GetRequiredService<GraphMutations>();
        }
    }
}