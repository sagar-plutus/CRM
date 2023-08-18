using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblSiteStatusDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSiteStatus] WHERE isActive = 1 ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllTblSiteStatus()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblSiteStatus");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [3-Oct-2017] added to select site status fro drop down
        public static List<DropDownTO> SelectSiteStatusForDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader siteStatusTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (siteStatusTODT.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (siteStatusTODT["idSiteStatus"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(siteStatusTODT["idSiteStatus"].ToString());
                    if (siteStatusTODT["siteStatusDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(siteStatusTODT["siteStatusDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectSiteStatus");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DataTable SelectTblSiteStatus(Int32 idSiteStatus)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idSiteStatus = " + idSiteStatus + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

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

        public static DataTable SelectAllTblSiteStatus(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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
        public static int InsertTblSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(ref tblSiteStatusTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblSiteStatus(ref TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(ref tblSiteStatusTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblSiteStatus");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(ref TblSiteStatusTO tblSiteStatusTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSiteStatus]( " +
            " [createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[siteStatusDisplayName]" +
            " ,[siteStatusDesc]" +
            " )" +
" VALUES (" +
            " @CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@SiteStatusDisplayName " +
            " ,@SiteStatusDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSiteStatus", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.IdSiteStatus;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblSiteStatusTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteStatusTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblSiteStatusTO.UpdatedOn);           
            cmdInsert.Parameters.Add("@SiteStatusDisplayName", System.Data.SqlDbType.NVarChar).Value = tblSiteStatusTO.SiteStatusDisplayName;
            cmdInsert.Parameters.Add("@SiteStatusDesc", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(tblSiteStatusTO.SiteStatusDesc);
            //return cmdInsert.ExecuteNonQuery();
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblSiteStatusTO.IdSiteStatus = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return -1;
        }
        #endregion

        #region Updation
        public static int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSiteStatusTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSiteStatusTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblSiteStatusTO tblSiteStatusTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSiteStatus] SET " +
            "  [idSiteStatus] = @IdSiteStatus" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[siteStatusCode]= @SiteStatusCode" +
            " ,[siteStatusDisplayName]= @SiteStatusDisplayName" +
            " ,[siteStatusDesc] = @SiteStatusDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSiteStatus", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.IdSiteStatus;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteStatusTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteStatusTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@SiteStatusCode", System.Data.SqlDbType.NVarChar).Value = tblSiteStatusTO.SiteStatusCode;
            cmdUpdate.Parameters.Add("@SiteStatusDisplayName", System.Data.SqlDbType.NVarChar).Value = tblSiteStatusTO.SiteStatusDisplayName;
            cmdUpdate.Parameters.Add("@SiteStatusDesc", System.Data.SqlDbType.NVarChar).Value = tblSiteStatusTO.SiteStatusDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblSiteStatus(Int32 idSiteStatus)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSiteStatus, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblSiteStatus(Int32 idSiteStatus, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSiteStatus, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idSiteStatus, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSiteStatus] " +
            " WHERE idSiteStatus = " + idSiteStatus + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSiteStatus", System.Data.SqlDbType.Int).Value = tblSiteStatusTO.IdSiteStatus;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
