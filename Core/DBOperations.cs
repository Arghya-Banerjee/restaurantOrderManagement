using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DBOperations<T> where T : new()
    {
        private static SqlDataHelper sdh = new SqlDataHelper();

        /// <summary>
        /// Executes DB SELECT statement (Capabilities to return all or paginated format based on stored procedure design)
        /// and returns query result in generic list format of supplied class.
        /// </summary>
        //
        /// <returns>
        /// List of generic class result.
        /// </returns>
        public static List<T> GetAllOrByRange(T obj, string strProcName)
        {
            List<T> result;
            try
            {
                List<T> list = new List<T>();
                SqlParameter[] parameter = Util.GetParameter(obj);
                object obj2 = sdh.ExecuteDataSet(Util.CS(), strProcName, CommandType.StoredProcedure, parameter);
                list = ListProvider<T>.FindAll((DataSet)obj2);
                result = list;
            }
            catch (Exception ex)
            {
                throw ex;
                //return new List<T>();
            }
            return result;
        }

        /// <summary>
        /// Executes DB SELECT statement (mostly based on where clause) and returns only one row as object of supplied class.
        /// </summary>
        /// <returns>
        /// Object of generic class result.
        /// </returns>
        public static T GetSpecific(T obj, string strProcName)
        {
            T result;
            try
            {
                List<T> list = new List<T>();
                SqlParameter[] parameter = Util.GetParameter(obj);
                object obj2 = sdh.ExecuteDataSet(Util.CS(), strProcName, CommandType.StoredProcedure, parameter);
                list = ListProvider<T>.FindAll((DataSet)obj2);
                result = ((list.Count > 0) ? list[0] : new T());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Executes DB DML statements (INSERT/UPDATE/DELETE).
        /// </summary>
        /// <returns>
        /// Effected rows as integer.
        /// </returns>
        public static int DMLOperation(T obj, string strProcName, DMLOperationFlag operationFlag = DMLOperationFlag.None)
        {
            try
            {
                SqlParameter[] parameter = Util.GetParameter(obj);
                return sdh.ExecuteNonQuery(Util.CS(), strProcName, CommandType.StoredProcedure, parameter);
            }
            catch (Exception ex)
            {
                throw ex;
                //return default(int);
            }
        }

        /// <summary>
        /// Executes DB INSERT statement and expects output parameter.
        /// </summary>
        /// <returns>
        /// Output parameter string.
        /// </returns>
        public static string InsertWithOutputVariable(T obj, string strProcName)
        {
            try
            {
                string returnResult = string.Empty;
                SqlParameter[] arrPrm = Util.GetParameter(obj);
                returnResult = sdh.ExecuteNonQueryReturnOutput(Util.CS(), strProcName, CommandType.StoredProcedure, arrPrm);
                return returnResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public enum DMLOperationFlag
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
}
