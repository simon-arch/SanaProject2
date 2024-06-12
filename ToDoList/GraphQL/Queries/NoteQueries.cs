using GraphQL;
using GraphQL.Types;
using ToDoList.Services;
using static ToDoList.Services.ServiceFactory;

namespace ToDoList.GraphQL.Queries
{
    public class NoteQueries : ObjectGraphType
    {
        public NoteQueries(ServiceFactory serviceFactory, IHttpContextAccessor accessor) 
        {
            Name = "Notes_Q";
            Field<ListGraphType<NoteType>>("GetAll").Resolve(x => serviceFactory.GetNoteService(GetServiceID(accessor))!.GetAll());
            Field<NoteType>("Get")
                .Arguments(new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }))
                .Resolve(context => serviceFactory.GetNoteService(GetServiceID(accessor))!.Get(context.GetArgument<int>("id"))!);
        }
        private int GetServiceID(IHttpContextAccessor accessor)
        {
            return Enum.TryParse(accessor.HttpContext!.Request.Headers["CurrentDatabase"], out CurrentDatabase service) ? (int)service : 0;
        }
    }
}
