using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;

namespace restaurantOrderManagement.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult RateUs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRating(int foodRating, int waiterRating, int restaurantRating)
        {
            RatingsModel ratings = new RatingsModel();
            ratings.OpMode = 1;
            ratings.FoodRating = foodRating;
            ratings.WaiterRating = waiterRating;
            ratings.RestaurantRating = restaurantRating;
            DBOperations<RatingsModel>.DMLOperation(ratings, Constant.usp_Rating);

            return RedirectToAction("Waiter", "CurrentOrders");
        }
    }
}
