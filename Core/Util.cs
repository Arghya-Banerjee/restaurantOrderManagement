using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration;
namespace Core
{
    public class Util
    {
        

        public static string CS()
        {
            try
            {

                string connStr = AppSettingHelper.GetSetting("ConnectionStrings", "DefaultConnection");
               
                return connStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlParameter[] GetParameter(object obj)
        {
            string name = string.Empty;
            try
            {

                Type myType = obj.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                SqlParameter[] arrSqlParameter = new SqlParameter[(props.Count)];
                int index = 0;
                foreach (PropertyInfo prop in props)
                {
                    Type propType = prop.GetType();

                    object propValue = prop.GetValue(obj, null);
                    // string name = prop.Name;

                    name = prop.Name;
                    if (prop.PropertyType == typeof(XmlDocument))
                    {
                        if (propValue != null)
                        {
                            //arrSqlParameter[index] = new SqlParameter("@" + name, SqlDbType.NText);
                            arrSqlParameter[index] = new SqlParameter("@" + name, SqlDbType.Xml);
                            arrSqlParameter[index].Value = ((XmlDocument)propValue).InnerXml;
                        }
                        else
                        {
                            //arrSqlParameter[index] = new SqlParameter("@" + name, SqlDbType.NText);
                            arrSqlParameter[index] = new SqlParameter("@" + name, SqlDbType.Xml);
                            arrSqlParameter[index].Value = null;
                        }

                    }

                    else if (prop.PropertyType != typeof(XmlDocument))
                    {
                        if (prop.GetType() == typeof(DateTime))
                        {
                            DateTime objDate = new DateTime();
                            arrSqlParameter[index] = ((DateTime)propValue != objDate) ? new SqlParameter("@" + name, propValue) : new SqlParameter("@" + name, null);
                        }
                        else
                        {
                            //arrSqlParameter[index] = new SqlParameter("@" + name, propValue);
                            bool isOutputParam = false;
                            foreach (Attribute a in prop.GetCustomAttributes(false))
                            {
                                //if (a is outputparam || a.TypeId.ToString() == "outputparam")
                                if (a is outputparam)
                                {
                                    outputparam os = (outputparam)a;
                                    if (os.ParamType == ParamType.Output)
                                    {
                                        isOutputParam = true;
                                    }
                                }
                            }
                            if (!isOutputParam)
                            {
                                arrSqlParameter[index] = new SqlParameter("@" + name, propValue);
                            }
                            else
                            {
                                int size = default(int);

                                foreach (PropertyInfo propertyInfo in prop.GetType().GetProperties())
                                {
                                    if (propertyInfo.PropertyType == typeof(int))
                                        size = 4;
                                    else if (propertyInfo.PropertyType == typeof(string))
                                        size = 2048;
                                    else if (propertyInfo.PropertyType == typeof(long))
                                        size = 8;
                                    else
                                        size = 0;
                                }

                                var outputParam = new SqlParameter("@" + name, propValue) { Direction = ParameterDirection.Output, Size = size > 0 ? size : 2048 };
                                arrSqlParameter[index] = outputParam;
                            }
                        }
                    }
                    index++;
                }

                return arrSqlParameter;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    /*public static class extntionMethod
    {
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }
    }*/

    public class DualReturnType
    {
        public DataTable dt { get; set; }
        public string sp { get; set; }
    }

    public class clsEnum
    {
        public enum PayEffect { Add = 0, Substract = 1 }

        public enum Mnth { January = 1, February = 2, March = 3, April = 4, May = 5, June = 6, July = 7, August = 8, September = 9, October = 10, November = 11, December = 12 }

        public enum ImportType { Collection = 0, Sale = 1, Outstanding = 2 }

        public enum AlertType { Default = 0, Info = 1, Primary = 2, Success = 3, Warning = 4, Danger = 5 }
    }
}
