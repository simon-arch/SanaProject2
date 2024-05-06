using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INoteService _noteService;
        private readonly ICategoryService _categoryService;
        public HomeController(ILogger<HomeController> logger, INoteService noteService, ICategoryService categoryService)
        {
            _logger = logger;
            _noteService = noteService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var notes = await _noteService.GetAll();
            ViewBag.notes = notes.Reverse();
            ViewBag.categories = await _categoryService.GetAll();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SaveRecord() 
        {
            Note note = new Note();
            note.name = HttpContext.Request.Form["name"].ToString();
            if (note.name != string.Empty)
            {
                note.description = HttpContext.Request.Form["description"].ToString();
                note.created = DateTime.Now;
                note.modified = DateTime.Now;
                string deadline = HttpContext.Request.Form["deadline"].ToString();
                if (!deadline.IsNullOrEmpty())
                    note.deadline = DateTime.Parse(deadline);
                note.statuscode = 0;
                string categories = HttpContext.Request.Form["categories"].ToString();
                if (!categories.IsNullOrEmpty())
                {
                    string[] idArr = categories.Split(",");
                    foreach (string id in idArr)
                    {
                        CategoryNote categoryNote = new CategoryNote();
                        categoryNote.noteid = note.id;
                        categoryNote.categoryid = int.Parse(id);
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
        public IActionResult CreateCategory()
        {
            Category category = new Category();
            category.name = HttpContext.Request.Form["name"].ToString();
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