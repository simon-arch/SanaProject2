using Dapper;
using Dapper.Contrib.Extensions;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        public NoteService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Note note)
        {
            int noteId = (int)_context.Connection.Insert(note);
            foreach(CategoryNote categoryNote in note.categoriesNotes)
                categoryNote.noteid = noteId;
            _context.Connection.Insert(note.categoriesNotes);
        }

        public void Delete(int id)
        {
            Note? note = Get(id);
            if (note != null)
                _context.Connection.Delete(note);
        }

        public Note? Get(int id)
        {
            string query = $@"
                SELECT n.id, n.name, n.created, n.modified, n.deadline, n.statuscode, n.description, c.id, c.name
                FROM Note n
                LEFT JOIN categoryNote cn ON n.id = cn.noteid
                LEFT JOIN category c ON cn.categoryid = c.id
                WHERE n.id = {id}
            ";

            var notes = _context.Connection.Query<Note, Category, Note>(query, (n, c) =>
            {
                if (c != null)
                {
                    n.categoriesNotes.Add(new CategoryNote {
                        category = c,
                        categoryid = c.id,
                        note = n,
                        noteid = n.id
                    });
                }
                return n;
            });

            Note? note = notes.FirstOrDefault();
            if (note != null)
            {
                note.categoriesNotes.Clear();
                foreach (var n in notes)
                {
                    if (n.categoriesNotes.Count > 0)
                        note.categoriesNotes.Add(n.categoriesNotes.First());
                }
            }
            return note;
        }

        public IEnumerable<Note> GetAll()
        {
            string query = @"
                SELECT n.id, n.name, n.created, n.modified, n.deadline, n.statuscode, n.description, c.id, c.name
                FROM Note n
                LEFT JOIN categoryNote cn ON n.id = cn.noteid
                LEFT JOIN category c ON cn.categoryid = c.id
            ";

            var notes = _context.Connection.Query<Note, Category, Note>(query, (n, c) =>
            {
                if (c != null)
                {
                    n.categoriesNotes.Add(new CategoryNote { 
                        category = c, 
                        categoryid = c.id, 
                        note = n, 
                        noteid = n.id
                    });
                }
                return n;
            });

            var result = notes.GroupBy(n => n.id).Select(g =>
            {
                var note = g.First();
                note.categoriesNotes = g.TakeWhile(n => n.categoriesNotes.Count > 0)
                                        .Select(p => p.categoriesNotes.First()).ToList();
                return note;
            });

            return result;
        }

        public void Update(Note newNote, int id)
        {
            Note? note = Get(id);
            if (note != null)
            {
                newNote.id = note.id;
                _context.Connection.Update(newNote);
            }
        }
    }
}
