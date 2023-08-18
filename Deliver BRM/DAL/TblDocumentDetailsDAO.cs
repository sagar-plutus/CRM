using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.StaticStuff;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblDocumentDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT *, tblUser.userDisplayName FROM tblDocumentDetails tblDocumentDetails " +
                                 " LEFT JOIN tblUser tblUser  ON tblDocumentDetails.createdBy = tblUser.idUser ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblDocumentDetailsTO> SelectAllTblDocumentDetails(Int32 createdBy)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE createdBy = " + createdBy + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDocumentDetailsTO> list = ConvertDTToList(reader);
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

        public static TblDocumentDetailsTO SelectTblDocumentDetails(Int32 idDocument)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idDocument = " + idDocument + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDocumentDetailsTO> list = ConvertDTToList(reader);
                if(list != null && list.Count==0)
                    return list[0];
                else
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

        public static List<TblDocumentDetailsTO> SelectAllTblDocumentDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDocumentDetailsTO> list = ConvertDTToList(reader);
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


        public  static List<TblDocumentDetailsTO> SelectDocumentDetailsBasedOnFileType(Int32 FileTypeId,Int32 createdBy)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE fileTypeId = " + FileTypeId + " AND idActive=1 " +
                                      " AND tblDocumentDetails.createdBy="+createdBy;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDocumentDetailsTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
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


        public static List<TblDocumentDetailsTO> GetUploadedFileBasedOnDocumentId(string DocumentIds)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idDocument in (" + DocumentIds + ") AND idActive=1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDocumentDetailsTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
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

        #endregion

        #region Insertion
        public static int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblDocumentDetailsTO, cmdInsert);
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

        public static int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblDocumentDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblDocumentDetailsTO tblDocumentDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblDocumentDetails]( " +
            //"  [idDocument]" +
            " [moduleId]" +
            " ,[createdBy]" +
            " ,[idActive]" +
            " ,[createdOn]" +
            " ,[documentDesc]" +
            " ,[path]" +
            " ,[fileTypeId]" +
            " )" +
" VALUES (" +
            //"  @IdDocument " +
            " @ModuleId " +
            " ,@CreatedBy " +
            " ,@IdActive " +
            " ,@CreatedOn " +
            " ,@DocumentDesc " +
            " ,@Path " +
            " ,@FileTypeId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.ModuleId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@IdActive", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblDocumentDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@DocumentDesc", System.Data.SqlDbType.NVarChar).Value = tblDocumentDetailsTO.DocumentDesc;
            cmdInsert.Parameters.Add("@Path", System.Data.SqlDbType.NVarChar).Value = tblDocumentDetailsTO.Path;
            cmdInsert.Parameters.Add("@FileTypeId", System.Data.SqlDbType.NVarChar).Value = tblDocumentDetailsTO.FileTypeId;
            //return cmdInsert.ExecuteNonQuery();
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblDocumentDetailsTO.IdDocument = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblDocumentDetailsTO, cmdUpdate);
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

        public static int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblDocumentDetailsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblDocumentDetailsTO tblDocumentDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblDocumentDetails] SET " +
            //"  [idDocument] = @IdDocument" +
            " [moduleId]= @ModuleId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[idActive]= @IdActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[documentDesc]= @DocumentDesc" +
            " ,[path] = @Path" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IdDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.ModuleId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@IdActive", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblDocumentDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@DocumentDesc", System.Data.SqlDbType.NVarChar).Value = tblDocumentDetailsTO.DocumentDesc;
            cmdUpdate.Parameters.Add("@Path", System.Data.SqlDbType.NVarChar).Value = tblDocumentDetailsTO.Path;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblDocumentDetails(Int32 idDocument)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idDocument, cmdDelete);
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

        public static int DeleteTblDocumentDetails(Int32 idDocument, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idDocument, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idDocument, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblDocumentDetails] " +
            " WHERE idDocument = " + idDocument + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idDocument", System.Data.SqlDbType.Int).Value = tblDocumentDetailsTO.IdDocument;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion


        public static List<TblDocumentDetailsTO> ConvertDTToList(SqlDataReader tblDocumentDetailsTODT)
        {
            List<TblDocumentDetailsTO> tblDocumentDetailsTOList = new List<TblDocumentDetailsTO>();
            if (tblDocumentDetailsTODT != null)
            {
                while(tblDocumentDetailsTODT.Read())
                {
                    TblDocumentDetailsTO tblDocumentDetailsTONew = new TblDocumentDetailsTO();
                    if (tblDocumentDetailsTODT["idDocument"] != DBNull.Value)
                        tblDocumentDetailsTONew.IdDocument = Convert.ToInt32(tblDocumentDetailsTODT["idDocument"].ToString());
                    if (tblDocumentDetailsTODT["moduleId"] != DBNull.Value)
                        tblDocumentDetailsTONew.ModuleId = Convert.ToInt32(tblDocumentDetailsTODT["moduleId"].ToString());
                    if (tblDocumentDetailsTODT["createdBy"] != DBNull.Value)
                        tblDocumentDetailsTONew.CreatedBy = Convert.ToInt32(tblDocumentDetailsTODT["createdBy"].ToString());
                    if (tblDocumentDetailsTODT["idActive"] != DBNull.Value)
                        tblDocumentDetailsTONew.IsActive = Convert.ToInt32(tblDocumentDetailsTODT["idActive"].ToString());
                    if (tblDocumentDetailsTODT["createdOn"] != DBNull.Value)
                        tblDocumentDetailsTONew.CreatedOn = Convert.ToDateTime(tblDocumentDetailsTODT["createdOn"].ToString());
                    if (tblDocumentDetailsTODT["documentDesc"] != DBNull.Value)
                        tblDocumentDetailsTONew.DocumentDesc = Convert.ToString(tblDocumentDetailsTODT["documentDesc"].ToString());
                    if (tblDocumentDetailsTODT["path"] != DBNull.Value)
                        tblDocumentDetailsTONew.Path = Convert.ToString(tblDocumentDetailsTODT["path"].ToString());
                    if (tblDocumentDetailsTODT["fileTypeId"] != DBNull.Value)
                        tblDocumentDetailsTONew.FileTypeId = Convert.ToInt32(tblDocumentDetailsTODT["fileTypeId"].ToString());
                    if (tblDocumentDetailsTODT["userDisplayName"] != DBNull.Value)
                        tblDocumentDetailsTONew.UserName = Convert.ToString(tblDocumentDetailsTODT["userDisplayName"].ToString());
                    tblDocumentDetailsTOList.Add(tblDocumentDetailsTONew);
                }
            }
            return tblDocumentDetailsTOList;
        }

    }
}
