namespace restaurantOrderManagement.Models
{
    public class CurrentOrdersModel
    {
        public int OpMode { get; set; }
        public long OrderID { get; set; }
        public string CreatedBy { get; set; }
        public int TableNumber { get; set; }
        public int OrderStatus { get; set; }
    }
}
