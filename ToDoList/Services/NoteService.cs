using Microsoft.EntityFrameworkCore;
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
            _context.notes.Add(note);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Note? note = Get(id);
            if (note != null)
            {
                _context.Remove(note);
                _context.SaveChanges();
            }
        }

        public Note? Get(int id)
        {
            return _context.notes.FirstOrDefault(n => n.id == id);
        }

        public async Task<IEnumerable<Note>> GetAll()
        {
            return await _context.notes.Include(n => n.categoriesNotes).ThenInclude(cn => cn.category).ToListAsync();
        }

        public void Update(Note newNote, int id)
        {
            Note? note = Get(id);
            if (note != null)
            {
                note = newNote;
                _context.SaveChanges();
            }
        }
    }
}
