using restaurantOrderManagement.Models;

namespace restaurantOrderManagement.ViewModels
{
    public class ProductDetailsViewModel
    {
        public List<StockProductDetailsModel> stockProductDetails { get; set; }
        public List<StockCategoryModel> stockCategory { get; set; }
    }
}
