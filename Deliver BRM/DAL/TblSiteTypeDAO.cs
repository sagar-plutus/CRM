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
    public class TblSiteTypeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSiteType] WHERE isActive = 1";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblSiteTypeTO> SelectAllTblSiteTypeList()
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
                SqlDataReader siteTypeTO = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSiteTypeTO> siteTypeToList= ConvertDTToList(siteTypeTO);
                return siteTypeToList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblSiteType");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DataTable SelectTblSiteType(Int32 idSiteType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idSiteType = " + idSiteType + " ";
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

        public static DataTable SelectAllTblSiteType(SqlConnection conn, SqlTransaction tran)
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

        public static List<TblSiteTypeTO> ConvertDTToList(SqlDataReader tblSiteTypeTODT)
        {
            List<TblSiteTypeTO> tblSiteTypeTOList = new List<TblSiteTypeTO>();
            if (tblSiteTypeTODT != null)
            {
                while (tblSiteTypeTODT.Read())
                {
                    TblSiteTypeTO siteTypeTONew = new TblSiteTypeTO();
                    if (tblSiteTypeTODT["idSiteType"] != DBNull.Value)
                        siteTypeTONew.IdSiteType = Convert.ToInt32(tblSiteTypeTODT["idSiteType"].ToString());
                    if (tblSiteTypeTODT["siteTypeDisplayName"] != DBNull.Value)
                        siteTypeTONew.SiteTypeDisplayName = tblSiteTypeTODT["siteTypeDisplayName"].ToString();
                    if (tblSiteTypeTODT["parentSiteTypeId"] != DBNull.Value)
                        siteTypeTONew.ParentSiteTypeId = Convert.ToInt32(tblSiteTypeTODT["parentSiteTypeId"].ToString());
                    if (tblSiteTypeTODT["dimSiteTypeId"] != DBNull.Value)
                        siteTypeTONew.DimSiteTypeId = Convert.ToInt32(tblSiteTypeTODT["dimSiteTypeId"].ToString());
                    if (tblSiteTypeTODT["isActive"] != DBNull.Value)
                        siteTypeTONew.IsActive = Convert.ToInt16(tblSiteTypeTODT["isActive"].ToString());
                    tblSiteTypeTOList.Add(siteTypeTONew);
                }
            }
            return tblSiteTypeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblSiteType(TblSiteTypeTO tblSiteTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(ref tblSiteTypeTO, cmdInsert);
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

        public static int InsertTblSiteType(ref TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(ref tblSiteTypeTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblSiteType");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(ref TblSiteTypeTO tblSiteTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSiteType]( " +
            //"  [idSiteType]" +
            "  [parentSiteTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " ,[siteTypeDisplayName]" +
            " ,[siteTypeDesc]" +
            " ,[dimSiteTypeId]" +
            " )" +
" VALUES (" +
            //"  @IdSiteType " +
            "  @ParentSiteTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@IsActive " +
            " ,@SiteTypeDisplayName " +
            " ,@SiteTypeDesc " +
            " ,@DimSiteTypeId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSiteType", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.IdSiteType;
            cmdInsert.Parameters.Add("@ParentSiteTypeId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblSiteTypeTO.ParentSiteTypeId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteTypeTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteTypeTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteTypeTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblSiteTypeTO.IsActive;
            cmdInsert.Parameters.Add("@SiteTypeDisplayName", System.Data.SqlDbType.NVarChar).Value = tblSiteTypeTO.SiteTypeDisplayName;
            cmdInsert.Parameters.Add("@SiteTypeDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteTypeTO.SiteTypeDesc);
            cmdInsert.Parameters.Add("@DimSiteTypeId", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.DimSiteTypeId;
            //return cmdInsert.ExecuteNonQuery();

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblSiteTypeTO.IdSiteType = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return -1;
        }
        #endregion

        #region Updation
        public static int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSiteTypeTO, cmdUpdate);
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

        public static int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSiteTypeTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblSiteTypeTO tblSiteTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSiteType] SET " +
            "  [idSiteType] = @IdSiteType" +
            " ,[parentSiteTypeId]= @ParentSiteTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            " ,[siteTypeCode]= @SiteTypeCode" +
            " ,[siteTypeDisplayName]= @SiteTypeDisplayName" +
            " ,[siteTypeDesc] = @SiteTypeDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSiteType", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.IdSiteType;
            cmdUpdate.Parameters.Add("@ParentSiteTypeId", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.ParentSiteTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteTypeTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteTypeTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblSiteTypeTO.IsActive;
            //cmdUpdate.Parameters.Add("@SiteTypeCode", System.Data.SqlDbType.NVarChar).Value = tblSiteTypeTO.SiteTypeCode;
            cmdUpdate.Parameters.Add("@SiteTypeDisplayName", System.Data.SqlDbType.NVarChar).Value = tblSiteTypeTO.SiteTypeDisplayName;
            cmdUpdate.Parameters.Add("@SiteTypeDesc", System.Data.SqlDbType.NVarChar).Value = tblSiteTypeTO.SiteTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblSiteType(Int32 idSiteType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSiteType, cmdDelete);
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

        public static int DeleteTblSiteType(Int32 idSiteType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSiteType, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idSiteType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSiteType] " +
            " WHERE idSiteType = " + idSiteType + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSiteType", System.Data.SqlDbType.Int).Value = tblSiteTypeTO.IdSiteType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
