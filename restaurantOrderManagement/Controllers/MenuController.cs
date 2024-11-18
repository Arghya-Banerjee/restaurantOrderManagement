using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;

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

            return View(list);
        }
    }
}
