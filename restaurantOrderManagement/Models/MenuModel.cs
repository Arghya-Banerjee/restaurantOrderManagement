namespace restaurantOrderManagement.Models
{
    public class MenuModel
    {
        public int Opmode { get; set; }
        public int menuid { get; set; }
        public string foodname { get; set; }
        public string fooddescription { get; set; }
        public int foodcategoryid { get; set; }
        public decimal foodprice { get; set; }
        public int foodavailable { get; set; }
        public int catid { get; set; }
        public string categoryname { get; set; }
        public string createdby { get; set; }
    }
}
