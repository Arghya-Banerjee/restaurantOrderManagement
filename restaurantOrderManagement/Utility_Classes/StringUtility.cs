using System.Globalization;

namespace restaurantOrderManagement.Utility_Classes
{
    public class StringUtility
    {
        public static string toTitleCase(string str)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            string res = ti.ToTitleCase(str);
            return res;
        }
    }
}
