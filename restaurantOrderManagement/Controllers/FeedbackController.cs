using Core;
using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;

namespace restaurantOrderManagement.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult RateUs(int orderId)
        {
            ViewBag.OrderId = orderId; // Adding orderId to pass to Submit Rating
            return View();
        }

        [HttpPost]
        [Route("Feedback/SubmitRating")]
        public IActionResult SubmitRating(int foodRating, int waiterRating, int restaurantRating, int orderId)
        {
            try
            {
                RatingsModel ratings = new RatingsModel
                {
                    OpMode = 1,
                    FoodRating = foodRating,
                    WaiterRating = waiterRating,
                    RestaurantRating = restaurantRating,
                    OrderID = orderId
                };
                DBOperations<RatingsModel>.DMLOperation(ratings, Constant.usp_Rating);

                OrderHeaderModel orderHeader = new OrderHeaderModel
                {
                    Opmode = 4,
                    OrderID = orderId,
                    CreatedOn = DateTime.Now,
                    OrderDate = DateTime.Now
                };
                int tableNumber = DBOperations<OrderHeaderModel>.GetSpecific(orderHeader, Constant.usp_OrderHeader).TableNumber;

                string xmlFilePath = $"wwwroot/cart_{tableNumber}.xml";
                try
                {
                    if (System.IO.File.Exists(xmlFilePath))
                    {
                        System.IO.File.Delete(xmlFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File deletion error: {ex.Message}");
                }

                OrderHeaderModel changeStatusDummy = new OrderHeaderModel
                {
                    TableNumber = tableNumber,
                    Opmode = 2,
                    OrderStatus = 1,
                    OrderDate = DateTime.Now,
                    CreatedOn = DateTime.Now
                };
                DBOperations<OrderHeaderModel>.DMLOperation(changeStatusDummy, Constant.usp_OrderHeader);

                System.Diagnostics.Debug.WriteLine("Redirecting to CurrentOrders...");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
