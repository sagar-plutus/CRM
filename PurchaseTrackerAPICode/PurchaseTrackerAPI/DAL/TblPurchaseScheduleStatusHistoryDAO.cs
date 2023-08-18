using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseScheduleStatusHistoryDAO : ITblPurchaseScheduleStatusHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseScheduleStatusHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseScheduleStatusHistory]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistory()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                List<TblPurchaseScheduleStatusHistoryTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblPurchaseScheduleStatusHistoryTO SelectTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId)
        {
            SqlDataReader reader = null;

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE  isLatest =1 and PurchaseScheduleSummaryId = " + purchaseScheduleSummaryId + " ";
                // isActive = 1 and
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleStatusHistoryTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }





        public List<TblPurchaseScheduleStatusHistoryTO> SelectTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId, bool getApprovedTO, bool isForApproval, Int32 StatusId, SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader reader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {

                if (isForApproval)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE PurchaseScheduleSummaryId = " + purchaseScheduleSummaryId +
                     " and (statusId = " + StatusId + " or Isnull(isIgnoreApproval,0)  = " + 1 + ") ";
                    if (getApprovedTO)
                    {
                        cmdSelect.CommandText += " AND Isnull(isApproved, 0) = 1";
                    }
                    else
                    {
                        cmdSelect.CommandText += " AND Isnull(isApproved, 0) != 1";
                    }
                }
                else
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE PurchaseScheduleSummaryId = " + purchaseScheduleSummaryId + "and isIgnoreApproval  = " + 1;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleStatusHistoryTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader reader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleStatusHistoryTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public List<TblPurchaseScheduleStatusHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseScheduleStatusHistoryTODT)
        {
            List<TblPurchaseScheduleStatusHistoryTO> tblPurchaseScheduleStatusHistoryTOList = new List<TblPurchaseScheduleStatusHistoryTO>();
            if (tblPurchaseScheduleStatusHistoryTODT != null)
            {
                while (tblPurchaseScheduleStatusHistoryTODT.Read())
                {
                    TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTONew = new TblPurchaseScheduleStatusHistoryTO();
                    if (tblPurchaseScheduleStatusHistoryTODT["idScheduleAuthHistory"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.IdScheduleAuthHistory = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["idScheduleAuthHistory"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["PurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["PurchaseScheduleSummaryId"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["phaseId"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.PhaseId = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["phaseId"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["createdBy"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["updatedBy"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["acceptStatusId"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.AcceptStatusId = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["acceptStatusId"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["acceptPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.AcceptPhaseId = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["acceptPhaseId"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["rejectStatusId"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.RejectStatusId = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["rejectStatusId"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["rejectPhaseId"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.RejectPhaseId = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["rejectPhaseId"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["isActive"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["isIgnoreApproval"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.IsIgnoreApproval = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["isIgnoreApproval"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["isApproved"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.IsApproved = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["isApproved"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["isLatest"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.IsLatest = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["isLatest"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleStatusHistoryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleStatusHistoryTODT["updatedOn"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["statusRemark"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.StatusRemark = Convert.ToString(tblPurchaseScheduleStatusHistoryTODT["statusRemark"].ToString());
                    if (tblPurchaseScheduleStatusHistoryTODT["approvalType"] != DBNull.Value)
                        tblPurchaseScheduleStatusHistoryTONew.ApprovalType = Convert.ToInt32(tblPurchaseScheduleStatusHistoryTODT["approvalType"].ToString());

                    tblPurchaseScheduleStatusHistoryTOList.Add(tblPurchaseScheduleStatusHistoryTONew);
                }
            }
            return tblPurchaseScheduleStatusHistoryTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseScheduleStatusHistoryTO, cmdInsert);
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

        public int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseScheduleStatusHistoryTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseScheduleStatusHistory]( " +
            " [PurchaseScheduleSummaryId]" +
            " ,[statusId]" +
            " ,[phaseId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[acceptStatusId]" +
            " ,[acceptPhaseId]" +
            " ,[rejectStatusId]" +
            " ,[rejectPhaseId]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[statusRemark]" +
            " ,[navigationUrl]" +
            " ,[isIgnoreApproval]" +
            " ,[isApproved]" +
             " ,[isLatest]" +
             " ,[approvalType]" +
            " )" +
" VALUES (" +
            " @PurchaseScheduleSummaryId " +
            " ,@StatusId " +
            " ,@PhaseId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@AcceptStatusId " +
            " ,@AcceptPhaseId " +
            " ,@RejectStatusId " +
            " ,@RejectPhaseId " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@StatusRemark " +
            ",@NavigationUrl" +
            ",@IsIgnoreApproval" +
            ",@IsApproved" +
            ",@IsLatest" +
            ",@ApprovalType" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            // cmdInsert.Parameters.Add("@IdScheduleAuthHistory", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.IdScheduleAuthHistory;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@PhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.PhaseId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.UpdatedBy);
            cmdInsert.Parameters.Add("@AcceptStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.AcceptStatusId);
            cmdInsert.Parameters.Add("@AcceptPhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.AcceptPhaseId);
            cmdInsert.Parameters.Add("@RejectStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.RejectStatusId);
            cmdInsert.Parameters.Add("@RejectPhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.RejectPhaseId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseScheduleStatusHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.UpdatedOn);
            cmdInsert.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.StatusRemark);
            cmdInsert.Parameters.Add("@NavigationUrl", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.NavigationUrl);
            cmdInsert.Parameters.Add("@IsIgnoreApproval", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.IsIgnoreApproval);
            cmdInsert.Parameters.Add("@IsLatest", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.IsLatest);
            cmdInsert.Parameters.Add("@IsApproved", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.IsApproved);
            cmdInsert.Parameters.Add("@ApprovalType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.ApprovalType);

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseScheduleStatusHistoryTO, cmdUpdate);
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

        public int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseScheduleStatusHistoryTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseScheduleStatusHistory] SET " +
            " [PurchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[statusId]= @StatusId" +
            " ,[phaseId]= @PhaseId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[acceptStatusId]= @AcceptStatusId" +
            " ,[acceptPhaseId]= @AcceptPhaseId" +
            " ,[rejectStatusId]= @RejectStatusId" +
            " ,[rejectPhaseId]= @RejectPhaseId" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[statusRemark] = @StatusRemark" +
            ",[navigationUrl] = @NavigationUrl " +
            ",[isIgnoreApproval] = @IsIgnoreApproval " +
            ",[isLatest] = @IsLatest " +
            ",[isApproved] = @IsApproved " +
            ",[approvalType] = @ApprovalType " +
            " WHERE [idScheduleAuthHistory] = @IdScheduleAuthHistory ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdScheduleAuthHistory", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.IdScheduleAuthHistory;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.StatusId;
            cmdUpdate.Parameters.Add("@PhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.PhaseId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@AcceptStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.AcceptStatusId);
            cmdUpdate.Parameters.Add("@AcceptPhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.AcceptPhaseId);
            cmdUpdate.Parameters.Add("@RejectStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.RejectStatusId);
            cmdUpdate.Parameters.Add("@RejectPhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.RejectPhaseId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseScheduleStatusHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseScheduleStatusHistoryTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.StatusRemark);
            cmdUpdate.Parameters.Add("@NavigationUrl", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.NavigationUrl);
            cmdUpdate.Parameters.Add("@IsIgnoreApproval", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.IsIgnoreApproval);
            cmdUpdate.Parameters.Add("@IsApproved", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.IsApproved);
            cmdUpdate.Parameters.Add("@IsLatest", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.IsLatest);
            cmdUpdate.Parameters.Add("@ApprovalType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseScheduleStatusHistoryTO.ApprovalType);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(purchaseScheduleSummaryId, cmdDelete);
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

        public int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(purchaseScheduleSummaryId, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 purchaseScheduleSummaryId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseScheduleStatusHistory] " +
            " WHERE PurchaseScheduleSummaryId = " + purchaseScheduleSummaryId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleStatusHistoryTO.PurchaseScheduleSummaryId;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllStatusHistoryAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseScheduleStatusHistory WHERE PurchaseScheduleSummaryId=" + purchaseScheduleId + "";
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
