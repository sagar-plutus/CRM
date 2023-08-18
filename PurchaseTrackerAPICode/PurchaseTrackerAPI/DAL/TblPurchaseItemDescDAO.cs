using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using System.Configuration;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.Models;


namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseItemDescDAO : ITblPurchaseItemDescDAO
    {
        #region Methods

        private readonly IConnectionString _iConnectionString;
        public TblPurchaseItemDescDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblPurchaseItemDesc.*,tblProdItemDesc.*, tblPurchaseWeighingStageSummary.weightStageId FROM [tblPurchaseItemDesc] tblPurchaseItemDesc "
                                 + " left join tblProdItemDesc tblProdItemDesc on tblProdItemDesc.idProdItemDesc = tblPurchaseItemDesc.ProdItemDescId "
                                 + " left join tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary on tblPurchaseWeighingStageSummary.idPurchaseWeighingStage =  tblPurchaseItemDesc.StageId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc()
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

                //cmdSelect.Parameters.Add("@idPurchaseItemDesc", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.IdPurchaseItemDesc;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc(int rootScheduleId, int itemId, int stageId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where rootScheduleId =@rootScheduleId and StageId =@StageId and prodItemId =@prodItemId and tblPurchaseItemDesc.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@rootScheduleId", System.Data.SqlDbType.Int).Value = rootScheduleId;
                cmdSelect.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = itemId;
                cmdSelect.Parameters.Add("@StageId", System.Data.SqlDbType.Int).Value = stageId;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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
        public List<TblPurchaseItemDescTO> GetAllDescriptionListForCorrection(int rootScheduleId, int itemId, int phaseId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where rootScheduleId =@rootScheduleId and phaseid =@phaseId and prodItemId =@prodItemId and tblPurchaseItemDesc.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@rootScheduleId", System.Data.SqlDbType.Int).Value = rootScheduleId;
                cmdSelect.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = itemId;
                cmdSelect.Parameters.Add("@phaseId", System.Data.SqlDbType.Int).Value = phaseId;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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


        private List<TblPurchaseItemDescTO> ConvertDTToList(SqlDataReader sqlReader)
        {
            List<TblPurchaseItemDescTO> list = new List<TblPurchaseItemDescTO>();
            if (sqlReader != null)
            {
                while (sqlReader.Read())
                {
                    TblPurchaseItemDescTO tblPurchaseItemDescTONew = new TblPurchaseItemDescTO();
                    if (sqlReader["idPurchaseItemDesc"] != DBNull.Value)
                        tblPurchaseItemDescTONew.IdPurchaseItemDesc = Convert.ToInt32(sqlReader["idPurchaseItemDesc"].ToString());

                    if (sqlReader["name"] != DBNull.Value)
                        tblPurchaseItemDescTONew.Name = sqlReader["name"].ToString();

                    if (sqlReader["description"] != DBNull.Value)
                        tblPurchaseItemDescTONew.Description = sqlReader["description"].ToString();

                    if (sqlReader["rootScheduleId"] != DBNull.Value)
                        tblPurchaseItemDescTONew.RootScheduleId = Convert.ToInt32(sqlReader["rootScheduleId"].ToString());
                    if (sqlReader["phaseId"] != DBNull.Value)
                        tblPurchaseItemDescTONew.PhaseId = Convert.ToInt32(sqlReader["phaseId"].ToString());
                    if (sqlReader["prodItemDescId"] != DBNull.Value)
                        tblPurchaseItemDescTONew.ProdItemDescId = Convert.ToInt32(sqlReader["prodItemDescId"].ToString());
                    if (sqlReader["prodItemId"] != DBNull.Value)
                        tblPurchaseItemDescTONew.ProdItemId = Convert.ToInt32(sqlReader["prodItemId"].ToString());
                    if (sqlReader["StageId"] != DBNull.Value)
                        tblPurchaseItemDescTONew.StageId = Convert.ToInt32(sqlReader["StageId"].ToString());
                    if (sqlReader["isActive"] != DBNull.Value)
                        tblPurchaseItemDescTONew.IsActive = Convert.ToInt32(sqlReader["isActive"].ToString());
                    if (sqlReader["createdBy"] != DBNull.Value)
                        tblPurchaseItemDescTONew.CreatedBy = Convert.ToInt32(sqlReader["createdBy"].ToString());
                    if (sqlReader["updatedBy"] != DBNull.Value)
                        tblPurchaseItemDescTONew.UpdatedBy = Convert.ToInt32(sqlReader["updatedBy"].ToString());


                    if (sqlReader["weightStageId"] != DBNull.Value)
                        tblPurchaseItemDescTONew.WtStageId = Convert.ToInt32(sqlReader["weightStageId"].ToString());

                    if (sqlReader["createdOn"] != DBNull.Value)
                        tblPurchaseItemDescTONew.CreatedOn = Convert.ToDateTime(sqlReader["createdOn"].ToString());
                    if (sqlReader["updatedOn"] != DBNull.Value)
                        tblPurchaseItemDescTONew.UpdatedOn = Convert.ToDateTime(sqlReader["updatedOn"].ToString());

                    list.Add(tblPurchaseItemDescTONew);
                }
            }
            return list;
        }

        public TblPurchaseItemDescTO SelectTblPurchaseItemDesc(Int32 idPurchaseItemDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseItemDesc = " + idPurchaseItemDesc + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseItemDesc", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.IdPurchaseItemDesc;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list[0];
                else return null;
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


        public List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc(SqlConnection conn, SqlTransaction tran, int rootScheduleId, int itemId, int stageId)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " where rootScheduleId =@rootScheduleId and prodItemId =@prodItemId and StageId =@StageId and tblPurchaseItemDesc.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@rootScheduleId", System.Data.SqlDbType.Int).Value = rootScheduleId;
                cmdSelect.Parameters.Add("@StageId", System.Data.SqlDbType.Int).Value = stageId;
                cmdSelect.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = itemId;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseItemDesc", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.IdPurchaseItemDesc;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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
        public int InsertTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseItemDescTO, cmdInsert);
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

        public int InsertTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseItemDescTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseItemDesc]( " +
            "  [rootScheduleId]" +
            " ,[phaseId]" +
            " ,[prodItemDescId]" +
            " ,[prodItemId]" +
            " ,[StageId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " )" +
    " VALUES (" +
            "  @RootScheduleId " +
            " ,@PhaseId " +
            " ,@ProdItemDescId " +
            " ,@ProdItemId " +
            " ,@StageId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.RootScheduleId);
            cmdInsert.Parameters.Add("@PhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.PhaseId);
            cmdInsert.Parameters.Add("@ProdItemDescId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.ProdItemDescId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.ProdItemId);
            cmdInsert.Parameters.Add("@StageId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.StageId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.UpdatedOn);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseItemDescTO, cmdUpdate);
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

        public int UpdateTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseItemDescTO, cmdUpdate);
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

        public int UpdateAllTblPurchaseItemDescPhaseAndRootWise(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandForAll(tblPurchaseItemDescTO, cmdUpdate);
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

        public int ExecuteUpdationCommandForAll(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlCommand cmdUpdate)
        {
            string phaseId = tblPurchaseItemDescTO.PhaseId.ToString();

            if (tblPurchaseItemDescTO.PhaseId == (int)Constants.PurchaseVehiclePhasesE.UNLOADING_COMPLETED)
            {
                phaseId = tblPurchaseItemDescTO.PhaseId + "," + (int)Constants.PurchaseVehiclePhasesE.GRADING;
            }

            String sqlQuery = @" UPDATE [tblPurchaseItemDesc] SET " +
            "  [isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE rootScheduleId = @RootScheduleId and  phaseId in ( " + phaseId + " ) and prodItemId=@ProdItemId and StageId=@StageId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.RootScheduleId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.ProdItemId;
            cmdUpdate.Parameters.Add("@StageId", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.StageId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemDescTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }

        public int ExecuteUpdationCommand(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseItemDesc] SET " +
            "  [rootScheduleId]= @RootScheduleId" +
            " ,[phaseId]= @PhaseId" +
            " ,[prodItemDescId]= @ProdItemDescId" +
            " ,[StageId]= @StageId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseItemDesc", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.IdPurchaseItemDesc;
            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.RootScheduleId;
            cmdUpdate.Parameters.Add("@PhaseId", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.PhaseId;
            cmdUpdate.Parameters.Add("@StageId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemDescTO.StageId);
            cmdUpdate.Parameters.Add("@ProdItemDescId", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.ProdItemDescId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.ProdItemId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemDescTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemDescTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseItemDesc(Int32 idPurchaseItemDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseItemDesc, cmdDelete);
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

        public int DeleteTblPurchaseItemDesc(Int32 idPurchaseItemDesc, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseItemDesc, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchaseItemDesc, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseItemDesc] " +
            " WHERE idPurchaseItemDesc = " + idPurchaseItemDesc + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseItemDesc", System.Data.SqlDbType.Int).Value = tblPurchaseItemDescTO.IdPurchaseItemDesc;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllPurchaseItemDescAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseItemDesc WHERE rootScheduleId=" + purchaseScheduleId + "  OR stageId IN (select idPurchaseWeighingStage from tblPurchaseWeighingStageSummary where purchaseScheduleSummaryId = " + purchaseScheduleId + ")";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
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

        #endregion

    }
}
