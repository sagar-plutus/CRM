using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblTaskModuleExtDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblTaskModuleExt]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblTaskModuleExtTO> SelectAllTblTaskModuleExt()
        {
            String sqlConnStr = Startup.ConnectionString ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTaskModuleExt", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.IdTaskModuleExt;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTaskModuleExtTO> TblTaskModuleExtTOList = ConvertDTToList(sqlReader);
                if (TblTaskModuleExtTOList != null)
                    return TblTaskModuleExtTOList;
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

        public static TblTaskModuleExtTO SelectTblTaskModuleExt(Int32 idTaskModuleExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTaskModuleExt = " + idTaskModuleExt + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTaskModuleExt", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.IdTaskModuleExt;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTaskModuleExtTO> TblTaskModuleExtTOList = ConvertDTToList(sqlReader);
                if (TblTaskModuleExtTOList != null && TblTaskModuleExtTOList.Count > 0)
                    return TblTaskModuleExtTOList[0];
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

        public static List<TblTaskModuleExtTO> SelectAllTblTaskModuleExt(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTaskModuleExt", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.IdTaskModuleExt;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTaskModuleExtTO> TblTaskModuleExtTOList = ConvertDTToList(sqlReader);
                if (TblTaskModuleExtTOList != null && TblTaskModuleExtTOList.Count > 0)
                    return TblTaskModuleExtTOList;
                else
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

        public static List<TblTaskModuleExtTO> SelectTaskModuleDetailsByEntityId(Int32 EntityId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE entityId = " + EntityId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTaskModuleExt", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.IdTaskModuleExt;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTaskModuleExtTO> TblTaskModuleExtTOList = ConvertDTToList(sqlReader);
                if (TblTaskModuleExtTOList != null && TblTaskModuleExtTOList.Count > 0)
                    return TblTaskModuleExtTOList;
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
        public static int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblTaskModuleExtTO, cmdInsert);
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

        public static int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTaskModuleExtTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblTaskModuleExtTO tblTaskModuleExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTaskModuleExt]( " +
            //"  [idTaskModuleExt]" +
            " [moduleId]" +
            " ,[taskId]" +
            " ,[taskTypeId]" +
            " ,[entityId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[eventTypeId]" +
            " )" +
" VALUES (" +
            //"  @IdTaskModuleExt " +
            " @ModuleId " +
            " ,@TaskId " +
            " ,@TaskTypeId " +
            " ,@EntityId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@EventTypeId " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdTaskModuleExt", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.IdTaskModuleExt;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.ModuleId;
            cmdInsert.Parameters.Add("@TaskId", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.TaskId;
            cmdInsert.Parameters.Add("@TaskTypeId", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.TaskTypeId;
            cmdInsert.Parameters.Add("@EntityId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTaskModuleExtTO.EntityId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTaskModuleExtTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTaskModuleExtTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTaskModuleExtTO.UpdatedOn);
            cmdInsert.Parameters.Add("@EventTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTaskModuleExtTO.EventTypeId); 
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblTaskModuleExtTO, cmdUpdate);
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

        public static int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTaskModuleExtTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblTaskModuleExtTO tblTaskModuleExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTaskModuleExt] SET " +
            //"  [idTaskModuleExt] = @IdTaskModuleExt" +
            " [moduleId]= @ModuleId" +
            " ,[taskId]= @TaskId" +
            " ,[taskTypeId]= @TaskTypeId" +
            " ,[entityId]= @EntityId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[eventTypeId]=@EventTypeId " +
            " WHERE [idTaskModuleExt] = @IdTaskModuleExt ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTaskModuleExt", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.IdTaskModuleExt;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.ModuleId;
            cmdUpdate.Parameters.Add("@TaskId", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.TaskId;
            cmdUpdate.Parameters.Add("@TaskTypeId", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.TaskTypeId;
            cmdUpdate.Parameters.Add("@EntityId", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.EntityId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTaskModuleExtTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTaskModuleExtTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@EventTypeId ", System.Data.SqlDbType.DateTime).Value = tblTaskModuleExtTO.EventTypeId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblTaskModuleExt(Int32 idTaskModuleExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTaskModuleExt, cmdDelete);
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

        public static int DeleteTblTaskModuleExt(Int32 idTaskModuleExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTaskModuleExt, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idTaskModuleExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblTaskModuleExt] " +
            " WHERE idTaskModuleExt = " + idTaskModuleExt + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTaskModuleExt", System.Data.SqlDbType.Int).Value = tblTaskModuleExtTO.IdTaskModuleExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblTaskModuleExtTO> ConvertDTToList(SqlDataReader tblTaskModuleExtTODT)
        {
            List<TblTaskModuleExtTO> tblTaskModuleExtTOList = new List<TblTaskModuleExtTO>();
            if (tblTaskModuleExtTODT != null)
            {
                while (tblTaskModuleExtTODT.Read())
                {
                    TblTaskModuleExtTO tblTaskModuleExtTONew = new TblTaskModuleExtTO();
                    if (tblTaskModuleExtTODT["idTaskModuleExt"] != DBNull.Value)
                        tblTaskModuleExtTONew.IdTaskModuleExt = Convert.ToInt32(tblTaskModuleExtTODT["idTaskModuleExt"].ToString());
                    if (tblTaskModuleExtTODT["moduleId"] != DBNull.Value)
                        tblTaskModuleExtTONew.ModuleId = Convert.ToInt32(tblTaskModuleExtTODT["moduleId"].ToString());
                    if (tblTaskModuleExtTODT["taskId"] != DBNull.Value)
                        tblTaskModuleExtTONew.TaskId = Convert.ToInt32(tblTaskModuleExtTODT["taskId"].ToString());
                    if (tblTaskModuleExtTODT["taskTypeId"] != DBNull.Value)
                        tblTaskModuleExtTONew.TaskTypeId = Convert.ToInt32(tblTaskModuleExtTODT["taskTypeId"].ToString());
                    if (tblTaskModuleExtTODT["entityId"] != DBNull.Value)
                        tblTaskModuleExtTONew.EntityId = Convert.ToInt32(tblTaskModuleExtTODT["entityId"].ToString());
                    if (tblTaskModuleExtTODT["createdBy"] != DBNull.Value)
                        tblTaskModuleExtTONew.CreatedBy = Convert.ToInt32(tblTaskModuleExtTODT["createdBy"].ToString());
                    if (tblTaskModuleExtTODT["updatedBy"] != DBNull.Value)
                        tblTaskModuleExtTONew.UpdatedBy = Convert.ToInt32(tblTaskModuleExtTODT["updatedBy"].ToString());
                    if (tblTaskModuleExtTODT["createdOn"] != DBNull.Value)
                        tblTaskModuleExtTONew.CreatedOn = Convert.ToDateTime(tblTaskModuleExtTODT["createdOn"].ToString());
                    if (tblTaskModuleExtTODT["updatedOn"] != DBNull.Value)
                        tblTaskModuleExtTONew.UpdatedOn = Convert.ToDateTime(tblTaskModuleExtTODT["updatedOn"].ToString());
                    if (tblTaskModuleExtTODT["eventTypeId"] != DBNull.Value)
                        tblTaskModuleExtTONew.EventTypeId = Convert.ToInt32(tblTaskModuleExtTODT["eventTypeId"].ToString());
                    
                    tblTaskModuleExtTOList.Add(tblTaskModuleExtTONew);
                }
            }
            return tblTaskModuleExtTOList;
        }

    }
}
