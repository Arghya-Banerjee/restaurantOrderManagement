using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class SqlDataHelper
    {
        #region [Variable Declaration]
        private int DataCmdTimeout = 0;
        private SqlCommand cmd = new SqlCommand();
        public SqlConnection DataConn = new SqlConnection();
        #endregion [Variable Declaration]

        #region [Constructor]
        public SqlDataHelper() { }
        #endregion [Constructor]

        #region [Public Methods]
        /*public int ExecuteNonQuery(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            try
            {
                PrepareCommand(strConn, strProc, cmdType, arrPrm);
                int i = cmd.ExecuteNonQuery();
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                return i;
            }
            catch (Exception ex)
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                throw ex;
            }
        }*/

        /*public string ExecuteNonQueryReturnOutput(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            try
            {
                PrepareCommand(strConn, strProc, cmdType, arrPrm);
                string outputVal = arrPrm.SingleOrDefault(x => x.Direction == ParameterDirection.Output).ParameterName;
                cmd.ExecuteNonQuery();
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                return Convert.ToString(cmd.Parameters[outputVal].Value);
            }
            catch (Exception ex)
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                throw ex;
            }
        }*/

        /*public DataSet ExecuteDataSet(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            try
            {
                PrepareCommand(strConn, strProc, cmdType, arrPrm);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                return ds;
            }
            catch (Exception ex)
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                throw ex;
            }
        }*/

        public object ExecuteReader(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            try
            {
                PrepareCommand(strConn, strProc, cmdType, arrPrm);
                object o = cmd.ExecuteReader();
                return o;
            }
            catch (Exception ex)
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                throw ex;
            }
        }

        public DataSet ExecuteDataSet(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = DataCmdTimeout;
                    cmd.CommandText = strProc;
                    cmd.CommandType = cmdType;
                    int i = arrPrm.GetUpperBound(0);
                    int j = 0;
                    cmd.Parameters.Clear();
                    foreach (SqlParameter p in arrPrm)
                    {
                        if (j <= i)
                        {
                            cmd.Parameters.Add(p);
                        }
                        j++;
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                        cn.Dispose();
                    }
                }
            }
            return ds;
        }

        public int ExecuteNonQuery(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            int ix = default(int);
            using (SqlConnection cn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = DataCmdTimeout;
                    cmd.CommandText = strProc;
                    cmd.CommandType = cmdType;
                    int i = arrPrm.GetUpperBound(0);
                    int j = 0;
                    cmd.Parameters.Clear();
                    foreach (SqlParameter p in arrPrm)
                    {
                        if (j <= i)
                        {
                            cmd.Parameters.Add(p);
                        }
                        j++;
                    }
                    ix = cmd.ExecuteNonQuery();
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                        cn.Dispose();
                    }
                }
            }
            return ix;
        }

        public string ExecuteNonQueryReturnOutput(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            string outputVal = string.Empty;
            string returnValue = string.Empty;
            using (SqlConnection cn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = DataCmdTimeout;
                    cmd.CommandText = strProc;
                    cmd.CommandType = cmdType;
                    int i = arrPrm.GetUpperBound(0);
                    int j = 0;
                    cmd.Parameters.Clear();
                    foreach (SqlParameter p in arrPrm)
                    {
                        if (j <= i)
                        {
                            cmd.Parameters.Add(p);
                        }
                        j++;
                    }
                    outputVal = arrPrm.SingleOrDefault(x => x.Direction == ParameterDirection.Output).ParameterName;
                    cmd.ExecuteNonQuery();
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                        cn.Dispose();
                    }
                    returnValue = Convert.ToString(cmd.Parameters[outputVal].Value);
                }
            }
            return returnValue;
        }
        #endregion [Public Methods]

        #region [Private Methods]
        private void PrepareCommand(string strConn, string strProc, CommandType cmdType, int Opmode)
        {
            try
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                }
                DataConn.ConnectionString = strConn;
                DataConn.Open();
                cmd.Connection = DataConn;
                cmd.CommandTimeout = DataCmdTimeout;
                cmd.CommandText = strProc;
                cmd.CommandType = cmdType;
                cmd.Parameters.AddWithValue("@OPMODE", Opmode);
                return;
            }
            catch (Exception ex)
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                throw ex;
            }
        }

        private void PrepareCommand(string strConn, string strProc, CommandType cmdType, SqlParameter[] arrPrm)
        {
            try
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                }
                DataConn.ConnectionString = strConn;
                DataConn.Open();
                cmd.Connection = DataConn;
                cmd.CommandTimeout = DataCmdTimeout;
                cmd.CommandText = strProc;
                cmd.CommandType = cmdType;
                int i = arrPrm.GetUpperBound(0);
                int j = 0;
                cmd.Parameters.Clear();
                foreach (SqlParameter p in arrPrm)
                {
                    if (j <= i)
                    {
                        cmd.Parameters.Add(p);
                    }
                    j++;
                }
                return;
            }
            catch (Exception ex)
            {
                if (DataConn.State == ConnectionState.Open)
                {
                    DataConn.Close();
                    DataConn.Dispose();
                }
                throw ex;
            }
        }
        #endregion [Private Methods]
    }
}
