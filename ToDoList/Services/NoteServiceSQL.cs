using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class NoteServiceSQL : INoteService
    {
        private readonly ApplicationSQLContext _context;
        public NoteServiceSQL(ApplicationSQLContext context)
        {
            _context = context;
        }
        public int Add(Note note)
        {
            IDbConnection connection = _context.NewConnection();
            string sql = @$"INSERT INTO Note (name, description, created, modified, deadline, statuscode) 
                            OUTPUT inserted.Id                            
                            VALUES (@name, @description, @created, @modified, @deadline, @statuscode)";
            int noteId = connection.QuerySingle<int>(sql, note);
            foreach (CategoryNote categoryNote in note.categoriesNotes)
                categoryNote.noteid = noteId;
            sql = $@"INSERT INTO categoryNote (noteid, categoryid)
                     VALUES (@noteid, @categoryid)";
            connection.Execute(sql, note.categoriesNotes);
            return noteId;
        }

        public void Delete(int id)
        {
            IDbConnection connection = _context.NewConnection();
            Note? note = Get(id);
            if (note != null)
            {
                string sql = @$"DELETE FROM Note WHERE Id = {id}";
                connection.Execute(sql);
            }
        }

        public Note? Get(int id)
        {
            IDbConnection connection = _context.NewConnection();
            string query = $@"
                SELECT n.id, n.name, n.created, n.modified, n.deadline, n.statuscode, n.description, c.id, c.name
                FROM Note n
                LEFT JOIN categoryNote cn ON n.id = cn.noteid
                LEFT JOIN category c ON cn.categoryid = c.id
                WHERE n.id = {id}
            ";

            var notes = connection.Query<Note, Category, Note>(query, (n, c) =>
            {
                if (c != null)
                {
                    n.categoriesNotes.Add(new CategoryNote
                    {
                        category = c,
                        categoryid = c.id,
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
            IDbConnection connection = _context.NewConnection();
            string query = @"
                SELECT n.id, n.name, n.created, n.modified, n.deadline, n.statuscode, n.description, c.id, c.name
                FROM Note n
                LEFT JOIN categoryNote cn ON n.id = cn.noteid
                LEFT JOIN category c ON cn.categoryid = c.id
            ";

            var notes = connection.Query<Note, Category, Note>(query, (n, c) =>
            {
                if (c != null)
                {
                    n.categoriesNotes.Add(new CategoryNote
                    {
                        category = c,
                        categoryid = c.id,
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
            IDbConnection connection = _context.NewConnection();
            Note? note = Get(id);
            if (note != null)
            {
                newNote.id = note.id;
                string sql = @"UPDATE Note
                               SET name = @name,
                                   description = @description,
                                   created = @created,
                                   modified = @modified,
                                   deadline = @deadline,
                                   statuscode = @statuscode
                               WHERE id = @id";
                connection.Execute(sql, newNote);
            }
        }
    }
}