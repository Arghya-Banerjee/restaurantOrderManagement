using restaurantOrderManagement.Models;

namespace restaurantOrderManagement.ViewModels
{
    public class BillViewModel
    {
        public long InvoiceID { get; set; }
        public long OrderID { get; set; }
        public int TableNumber { get; set; }
        public List<BillItemsModel> BillItems { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal AmountExcludingGST { get; set; }
        public decimal AmountIncludingGST { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
