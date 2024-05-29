using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private static int _serviceIndex;
        private readonly IEnumerable<INoteService> _noteService;
        private readonly IEnumerable<ICategoryService> _categoryService;
        public HomeController(IEnumerable<INoteService> noteService, IEnumerable<ICategoryService> categoryService)
        {
            _noteService = noteService;
            _categoryService = categoryService;
        }
        public IActionResult ChangeService(string dbSelect)
        {
            _serviceIndex = int.Parse(dbSelect);
            Console.WriteLine(dbSelect);
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var notes = _noteService.ElementAt(_serviceIndex).GetAll();
            var categories = _categoryService.ElementAt(_serviceIndex).GetAll();
            var model = new DataViewModel()
            {
                Notes = notes.Reverse().OrderBy(note => note.statuscode),
                Categories = categories,
            };
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SaveRecord(Note note, int[] categories) 
        {
            if (note.name != string.Empty)
            {
                note.statuscode = 0;
                note.created = DateTime.Now;
                note.modified = DateTime.Now;
                if (!categories.IsNullOrEmpty())
                {
                    foreach (int categoryId in categories)
                    {
                        CategoryNote categoryNote = new CategoryNote();
                        categoryNote.noteid = note.id;
                        categoryNote.categoryid = categoryId;
                        note.categoriesNotes.Add(categoryNote);
                    }
                }
                _noteService.ElementAt(_serviceIndex).Add(note);
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteRecord(int id)
        {
            _noteService.ElementAt(_serviceIndex).Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult CreateCategory(Category _category)
        {
            Category category = new Category();
            category.name = _category.name;
            if(category.name != string.Empty)
                _categoryService.ElementAt(_serviceIndex).Add(category);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.ElementAt(_serviceIndex).Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult UpdateStatus(int id)
        {
            Note? note = _noteService.ElementAt(_serviceIndex).Get(id);
            if (note != null)
            {
                note.statuscode = (note.statuscode == 0) ? 1 : 0; 
                note.modified = DateTime.Now;
                _noteService.ElementAt(_serviceIndex).Update(note, id);
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}