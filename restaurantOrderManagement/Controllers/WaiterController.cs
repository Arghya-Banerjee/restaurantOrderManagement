using Microsoft.AspNetCore.Mvc;
using restaurantOrderManagement.Models;  // Import your model or namespace for session handling

namespace restaurantOrderManagement.Controllers
{
    public class WaiterController : Controller
    {
        // Action that handles the 'WelcomeWaiter' page
        public IActionResult WelcomeWaiter()
        {
            // Retrieve session details
            var sessionDetails = HttpContext.Session.GetObjectFromJson<UserSec>("SessionDetails");

            // Check if session data is available and the role is 'waiter'
            if (sessionDetails == null || sessionDetails.vUserRole != UserRole.waiter)
            {
                return RedirectToAction("LoginPage", "Login"); // Redirect back to LoginPage if not a waiter
            }

            // Pass user ID to the view
            ViewBag.UserId = sessionDetails.UserId;

            // Return the WelcomeWaiter view
            return View();
        }
    }
}
