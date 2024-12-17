using Microsoft.VisualBasic;

namespace restaurantOrderManagement.Models
{
    public class StockItemModel
    {
        public int OpMode { get; set; }
        public int StockItemID { get; set; }
        public int PorductID { get; set; }
        public int Quantity { get; set; }
        public DateAndTime? DateReceived { get; set; }
        public required DateAndTime ExpiryDate {  get; set; }
    }
}
