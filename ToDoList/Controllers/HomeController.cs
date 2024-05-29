using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly INoteService _noteService;
        private readonly ICategoryService _categoryService;
        public HomeController(INoteService noteService, ICategoryService categoryService)
        {
            _noteService = noteService;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var notes = _noteService.GetAll();
            var categories = _categoryService.GetAll();
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
                _noteService.Add(note);
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteRecord(int id)
        {
            _noteService.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult CreateCategory(Category _category)
        {
            Category category = new Category();
            category.name = _category.name;
            if(category.name != string.Empty)
                _categoryService.Add(category);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult UpdateStatus(int id)
        {
            Note? note = _noteService.Get(id);
            if (note != null)
            {
                note.statuscode = (note.statuscode == 0) ? 1 : 0; 
                note.modified = DateTime.Now;
                _noteService.Update(note, id);
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