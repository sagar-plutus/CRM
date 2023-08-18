using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblLoadingSlipExtHistoryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tempLoadingSlipExtHistory]" +

                                  // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipExtHistory
                                  " UNION ALL " +
                                  " SELECT * FROM [finalLoadingSlipExtHistory]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingSlipExtHistoryTO> SelectAllTblLoadingSlipExtHistory()
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
                List<TblLoadingSlipExtHistoryTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
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

        public static TblLoadingSlipExtHistoryTO SelectTblLoadingSlipExtHistory(Int32 idConfirmHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery()+ ")sq1 WHERE idConfirmHistory = " + idConfirmHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtHistoryTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch(Exception ex)
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

        public static List<TblLoadingSlipExtHistoryTO> ConvertDTToList(SqlDataReader tblLoadingSlipExtHistoryTODT)
        {
            List<TblLoadingSlipExtHistoryTO> tblLoadingSlipExtHistoryTOList = new List<TblLoadingSlipExtHistoryTO>();
            if (tblLoadingSlipExtHistoryTODT != null)
            {
                while (tblLoadingSlipExtHistoryTODT.Read())
                {
                    TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTONew = new TblLoadingSlipExtHistoryTO();
                    if (tblLoadingSlipExtHistoryTODT["idConfirmHistory"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.IdConfirmHistory = Convert.ToInt32(tblLoadingSlipExtHistoryTODT["idConfirmHistory"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["loadingSlipExtId"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.LoadingSlipExtId = Convert.ToInt32(tblLoadingSlipExtHistoryTODT["loadingSlipExtId"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["lastConfirmationStatus"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.LastConfirmationStatus = Convert.ToInt32(tblLoadingSlipExtHistoryTODT["lastConfirmationStatus"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["currentConfirmationStatus"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.CurrentConfirmationStatus = Convert.ToInt32(tblLoadingSlipExtHistoryTODT["currentConfirmationStatus"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["parityDtlId"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.ParityDtlId = Convert.ToInt32(tblLoadingSlipExtHistoryTODT["parityDtlId"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["createdBy"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.CreatedBy = Convert.ToInt32(tblLoadingSlipExtHistoryTODT["createdBy"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["createdOn"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.CreatedOn = Convert.ToDateTime(tblLoadingSlipExtHistoryTODT["createdOn"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["lastRatePerMT"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.LastRatePerMT = Convert.ToDouble(tblLoadingSlipExtHistoryTODT["lastRatePerMT"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["currentRatePerMT"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.CurrentRatePerMT = Convert.ToDouble(tblLoadingSlipExtHistoryTODT["currentRatePerMT"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["lastRateCalcDesc"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.LastRateCalcDesc = Convert.ToString(tblLoadingSlipExtHistoryTODT["lastRateCalcDesc"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["currentRateCalcDesc"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.CurrentRateCalcDesc = Convert.ToString(tblLoadingSlipExtHistoryTODT["currentRateCalcDesc"].ToString());
                    if (tblLoadingSlipExtHistoryTODT["lastCdAplAmt"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.LastCdAplAmt = Convert.ToDouble(tblLoadingSlipExtHistoryTODT["lastCdAplAmt"]);
                    if (tblLoadingSlipExtHistoryTODT["currentCdAplAmt"] != DBNull.Value)
                        tblLoadingSlipExtHistoryTONew.CurrentCdAplAmt = Convert.ToDouble(tblLoadingSlipExtHistoryTODT["currentCdAplAmt"]);

                    tblLoadingSlipExtHistoryTOList.Add(tblLoadingSlipExtHistoryTONew);
                }
            }
            return tblLoadingSlipExtHistoryTOList;
        }

        public static List<TblLoadingSlipExtHistoryTO> SelectTempLoadingSlipExtHistoryList(Int32 loadingSlipExtId,SqlConnection conn,SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipExtId = " + loadingSlipExtId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtHistoryTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        #endregion

        #region Insertion
        public static int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipExtHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipExtHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlipExtHistory]( " +
                                "  [loadingSlipExtId]" +
                                " ,[lastConfirmationStatus]" +
                                " ,[currentConfirmationStatus]" +
                                " ,[parityDtlId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[lastRatePerMT]" +
                                " ,[currentRatePerMT]" +
                                " ,[lastRateCalcDesc]" +
                                " ,[currentRateCalcDesc]" +
                                " ,[lastCdAplAmt]" +
    " ,[currentCdAplAmt]" +
                                " )" +
                    " VALUES (" +
                                "  @LoadingSlipExtId " +
                                " ,@LastConfirmationStatus " +
                                " ,@CurrentConfirmationStatus " +
                                " ,@ParityDtlId " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@LastRatePerMT " +
                                " ,@CurrentRatePerMT " +
                                " ,@LastRateCalcDesc " +
                                " ,@CurrentRateCalcDesc " +
                                 " ,@lastCdAplAmt " +
    " ,@currentCdAplAmt " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdConfirmHistory", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.IdConfirmHistory;
            cmdInsert.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.LoadingSlipExtId;
            cmdInsert.Parameters.Add("@LastConfirmationStatus", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.LastConfirmationStatus;
            cmdInsert.Parameters.Add("@CurrentConfirmationStatus", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.CurrentConfirmationStatus;
            cmdInsert.Parameters.Add("@ParityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.ParityDtlId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipExtHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@LastRatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtHistoryTO.LastRatePerMT;
            cmdInsert.Parameters.Add("@CurrentRatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtHistoryTO.CurrentRatePerMT;
            cmdInsert.Parameters.Add("@LastRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.LastRateCalcDesc);
            cmdInsert.Parameters.Add("@CurrentRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.CurrentRateCalcDesc);
            cmdInsert.Parameters.Add("@lastCdAplAmt", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtHistoryTO.LastCdAplAmt;
            cmdInsert.Parameters.Add("@currentCdAplAmt", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtHistoryTO.CurrentCdAplAmt;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipExtHistoryTO.IdConfirmHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipExtHistoryTO, cmdUpdate);
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

        public static int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipExtHistoryTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlipExtHistory] SET " + 
            "  [idConfirmHistory] = @IdConfirmHistory" +
            " ,[loadingSlipExtId]= @LoadingSlipExtId" +
            " ,[lastConfirmationStatus]= @LastConfirmationStatus" +
            " ,[currentConfirmationStatus]= @CurrentConfirmationStatus" +
            " ,[parityDtlId]= @ParityDtlId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[lastRatePerMT]= @LastRatePerMT" +
            " ,[currentRatePerMT]= @CurrentRatePerMT" +
            " ,[lastRateCalcDesc]= @LastRateCalcDesc" +
            " ,[currentRateCalcDesc] = @CurrentRateCalcDesc" +
             " ,[lastCdAplAmt] = @lastCdAplAmt " +
    " ,[currentCdAplAmt] = @currentCdAplAmt " +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdConfirmHistory", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.IdConfirmHistory;
            cmdUpdate.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.LoadingSlipExtId;
            cmdUpdate.Parameters.Add("@LastConfirmationStatus", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.LastConfirmationStatus;
            cmdUpdate.Parameters.Add("@CurrentConfirmationStatus", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.CurrentConfirmationStatus;
            cmdUpdate.Parameters.Add("@ParityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.ParityDtlId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipExtHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@LastRatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtHistoryTO.LastRatePerMT;
            cmdUpdate.Parameters.Add("@CurrentRatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtHistoryTO.CurrentRatePerMT;
            cmdUpdate.Parameters.Add("@LastRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.LastRateCalcDesc);
            cmdUpdate.Parameters.Add("@CurrentRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.CurrentRateCalcDesc);
            cmdUpdate.Parameters.Add("@lastCdAplAmt", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtHistoryTO.LastCdAplAmt;
            cmdUpdate.Parameters.Add("@currentCdAplAmt", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtHistoryTO.CurrentCdAplAmt;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idConfirmHistory, cmdDelete);
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

        public static int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idConfirmHistory, cmdDelete);
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

        public static int DeleteTempLoadingSlipExtHistoryTOList(Int32 IdLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommandList(IdLoadingSlipExt, cmdDelete);
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
        
        public static int ExecuteDeletionCommand(Int32 idConfirmHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipExtHistory] " +
            " WHERE idConfirmHistory = " + idConfirmHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idConfirmHistory", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.IdConfirmHistory;
            return cmdDelete.ExecuteNonQuery();
        }

        public static int ExecuteDeletionCommandList(Int32 IdLoadingSlipExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipExtHistory] " +
            " WHERE loadingSlipExtId = " + IdLoadingSlipExt + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idConfirmHistory", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.IdConfirmHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
