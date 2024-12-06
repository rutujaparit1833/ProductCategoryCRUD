using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCategoryCRUD.Models;

namespace ProductCategoryCRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            int totalProducts = _context.products.Count();

            
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            List<Product> products = _context.products
                                    .Skip((pageNumber - 1) * pageSize)  
                                    .Take(pageSize) 
                                    .ToList();
            List<ProductcatgoryViewModel> productcatgorylist = new List<ProductcatgoryViewModel>();

            foreach (var item in products)
            {
                Category category=_context.categories.FirstOrDefault(a => a.Id == item.cId);
                if (category != null)
                {


                    ProductcatgoryViewModel productcatgory = new ProductcatgoryViewModel
                    {
                        cid = item.cId,
                        Pname = item.Name,
                        pid = item.Id,
                        cname = category.CName

                    };

                    productcatgorylist.Add(productcatgory);
                }
            }

            // Pass data to the view
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(productcatgorylist);
        }





        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.categories.ToList(); // For dropdown list
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = _context.categories.ToList(); 
            return View(product);
        }


        [HttpGet]
        public IActionResult GetProductById(int id)
        {
            var product = _context.products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.categories.ToList();
            return View(product);
        }

        [HttpGet]
        public IActionResult DeleteById(int id)
        {
            var product = _context.products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.products.Remove(product);
                _context.SaveChanges();
                TempData["Success"] = "Product deleted successfully";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            var product = _context.products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.categories.ToList(); 
            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            var existingProduct = _context.products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.cId = product.cId;

            _context.SaveChanges();
            TempData["Success"] = "Product updated successfully";
            return RedirectToAction("Index");
        }
    }

}
