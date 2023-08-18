using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblCRMShareDocsDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblCRMShareDocsDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetails()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCRMShareDocsDetailsTO> list = ConvertDTToList(sqlReader);
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

        public static TblCRMShareDocsDetailsTO SelectTblCRMShareDocsDetails(Int32 idShareDoc)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idShareDoc = " + idShareDoc + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShareDoc", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.IdShareDoc;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCRMShareDocsDetailsTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 0)
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

        public static List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShareDoc", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.IdShareDoc;
                // SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCRMShareDocsDetailsTO> list = ConvertDTToList(sqlReader);
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
        public static int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblCRMShareDocsDetailsTO, cmdInsert);
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

        public static int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblCRMShareDocsDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblCRMShareDocsDetails]( " +
            //"  [idShareDoc]" +
            " [visitId]" +
            " ,[documentId]" +
            " ,[userId]" +
            " ,[roleId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[entityTypeId]"+
            " )" +
" VALUES (" +
            //"  @IdShareDoc " +
            " @VisitId " +
            " ,@DocumentId " +
            " ,@UserId " +
            " ,@RoleId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@EntityTypeId"+
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdShareDoc", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.IdShareDoc;
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.VisitId;
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.DocumentId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.UserId;
            cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value =  StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblCRMShareDocsDetailsTO.RoleId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCRMShareDocsDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@EntityTypeId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.EntityTypeId;//EntityTypeId
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblCRMShareDocsDetailsTO, cmdUpdate);
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

        public static int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblCRMShareDocsDetailsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblCRMShareDocsDetails] SET " +
            //"  [idShareDoc] = @IdShareDoc" +
            " [visitId]= @VisitId" +
            " ,[documentId]= @DocumentId" +
            " ,[userId]= @UserId" +
            " ,[roleId]= @RoleId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn] = @CreatedOn" +
            " WHERE [idShareDoc] = @IdShareDoc";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdShareDoc", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.IdShareDoc;
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.VisitId;
            cmdUpdate.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.DocumentId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.UserId;
            cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.RoleId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCRMShareDocsDetailsTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblCRMShareDocsDetails(Int32 idShareDoc)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idShareDoc, cmdDelete);
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

        public static int DeleteTblCRMShareDocsDetails(Int32 idShareDoc, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idShareDoc, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idShareDoc, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblCRMShareDocsDetails] " +
            " WHERE idShareDoc = " + idShareDoc + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idShareDoc", System.Data.SqlDbType.Int).Value = tblCRMShareDocsDetailsTO.IdShareDoc;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        public static List<TblCRMShareDocsDetailsTO> ConvertDTToList(SqlDataReader tblCRMShareDocsDetailsTODT)
        {
            List<TblCRMShareDocsDetailsTO> tblCRMShareDocsDetailsTOList = new List<TblCRMShareDocsDetailsTO>();
            if (tblCRMShareDocsDetailsTODT != null)
            {
                while (tblCRMShareDocsDetailsTODT.Read())
                {
                    TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTONew = new TblCRMShareDocsDetailsTO();
                    if (tblCRMShareDocsDetailsTODT["idShareDoc"] != DBNull.Value)
                        tblCRMShareDocsDetailsTONew.IdShareDoc = Convert.ToInt32(tblCRMShareDocsDetailsTODT["idShareDoc"].ToString());
                    if (tblCRMShareDocsDetailsTODT["visitId"] != DBNull.Value)
                        tblCRMShareDocsDetailsTONew.VisitId = Convert.ToInt32(tblCRMShareDocsDetailsTODT["visitId"].ToString());
                    if (tblCRMShareDocsDetailsTODT["documentId"] != DBNull.Value)
                        tblCRMShareDocsDetailsTONew.DocumentId = Convert.ToInt32(tblCRMShareDocsDetailsTODT["documentId"].ToString());
                    if (tblCRMShareDocsDetailsTODT["userId"] != DBNull.Value)
                        tblCRMShareDocsDetailsTONew.UserId = Convert.ToInt32(tblCRMShareDocsDetailsTODT["userId"].ToString());
                    if (tblCRMShareDocsDetailsTODT["roleId"] != DBNull.Value)
                        tblCRMShareDocsDetailsTONew.RoleId = Convert.ToInt32(tblCRMShareDocsDetailsTODT["roleId"].ToString());
                    if (tblCRMShareDocsDetailsTODT["createdBy"] != DBNull.Value)
                        tblCRMShareDocsDetailsTONew.CreatedBy = Convert.ToInt32(tblCRMShareDocsDetailsTODT["createdBy"].ToString());
                    if (tblCRMShareDocsDetailsTODT["createdOn"] != DBNull.Value)
                        tblCRMShareDocsDetailsTONew.CreatedOn = Convert.ToDateTime(tblCRMShareDocsDetailsTODT["createdOn"].ToString());
                    tblCRMShareDocsDetailsTOList.Add(tblCRMShareDocsDetailsTONew);
                }
            }
            return tblCRMShareDocsDetailsTOList;
        }
    }
}
