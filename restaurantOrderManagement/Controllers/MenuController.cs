using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.ViewModels;

namespace restaurantOrderManagement.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {

            MenuModel model = new MenuModel();
            List<MenuModel> list = new List<MenuModel>();
            model.Opmode = 0;
            list = DBOperations<MenuModel>.GetAllOrByRange(model, Constant.usp_Menu);

            CategoryModel cats = new CategoryModel();
            List<CategoryModel> categories = new List<CategoryModel>();
            cats.Opmode = 1;
            categories = DBOperations<CategoryModel>.GetAllOrByRange(cats, Constant.usp_Menu);

            var viewMenuCat = new MenuViewModel
            {
                menuItems = list,
                categoryItems = categories,
                categoryName = "All"
            };

            return View(viewMenuCat);
        }

        [HttpPost]
        public ActionResult FilterMenu(string categoryName)
        {
            if(categoryName == "All")
            {
                return View("Index");
            }
            MenuModel model = new MenuModel();
            List<MenuModel> list = new List<MenuModel>();
            model.Opmode = 2;
            model.categoryname = categoryName;
            list = DBOperations<MenuModel>.GetAllOrByRange(model, Constant.usp_Menu);

            CategoryModel cats = new CategoryModel();
            List<CategoryModel> categories = new List<CategoryModel>();
            cats.Opmode = 1;
            categories = DBOperations<CategoryModel>.GetAllOrByRange(cats, Constant.usp_Menu);

            var viewMenuCat = new MenuViewModel
            {
                menuItems = list,
                categoryItems = categories,
                categoryName = categoryName
            };

            return View("Index", viewMenuCat);
        }
    }
}
