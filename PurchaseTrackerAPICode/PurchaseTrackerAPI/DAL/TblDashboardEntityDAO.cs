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
    public class TblDashboardEntityDAO : ITblDashboardEntityDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;


        public TblDashboardEntityDAO(IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblDashboardEntity]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblDashboardEntityTO> SelectAllTblDashboardEntity()
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
                List<TblDashboardEntityTO> list = ConvertDTToList(sqlReader);
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

        public List<TblDashboardEntityTO> SelectTblDashboardEntity(Int32 idDashboardEntity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idDashboardEntity = " + idDashboardEntity +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityTO> list = ConvertDTToList(sqlReader);
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

        public List<TblDashboardEntityTO> SelectAllDashboardEntityListByModuleId(Int32 moduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ISNULL(isActive,0) = 1 AND moduleId = " + moduleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityTO> list = ConvertDTToList(sqlReader);
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

        public List<TblDashboardEntityTO> SelectTblDashboardEntity(Int32 idDashboardEntity,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idDashboardEntity = " + idDashboardEntity + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public List<TblDashboardEntityTO> SelectAllTblDashboardEntity(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityTO> list = ConvertDTToList(sqlReader);
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

        public  List<TblDashboardEntityTO> ConvertDTToList(SqlDataReader tblDashboardEntityTODT)
        {
            List<TblDashboardEntityTO> tblDashboardEntityTOList = new List<TblDashboardEntityTO>();
            if (tblDashboardEntityTODT != null)
            {
                while(tblDashboardEntityTODT.Read())
                {
                    TblDashboardEntityTO tblDashboardEntityTONew = new TblDashboardEntityTO();
                    if (tblDashboardEntityTODT ["idDashboardEntity"] != DBNull.Value)
                        tblDashboardEntityTONew.IdDashboardEntity = Convert.ToInt32(tblDashboardEntityTODT ["idDashboardEntity"].ToString());
                    if (tblDashboardEntityTODT ["moduleId"] != DBNull.Value)
                        tblDashboardEntityTONew.ModuleId = Convert.ToInt32(tblDashboardEntityTODT ["moduleId"].ToString());
                    if (tblDashboardEntityTODT ["isActive"] != DBNull.Value)
                        tblDashboardEntityTONew.IsActive = Convert.ToInt32(tblDashboardEntityTODT ["isActive"].ToString());
                    if (tblDashboardEntityTODT ["createdBy"] != DBNull.Value)
                        tblDashboardEntityTONew.CreatedBy = Convert.ToInt32(tblDashboardEntityTODT ["createdBy"].ToString());
                    if (tblDashboardEntityTODT ["updatedBy"] != DBNull.Value)
                        tblDashboardEntityTONew.UpdatedBy = Convert.ToInt32(tblDashboardEntityTODT ["updatedBy"].ToString());
                    if (tblDashboardEntityTODT ["createdOn"] != DBNull.Value)
                        tblDashboardEntityTONew.CreatedOn = Convert.ToDateTime(tblDashboardEntityTODT ["createdOn"].ToString());
                    if (tblDashboardEntityTODT ["updateOn"] != DBNull.Value)
                        tblDashboardEntityTONew.UpdateOn = Convert.ToDateTime(tblDashboardEntityTODT ["updateOn"].ToString());
                    if (tblDashboardEntityTODT ["entityName"] != DBNull.Value)
                        tblDashboardEntityTONew.EntityName = Convert.ToString(tblDashboardEntityTODT ["entityName"].ToString());
                    if (tblDashboardEntityTODT ["entityValue"] != DBNull.Value)
                        tblDashboardEntityTONew.EntityValue = Convert.ToString(tblDashboardEntityTODT ["entityValue"].ToString());
                    tblDashboardEntityTOList.Add(tblDashboardEntityTONew);
                }
            }
            return tblDashboardEntityTOList;
        }

        //chetan[15-April-2020]
        public List<TblDashboardEntityTO> SelectAllDashboardEntityList(Int32 moduleId, int dashboardEntityId, DateTime fromDate,DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ISNULL(isActive,0) = 1 AND moduleId = " + moduleId + " And idDashboardEntity=" + dashboardEntityId + " and createdOn BETWEEN @fromDate And @toDate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblDashboardEntityTO> list = ConvertDTToList(sqlReader);


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
        public int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblDashboardEntityTO, cmdInsert);
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

        public int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblDashboardEntityTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblDashboardEntityTO tblDashboardEntityTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblDashboardEntity]( " + 
            //"  [idDashboardEntity]" +
            "  [moduleId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updateOn]" +
            " ,[entityName]" +
            " ,[entityValue]" +
            " )" +
" VALUES (" +
            //"  @IdDashboardEntity " +
            "  @ModuleId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdateOn " +
            " ,@EntityName " +
            " ,@EntityValue " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdDashboardEntity", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.IdDashboardEntity;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.ModuleId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblDashboardEntityTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdateOn", System.Data.SqlDbType.DateTime).Value = tblDashboardEntityTO.UpdateOn;
            cmdInsert.Parameters.Add("@EntityName", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityTO.EntityName;
            cmdInsert.Parameters.Add("@EntityValue", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityTO.EntityValue;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblDashboardEntityTO, cmdUpdate);
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

        public int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblDashboardEntityTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblDashboardEntityTO tblDashboardEntityTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblDashboardEntity] SET " + 
            //"  [idDashboardEntity] = @IdDashboardEntity" +
            "  [moduleId]= @ModuleId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updateOn]= @UpdateOn" +
            " ,[entityName]= @EntityName" +
            " ,[entityValue] = @EntityValue" +
            " WHERE idDashboardEntity = @IdDashboardEntity "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdDashboardEntity", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.IdDashboardEntity;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.ModuleId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblDashboardEntityTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblDashboardEntityTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblDashboardEntityTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdateOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblDashboardEntityTO.UpdateOn);
            cmdUpdate.Parameters.Add("@EntityName", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityTO.EntityName;
            cmdUpdate.Parameters.Add("@EntityValue", System.Data.SqlDbType.VarChar).Value = tblDashboardEntityTO.EntityValue;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblDashboardEntity(Int32 idDashboardEntity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idDashboardEntity, cmdDelete);
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

        public int DeleteTblDashboardEntity(Int32 idDashboardEntity, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idDashboardEntity, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idDashboardEntity, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblDashboardEntity] " +
            " WHERE idDashboardEntity = " + idDashboardEntity +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idDashboardEntity", System.Data.SqlDbType.Int).Value = tblDashboardEntityTO.IdDashboardEntity;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
