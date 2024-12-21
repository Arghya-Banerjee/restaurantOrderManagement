namespace restaurantOrderManagement.Models
{
    public class BillItemsModel
    {
        public int OpMode { get; set; }
        public int TableNumber { get; set; }
        public string foodname { get; set; }
        public decimal foodprice { get; set; }
        public decimal Quantity { get; set; }
    }
}
