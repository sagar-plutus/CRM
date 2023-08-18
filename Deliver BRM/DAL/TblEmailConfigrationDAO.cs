using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblEmailConfigrationDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblEmailConfigration]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblEmailConfigrationTO> SelectAllDimEmailConfigration()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEmailConfigrationTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblEmailConfigrationTO SelectDimEmailConfigrationIsActive()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEmailConfigrationTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblEmailConfigrationTO> SelectAllDimEmailConfigration(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idEmailConfig", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.IdEmailConfig;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEmailConfigrationTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public static int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimEmailConfigrationTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimEmailConfigrationTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblEmailConfigrationTO dimEmailConfigrationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblEmailConfigration]( " +
            " [portNumber]" +
            " ,[isActive]" +
            " ,[emailId]" +
            " ,[userName]" +
            " ,[password]" +
            " )" +
" VALUES (" +
            " @PortNumber " +
            " ,@IsActive " +
            " ,@EmailId " +
            " ,@UserName " +
            " ,@Password " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdEmailConfig", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.IdEmailConfig;
            cmdInsert.Parameters.Add("@PortNumber", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.PortNumber;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.IsActive;
            cmdInsert.Parameters.Add("@EmailId", System.Data.SqlDbType.VarChar).Value = dimEmailConfigrationTO.EmailId;
            cmdInsert.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar).Value = dimEmailConfigrationTO.UserName;
            cmdInsert.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = dimEmailConfigrationTO.Password;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimEmailConfigrationTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimEmailConfigrationTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblEmailConfigrationTO dimEmailConfigrationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblEmailConfigration] SET " +
            "[portNumber]= @PortNumber" +
            " ,[isActive]= @IsActive" +
            " ,[emailId]= @EmailId" +
            " ,[userName]= @UserName" +
            " ,[password] = @Password" +
            " WHERE idEmailConfig = " + dimEmailConfigrationTO.IdEmailConfig;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IdEmailConfig", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.IdEmailConfig;
            cmdUpdate.Parameters.Add("@PortNumber", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.PortNumber;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.IsActive;
            cmdUpdate.Parameters.Add("@EmailId", System.Data.SqlDbType.VarChar).Value = dimEmailConfigrationTO.EmailId;
            cmdUpdate.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar).Value = dimEmailConfigrationTO.UserName;
            cmdUpdate.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = dimEmailConfigrationTO.Password;
            return cmdUpdate.ExecuteNonQuery();
        }

        public static List<TblEmailConfigrationTO> ConvertDTToList(SqlDataReader dimEmailConfigrationTODT)
        {
            List<TblEmailConfigrationTO> dimEmailConfigrationTOList = new List<TblEmailConfigrationTO>();
            if (dimEmailConfigrationTODT != null)
            {
                while (dimEmailConfigrationTODT.Read())
                {
                    TblEmailConfigrationTO dimEmailConfigrationTONew = new TblEmailConfigrationTO();
                    if (dimEmailConfigrationTODT["idEmailConfig"] != DBNull.Value)
                        dimEmailConfigrationTONew.IdEmailConfig = Convert.ToInt32(dimEmailConfigrationTODT["idEmailConfig"].ToString());
                    if (dimEmailConfigrationTODT["portNumber"] != DBNull.Value)
                        dimEmailConfigrationTONew.PortNumber = Convert.ToInt32(dimEmailConfigrationTODT["portNumber"].ToString());
                    if (dimEmailConfigrationTODT["isActive"] != DBNull.Value)
                        dimEmailConfigrationTONew.IsActive = Convert.ToInt32(dimEmailConfigrationTODT["isActive"].ToString());
                    if (dimEmailConfigrationTODT["emailId"] != DBNull.Value)
                        dimEmailConfigrationTONew.EmailId = Convert.ToString(dimEmailConfigrationTODT["emailId"].ToString());
                    if (dimEmailConfigrationTODT["userName"] != DBNull.Value)
                        dimEmailConfigrationTONew.UserName = Convert.ToString(dimEmailConfigrationTODT["userName"].ToString());
                    if (dimEmailConfigrationTODT["password"] != DBNull.Value)
                        dimEmailConfigrationTONew.Password = Convert.ToString(dimEmailConfigrationTODT["password"].ToString());
                    dimEmailConfigrationTOList.Add(dimEmailConfigrationTONew);
                }
            }
            return dimEmailConfigrationTOList;
        }
        #endregion

        #region Deletion
        public static int DeleteDimEmailConfigration(Int32 idEmailConfig)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idEmailConfig, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteDimEmailConfigration(Int32 idEmailConfig, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idEmailConfig, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idEmailConfig, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblEmailConfigration] " +
            " WHERE idEmailConfig = " + idEmailConfig + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idEmailConfig", System.Data.SqlDbType.Int).Value = dimEmailConfigrationTO.IdEmailConfig;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
