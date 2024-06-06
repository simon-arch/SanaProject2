using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.GraphQL
{
    [GraphRoute("notes")]
    public class NotesController : GraphController
    {
        private ServiceFactory _serviceFactory;
        public NotesController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        [Query("getAll")]
        public IEnumerable<Note> GetAll()
        {
            return _serviceFactory.GetNoteService()!.GetAll();
        }
        [Query("get")]
        public Note Get(int id)
        {
            return _serviceFactory.GetNoteService()!.Get(id)!;
        }
        [Mutation("add")]
        public Note Add(string name, string description = "", List<int> categoryIds = null!, DateTime? deadline = null)
        {
            categoryIds = categoryIds ?? new List<int>();
            List<CategoryNote> categoriesNotes = new List<CategoryNote>();
            foreach (int categoryId in categoryIds)
            {
                Category category = _serviceFactory.GetCategoryService()!.Get(categoryId)!;
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
                name = name,
                description = description,
                created = DateTime.Now,
                modified = DateTime.Now,
                deadline = deadline,
                categoriesNotes = categoriesNotes
            };
            _serviceFactory.GetNoteService()!.Add(note);
            return note;
        }
        [Mutation("delete")]
        public Note Delete(int id)
        {
            Note note = _serviceFactory.GetNoteService()!.Get(id)!;
            _serviceFactory.GetNoteService()!.Delete(id);
            return note;
        }
        [Mutation("update")]
        public Note Update(int id)
        {
            Note note = _serviceFactory.GetNoteService()!.Get(id)!;
            if (note == null || note.statuscode == 1)
                return null!;
            note.statuscode = 1;
            note.modified = DateTime.Now;
            _serviceFactory.GetNoteService()!.Update(note, id);
            return note;
        }
    }
}
