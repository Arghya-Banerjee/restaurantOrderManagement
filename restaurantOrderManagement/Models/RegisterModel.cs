namespace restaurantOrderManagement.Models
{
    public class RegisterModel
    {
        public int OpMode { get; set; }
        public long Id { get; set; }
        public string UserID { get; set; }
        public string PassCode { get; set; }
        public string Email { get; set; }
        public long MobileNumber { get; set; }
        public int IsActive { get; set; }
        public int UserType { get; set; }
    }
}
