namespace restaurantOrderManagement.Models
{
    public class OrderDetailsModel
    {
        public int OpMode { get; set; }
        public long OrderDetailID { get; set; }
        public long OrderID { get; set; }
        public int MenuID { get; set; }
        public double Quantity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
