using System.ComponentModel.DataAnnotations;

namespace restaurantOrderManagement.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public int OpMode { get; set; }
        public string UserID { get; set; }
        public string? Email { get; set; }
        public long? MobileNumber { get; set; }
        public int UserType { get; set; }
    }
}
