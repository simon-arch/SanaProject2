using System.Xml.Linq;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class NoteServiceXML : INoteService
    {
        private readonly ApplicationXMLContext _context;
        public NoteServiceXML(ApplicationXMLContext context)
        {
            _context = context;
        }
        public int Add(Note note)
        {
            var idElem = _context.Connection.Root!
                .Element("NextID")!
                .Element("note")!;
            note.id = int.Parse(idElem.Value);
            idElem.Value = (note.id + 1).ToString();
            AddElem(note);
            return note.id;
        }
        public void Delete(int id)
        {
            var element = _context.Connection.Root?
                .Element("Notes")!
                .Elements(nameof(Note))
                .FirstOrDefault(e => (int)e.Element("id")! == id);
            if (element != null)
            {
                element.Remove();
                _context.SaveChanges();
            }
        }
        public Note? Get(int id)
        {
            var element = _context.Connection.Root?
                .Element("Notes")!
                .Elements(nameof(Note))
                .FirstOrDefault(e => (int)e.Element("id")! == id);
            return GetElem(element!);
        }
        private Note? GetElem(XElement elem)
        {
            if (elem == null) return null;
            Note? note = null;
            var categoryIds = elem.Element("Categories")!
                .Elements()
                .Select(id => int.Parse(id.Value));
            note = _context.Deserialize<Note>(elem);
            foreach (var categoryId in categoryIds)
            {
                var categoryelem = _context.Connection.Root?
                    .Element("Categories")!
                    .Elements()
                    .FirstOrDefault(e => int.Parse(e.Element("id")!.Value) == categoryId);
                if (categoryelem != null) 
                {
                    var category = _context.Deserialize<Category>(categoryelem);
                    note.categoriesNotes.Add(new CategoryNote()
                    {
                        noteid = note.id,
                        categoryid = categoryId,
                        category = category
                    });
                }
            }
            return note;
        }
        public IEnumerable<Note> GetAll()
        {
            List<Note> notes = new List<Note>();
            foreach (XElement elem in _context.Connection.Root?
                .Element("Notes")!
                .Elements()!)
            {
                Note note = GetElem(elem)!;
                if (note != null)
                {
                    notes.Add(note);
                }
            }
            return notes;
        }
        private void AddElem(Note note)
        {
            var target = _context.Serialize(note);
            target.Add(new XElement("Categories"));
            var categories = target.Element("Categories");
            foreach (var item in note.categoriesNotes)
            {
                categories!.Add(new XElement("id", item.categoryid));
            }
            _context.Connection.Root?
                .Element("Notes")!
                .Add(target);
            _context.SaveChanges();
        }
        public void Update(Note newNote, int id)
        {
            Delete(id);
            AddElem(newNote);
        }
    }
}
