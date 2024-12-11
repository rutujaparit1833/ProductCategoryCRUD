using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;
using ProductCategoryCRUD.IproductServices;
using ProductCategoryCRUD.Models;

namespace ProductCategoryCRUD.ProductServices
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _context;

        public ProductServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _context.products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteById(int id)
        {
            var products =await _context.products.FirstOrDefaultAsync(p => p.cId == id);

            if (products != null)
            {
                _context.products.RemoveRange(products);
                _context.SaveChanges();
            }
            return products;
        }

        public (List<ProductcatgoryViewModel> products, int totalPages) GetAllProducts(int pageNumber, int pageSize)
        {
            int totalProducts = _context.products.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Ensure the page number is within valid range
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            // Fetch paginated products
            List<Product> products = _context.products
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

            // Map products to view model
            List<ProductcatgoryViewModel> productcatgorylist = products.Select(item =>
            {
                Category category = _context.categories.FirstOrDefault(a => a.Id == item.cId);

                return new ProductcatgoryViewModel
                {
                    cid = item.cId,
                    Pname = item.Name,
                    pid = item.Id,
                    cname = category?.CName ?? "Unknown"
                };
            }).ToList();

            return (productcatgorylist, totalPages);

        }

        public async Task<List<Category>> GetAllcategories()
        {
           List<Category> categories=await  _context.categories.ToListAsync();
            return categories;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _context.products.FirstOrDefaultAsync(p => p.Id == id);
            if(product!=null)
            {
                return product;
            }
            throw null;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
         Product exitproduct = await _context.products.FirstOrDefaultAsync(a=>a.Id == product.Id);
            if (exitproduct != null) {
                exitproduct.Name = product.Name;
                exitproduct.cId = product.cId;
                await _context.SaveChangesAsync();
                return exitproduct;
            }
            return null;
        }

       
    }
}
