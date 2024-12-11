using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCategoryCRUD.IproductServices;
using ProductCategoryCRUD.Models;

namespace ProductCategoryCRUD.Controllers
{
    public class ProductController : Controller
    {
       private readonly IProductServices  iproductServices;

        public ProductController(IProductServices _iproductServices)
        {
            iproductServices = _iproductServices;
        }
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var (products, totalPages) = iproductServices.GetAllProducts(pageNumber, pageSize);

            // Pass pagination data to the view
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await iproductServices.GetAllcategories();  // For dropdown list
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await iproductServices.CreateProduct(product);
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index");
            }

            ViewBag.Categories =await iproductServices.GetAllcategories();
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product =await iproductServices.GetProduct( id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await iproductServices.GetAllcategories();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteById(int id)
        {
            
            if (id != null)
            {
                await iproductServices.DeleteById(id);

                TempData["Success"] = "Product deleted successfully";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await iproductServices.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await iproductServices.GetAllcategories();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            Product existingProduct =await iproductServices.UpdateProduct(product);
            if (existingProduct == null)
            {
                return NotFound();
            }

            
            TempData["Success"] = "Product updated successfully";
            return RedirectToAction("Index");
        }
    }

}
