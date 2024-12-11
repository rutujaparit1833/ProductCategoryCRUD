namespace ProductCategoryCRUD.Models
{
    public class PaginatedResult
    {
        public List<ProductcatgoryViewModel> Items { get; set; } = new List<ProductcatgoryViewModel>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
