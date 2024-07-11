using GraphQL;
using GraphQL.Types;
using ToDoList.Models;
using ToDoList.Services;
using static ToDoList.Services.ServiceFactory;

namespace ToDoList.GraphQL.Mutations
{
    public class NoteMutations : ObjectGraphType
    {
        public NoteMutations(ServiceFactory serviceFactory, IHttpContextAccessor accessor)
        {
            Name = "Notes_M";
            Field<NoteType>("Add")
                .Arguments(new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "name" },
                    new QueryArgument<StringGraphType> { Name = "description", DefaultValue = "" },
                    new QueryArgument<ListGraphType<IntGraphType>> { Name = "categoryIds", DefaultValue = new List<int>() },
                    new QueryArgument<DateTimeGraphType> { Name = "deadline", DefaultValue = null }
                    ))
                .Resolve(context => {
                    List<CategoryNote> categoriesNotes = new List<CategoryNote>();
                    foreach (int categoryId in context.GetArgument<List<int>>("categoryIds"))
                    {
                        Category category = serviceFactory.GetCategoryService(GetServiceID(accessor))!.Get(categoryId)!;
                        if (category == null) continue;

                        CategoryNote categoryNote = new CategoryNote()
                        {
                            category = category,
                            categoryid = categoryId,
                        };
                        categoriesNotes.Add(categoryNote);
                    };

                    Note note = new Note()
                    {
                        id = 0,
                        name = context.GetArgument<string>("name"),
                        description = context.GetArgument<string>("description"),
                        created = DateTime.Now,
                        modified = DateTime.Now,
                        deadline = context.GetArgument<DateTime?>("deadline"),
                        categoriesNotes = categoriesNotes
                    };
                    note.id = serviceFactory.GetNoteService(GetServiceID(accessor))!.Add(note);
                    return note;
            });
            Field<NoteType>("Delete")
                .Arguments(new QueryArgument<IntGraphType> { Name = "id" })
                .Resolve(context => {
                Note note = serviceFactory.GetNoteService(GetServiceID(accessor))!.Get(context.GetArgument<int>("id"))!;
                    serviceFactory.GetNoteService(GetServiceID(accessor))!.Delete(context.GetArgument<int>("id"));
                    return note;
            });
            Field<NoteType>("Update")
                .Arguments(new QueryArgument<IntGraphType> { Name = "id" })
                .Resolve(context => {
                    int id = context.GetArgument<int>("id");
                    Note note = serviceFactory.GetNoteService(GetServiceID(accessor))!.Get(id)!;
                    if (note == null || note.statuscode == 1)
                        return null!;
                note.statuscode = 1;
                    note.modified = DateTime.Now;
                    serviceFactory.GetNoteService(GetServiceID(accessor))!.Update(note, id);
                    return serviceFactory.GetNoteService(GetServiceID(accessor))!.Get(id)!;
            });
        }
        private int GetServiceID(IHttpContextAccessor accessor)
        {
            return Enum.TryParse(accessor.HttpContext!.Request.Headers["CurrentDatabase"], out CurrentDatabase service) ? (int)service : 0;
        }
    }
}
