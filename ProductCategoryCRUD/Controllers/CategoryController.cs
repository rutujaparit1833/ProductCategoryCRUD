using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductCategoryCRUD.Models;

namespace ProductCategoryCRUD.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context; 
        }

        public IActionResult Index()
        {
            List<Category> categoryList=_context.categories.ToList();
            return View(categoryList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (category == null)
            {
                return View();  
            }
           _context.categories.Add(category);
            await _context.SaveChangesAsync();
            ViewBag.success = "Category created successfully";
            return View();
        }

        [HttpGet]
        public IActionResult GetcategoryById(int id)
        {
            Category category=_context.categories.FirstOrDefault(a => a.Id == id);

            return View(category);
        }


        [HttpGet]
        public IActionResult DeleteByid(int id)
        {
            _context.categories.RemoveRange(_context.categories.Where(a => a.Id == id));
            int isDelted = _context.SaveChanges();
            return View("Index");
        }

        [HttpGet]
        public  IActionResult UpdateCategory(int id)
        {
            Category categoryresult = _context.categories.FirstOrDefault(a => a.Id == id);
            
            return View(categoryresult);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            Category categoryresult = _context.categories.FirstOrDefault(a => a.Id == category.Id);
            categoryresult.CName = category.CName;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
