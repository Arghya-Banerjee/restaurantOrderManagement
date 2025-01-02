using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;
using restaurantOrderManagement.ViewModels;

namespace restaurantOrderManagement.Controllers
{
    [Route("[controller]")]
    public class MenuController : Controller
    {
        [Route("ShowMenu")]
        public IActionResult ShowMenu()
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

        [Route("AddMenu")]
        public IActionResult AddMenu()
        {
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.Waiter)
            {
                return RedirectToAction("LoginPage", "Login");
            }

            ViewBag.userId = sessionDetails.UserID;

            return View();
        }

        [HttpPost]
        [Route("AddMenu")]
        public IActionResult AddMenu(MenuModel menu)
        {
            menu.Opmode = 3;
            UserSec userSession = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");
            string userid = userSession.UserID;
            userid = StringUtility.toTitleCase(userid);
            menu.createdby = userid;

            int res = DBOperations<MenuModel>.DMLOperation(menu, Constant.usp_Menu);

            return RedirectToAction("WelcomeWaiter", "Waiter");
        }
    }
}
