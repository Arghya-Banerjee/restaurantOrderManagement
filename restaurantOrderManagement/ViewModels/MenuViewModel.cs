using restaurantOrderManagement.Models;

namespace restaurantOrderManagement.ViewModels
{
    public class MenuViewModel
    {
        public List<MenuModel> menuItems { get; set; }
        public List<CategoryModel> categoryItems { get; set; }
        public string categoryName { get; set; }
    }
}
