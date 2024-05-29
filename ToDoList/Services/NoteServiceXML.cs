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
        public void Add(Note note)
        {
            var max = _context.Connection.Root?
                .Elements("note")
                .Select(elem => (int)elem.Element("id")!)
                .DefaultIfEmpty(-1)
                .Max();

            var noteElement = new XElement("note",
                new XElement("id", max + 1),
                new XElement("name", note.name),
                new XElement("created", note.created),
                new XElement("modified", note.modified),
                new XElement("deadline", note.deadline),
                new XElement("statuscode", note.statuscode),
                new XElement("description", note.description),
                new XElement("categories",
                    note.categoriesNotes.Select(cn => new XElement("categoryid", cn.categoryid))
                )
            );

            _context.Connection.Root?.Add(noteElement);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var element = _context.Connection.Root?
                .Elements("note")
                .FirstOrDefault(e => (int)e.Element("id")! == id);

            if (element != null)
            {
                element.Remove();
                _context.SaveChanges();
            }
        }
        public Note? Get(int id)
        {
            var element = _context.Connection.Root!.Elements("note")
                .FirstOrDefault(n => (int)n.Element("id")! == id);

            Dictionary<int, Category> categories = _context.Connection.Root.Elements("category")
                .ToDictionary(
                    c => (int)c.Element("id")!,
                    c => new Category
                    {
                        id = (int)c.Element("id")!,
                        name = (string)c.Element("name")!
                    }
                );

            Note note = new Note
            {
                id = (int)element!.Element("id")!,
                name = (string)element.Element("name")!,
                created = (DateTime)element.Element("created")!,
                modified = (DateTime)element.Element("modified")!,
                deadline = element.Element("deadline")?.Value != string.Empty ? (DateTime?)element.Element("deadline") : null,
                statuscode = (int)element.Element("statuscode")!,
                description = (string)element.Element("description")!,
                categoriesNotes = new List<CategoryNote>()
            };

            var elem = element.Element("categories");
            if (elem != null)
            {
                List<int> categoryids = elem.Elements("categoryid")
                    .Select(c => (int)c)
                    .ToList();

                List<int> valid = categoryids.Where(categories.ContainsKey).ToList();

                elem.Elements("categoryid")
                    .Where(c => !valid.Contains((int)c))
                    .Remove();

                foreach (var index in valid)
                {
                    Category category = categories[index];
                    note.categoriesNotes.Add(new CategoryNote
                    {
                        categoryid = category.id,
                        category = category,
                        noteid = note.id,
                    });
                }
            }

            return note;
        }
        public IEnumerable<Note> GetAll()
        {
            List<Note> notes = new List<Note>();
            foreach (var note in _context.Connection.Root?.Elements("note")!)
                notes.Add(Get((int)note.Element("id")!)!);
            return notes;
        }
        public void Update(Note newNote, int id)
        {
            Note? note = Get(id);
            if (note != null)
            {
                _context.Connection.Root?
                    .Elements("note")
                    .FirstOrDefault(n => (int)n.Element("id")! == id)?
                    .ReplaceWith(new XElement("note",
                        new XElement("id", newNote.id),
                        new XElement("name", newNote.name),
                        new XElement("created", newNote.created),
                        new XElement("modified", newNote.modified),
                        new XElement("deadline", newNote.deadline),
                        new XElement("statuscode", newNote.statuscode),
                        new XElement("description", newNote.description),
                        new XElement("categories",
                            note.categoriesNotes.Select(cn =>
                                new XElement("category", new XElement("id", cn.categoryid))
                            )
                        )
                    ));

                _context.SaveChanges();
            }
        }
    }
}
