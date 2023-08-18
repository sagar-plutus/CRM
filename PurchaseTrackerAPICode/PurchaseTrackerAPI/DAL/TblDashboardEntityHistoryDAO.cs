using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblDashboardEntityHistoryDAO : ITblDashboardEntityHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;

        public TblDashboardEntityHistoryDAO(IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblDashboardEntityHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblDashboardEntityHistoryTO> SelectAllTblDashboardEntityHistory()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblDashboardEntityHistoryTO> SelectTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idDashboardEntityHistoryId = " + idDashboardEntityHistoryId +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblDashboardEntityHistoryTO> SelectAllTblDashboardEntityHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }


        public List<TblDashboardEntityHistoryTO> ConvertDTToList(SqlDataReader tblDashboardEntityHistoryTODT)
        {
            List<TblDashboardEntityHistoryTO> tblDashboardEntityHistoryTOList = new List<TblDashboardEntityHistoryTO>();
            if (tblDashboardEntityHistoryTODT != null)
            {
                while(tblDashboardEntityHistoryTODT.Read())   
                {
                    TblDashboardEntityHistoryTO tblDashboardEntityHistoryTONew = new TblDashboardEntityHistoryTO();
                    if (tblDashboardEntityHistoryTODT ["idDashboardEntityHistoryId"] != DBNull.Value)
                        tblDashboardEntityHistoryTONew.IdDashboardEntityHistoryId = Convert.ToInt32(tblDashboardEntityHistoryTODT ["idDashboardEntityHistoryId"].ToString());
                    if (tblDashboardEntityHistoryTODT ["dashboardEntityId"] != DBNull.Value)
                        tblDashboardEntityHistoryTONew.DashboardEntityId = Convert.ToInt32(tblDashboardEntityHistoryTODT ["dashboardEntityId"].ToString());
                    if (tblDashboardEntityHistoryTODT ["moduleId"] != DBNull.Value)
                        tblDashboardEntityHistoryTONew.ModuleId = Convert.ToInt32(tblDashboardEntityHistoryTODT ["moduleId"].ToString());
                    if (tblDashboardEntityHistoryTODT ["createdBy"] != DBNull.Value)
                        tblDashboardEntityHistoryTONew.CreatedBy = Convert.ToInt32(tblDashboardEntityHistoryTODT ["createdBy"].ToString());
                    if (tblDashboardEntityHistoryTODT ["createdOn"] != DBNull.Value)
                        tblDashboardEntityHistoryTONew.CreatedOn = Convert.ToDateTime(tblDashboardEntityHistoryTODT ["createdOn"].ToString());
                    if (tblDashboardEntityHistoryTODT ["entityName"] != DBNull.Value)
                        tblDashboardEntityHistoryTONew.EntityName = Convert.ToString(tblDashboardEntityHistoryTODT ["entityName"].ToString());
                    if (tblDashboardEntityHistoryTODT ["entityValue"] != DBNull.Value)
                        tblDashboardEntityHistoryTONew.EntityValue = Convert.ToString(tblDashboardEntityHistoryTODT ["entityValue"].ToString());
                    tblDashboardEntityHistoryTOList.Add(tblDashboardEntityHistoryTONew);
                }
            }
            return tblDashboardEntityHistoryTOList;
        }

        public List<TblDashboardEntityHistoryTO> SelectAllDashboardEntityList(Int32 moduleId, int dashboardEntityId , DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE moduleId = " + moduleId + " And dashboardEntityId=" + dashboardEntityId + " and createdOn BETWEEN @fromDate And @toDate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityHistoryTO> list = ConvertDTToList(sqlReader);
                

                sqlReader.Dispose();
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
        #endregion

        #region Insertion
        public int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblDashboardEntityHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblDashboardEntityHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblDashboardEntityHistory]( " + 
            //"  [idDashboardEntityHistoryId]" +
            "  [dashboardEntityId]" +
            " ,[moduleId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[entityName]" +
            " ,[entityValue]" +
            " )" +
" VALUES (" +
            //"  @IdDashboardEntityHistoryId " +
            "  @DashboardEntityId " +
            " ,@ModuleId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@EntityName " +
            " ,@EntityValue " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdDashboardEntityHistoryId", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.IdDashboardEntityHistoryId;
            cmdInsert.Parameters.Add("@DashboardEntityId", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.DashboardEntityId;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.ModuleId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblDashboardEntityHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@EntityName", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityHistoryTO.EntityName;
            cmdInsert.Parameters.Add("@EntityValue", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityHistoryTO.EntityValue;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblDashboardEntityHistoryTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblDashboardEntityHistoryTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblDashboardEntityHistory] SET " + 
            //"  [idDashboardEntityHistoryId] = @IdDashboardEntityHistoryId" +
            "  [dashboardEntityId]= @DashboardEntityId" +
            " ,[moduleId]= @ModuleId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[entityName]= @EntityName" +
            " ,[entityValue] = @EntityValue" +
            " WHERE idDashboardEntityHistoryId = @IdDashboardEntityHistoryId "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdDashboardEntityHistoryId", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.IdDashboardEntityHistoryId;
            cmdUpdate.Parameters.Add("@DashboardEntityId", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.DashboardEntityId;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.ModuleId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblDashboardEntityHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@EntityName", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityHistoryTO.EntityName;
            cmdUpdate.Parameters.Add("@EntityValue", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityHistoryTO.EntityValue;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idDashboardEntityHistoryId, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idDashboardEntityHistoryId, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idDashboardEntityHistoryId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblDashboardEntityHistory] " +
            " WHERE idDashboardEntityHistoryId = " + idDashboardEntityHistoryId +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idDashboardEntityHistoryId", System.Data.SqlDbType.Int).Value = tblDashboardEntityHistoryTO.IdDashboardEntityHistoryId;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
