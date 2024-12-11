
using ProductCategoryCRUD.Models;

namespace ProductCategoryCRUD.IproductServices
{
    public interface IProductServices
    {
        public (List<ProductcatgoryViewModel> products, int totalPages) GetAllProducts(int pageNumber, int pageSize);
        public Task<Product> CreateProduct(Product product);  

        public Task<Product> UpdateProduct(Product product);  
        public Task<Product> GetProduct(int id);
        public Task<Product> DeleteById(int id);
        public Task<List<Category>> GetAllcategories();
    }
}
