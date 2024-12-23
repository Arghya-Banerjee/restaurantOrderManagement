namespace restaurantOrderManagement.Models
{
    public class RatingsModel
    {
        public int OpMode { get; set; }
        public long RatingID { get; set; }
        public string UserID { get; set; }
        public int FoodRating { get; set; }
        public int WaiterRating { get; set; }
        public int RestaurantRating { get; set; }
    }
}
