using GraphQL.Types;
using ToDoList.Models;

namespace ToDoList.GraphQL
{
    public class NoteType : ObjectGraphType<Note>
    {
        public NoteType()
        {
            Field(x => x.id);
            Field(x => x.name);
            Field(x => x.description, nullable: true);
            Field(x => x.created);
            Field(x => x.modified);
            Field(x => x.deadline, nullable: true);
            Field(x => x.statuscode);
            Field(x => x.categoriesNotes);
        }
    }
}
