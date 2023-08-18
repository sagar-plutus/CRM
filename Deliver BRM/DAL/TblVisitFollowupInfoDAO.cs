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
    public class TblVisitFollowupInfoDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitFollowupInfo]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllTblVisitFollowupInfo()
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

        public static DataTable SelectTblVisitFollowupInfo(Int32 idProjectFollowUpInfo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProjectFollowUpInfo = " + idProjectFollowUpInfo + " ";
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

        public static DataTable SelectAllTblVisitFollowupInfo(SqlConnection conn, SqlTransaction tran)
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

        public static TblVisitFollowupInfoTO SelectVisitFollowupInfo(Int32 visitId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE visitId = " + visitId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitFollowupInfoDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitFollowupInfoTO> visitFollowupInfoTOList = ConvertDTToList(visitFollowupInfoDT);
                if (visitFollowupInfoTOList != null)
                    return visitFollowupInfoTOList[0];
                else
                return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitFollowupInfo");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblVisitFollowupInfoTO> ConvertDTToList(SqlDataReader visitFollowupInfoDT)
        {
            List<TblVisitFollowupInfoTO> visitDetailsTOList = new List<TblVisitFollowupInfoTO>();
            if (visitFollowupInfoDT != null)
            {
                while (visitFollowupInfoDT.Read())
                {
                    TblVisitFollowupInfoTO visitFollowupInfoTONew = new TblVisitFollowupInfoTO();
                    if (visitFollowupInfoDT["idVisitFollowUpInfo"] != DBNull.Value)
                        visitFollowupInfoTONew.IdProjectFollowUpInfo = Convert.ToInt32(visitFollowupInfoDT["idVisitFollowUpInfo"].ToString());
                    if (visitFollowupInfoDT["visitId"] != DBNull.Value)
                        visitFollowupInfoTONew.VisitId = Convert.ToInt32(visitFollowupInfoDT["visitId"]);
                    if (visitFollowupInfoDT["shareInfoToWhom"] != DBNull.Value)
                        visitFollowupInfoTONew.ShareInfoToWhom = Convert.ToInt32(visitFollowupInfoDT["shareInfoToWhom"]);
                    if (visitFollowupInfoDT["shareInfoToWhomId"] != DBNull.Value)
                        visitFollowupInfoTONew.ShareInfoToWhomId = Convert.ToInt32(visitFollowupInfoDT["shareInfoToWhomId"]);
                    if (visitFollowupInfoDT["shareInfoOn"] != DBNull.Value)
                        visitFollowupInfoTONew.ShareInfoOn = Convert.ToDateTime(visitFollowupInfoDT["shareInfoOn"]);
                    if (visitFollowupInfoDT["callBySelfToWhom"] != DBNull.Value)
                        visitFollowupInfoTONew.CallBySelfToWhom = Convert.ToInt32(visitFollowupInfoDT["callBySelfToWhom"]);

                    if (visitFollowupInfoDT["callBySelfToWhomId"] != DBNull.Value)
                        visitFollowupInfoTONew.CallBySelfToWhomId = Convert.ToInt32(visitFollowupInfoDT["callBySelfToWhomId"]);
                    if (visitFollowupInfoDT["callBySelfOn"] != DBNull.Value)
                        visitFollowupInfoTONew.CallBySelfOn = Convert.ToDateTime(visitFollowupInfoDT["callBySelfOn"]);

                    if (visitFollowupInfoDT["arrangeVisitOf"] != DBNull.Value)
                        visitFollowupInfoTONew.ArrangeVisitOf = Convert.ToInt32(visitFollowupInfoDT["arrangeVisitOf"]);
                    if (visitFollowupInfoDT["arrangeVisitTo"] != DBNull.Value)
                        visitFollowupInfoTONew.ArrangeVisitTo = Convert.ToInt32(visitFollowupInfoDT["arrangeVisitTo"]);
                    if (visitFollowupInfoDT["arrangeVisitOn"] != DBNull.Value)
                        visitFollowupInfoTONew.ArrangeVisitOn = Convert.ToDateTime(visitFollowupInfoDT["arrangeVisitOn"]);
                    if (visitFollowupInfoDT["arrangeVisitFor"] != DBNull.Value)
                        visitFollowupInfoTONew.ArrangeVisitFor = Convert.ToInt32(visitFollowupInfoDT["arrangeVisitFor"]);
                    if (visitFollowupInfoDT["arrangeVisitReminder"] != DBNull.Value)
                        visitFollowupInfoTONew.ArrangeVisitReminder = Convert.ToDateTime(visitFollowupInfoDT["arrangeVisitReminder"]);
                    if (visitFollowupInfoDT["InfluencerReminder"] != DBNull.Value)
                        visitFollowupInfoTONew.InfluencerReminder = Convert.ToDateTime(visitFollowupInfoDT["InfluencerReminder"]);
                    if (visitFollowupInfoDT["possiblityConversionId"] != DBNull.Value)
                        visitFollowupInfoTONew.PossiblityConversionId = Convert.ToInt32(visitFollowupInfoDT["possiblityConversionId"]);
                    if (visitFollowupInfoDT["possibilityQualitySatisfactionId"] != DBNull.Value)
                        visitFollowupInfoTONew.PossibilityQualitySatisfactionId = Convert.ToInt32(visitFollowupInfoDT["possibilityQualitySatisfactionId"]);
                    if (visitFollowupInfoDT["possibilityServiceSatisfactionId"] != DBNull.Value)
                        visitFollowupInfoTONew.PossibilityServiceSatisfactionId = Convert.ToInt32(visitFollowupInfoDT["possibilityServiceSatisfactionId"]);

                    if (visitFollowupInfoDT["createdBy"] != DBNull.Value)
                        visitFollowupInfoTONew.CreatedBy = Convert.ToInt32(visitFollowupInfoDT["createdBy"]);
                    if (visitFollowupInfoDT["createdOn"] != DBNull.Value)
                        visitFollowupInfoTONew.CreatedOn = Convert.ToDateTime(visitFollowupInfoDT["createdOn"]);

                    if (visitFollowupInfoDT["updatedBy"] != DBNull.Value)
                        visitFollowupInfoTONew.UpdatedBy = Convert.ToInt32(visitFollowupInfoDT["updatedBy"]);
                    if (visitFollowupInfoDT["updatedOn"] != DBNull.Value)
                        visitFollowupInfoTONew.UpdatedOn = Convert.ToDateTime(visitFollowupInfoDT["updatedOn"]);

                    visitDetailsTOList.Add(visitFollowupInfoTONew);
                }
            }
            return visitDetailsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitFollowupInfoTO, cmdInsert);
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

        public static int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitFollowupInfoTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitFollowupInfo");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitFollowupInfo]( " +
            " [visitId]" +
            " ,[shareInfoToWhom]" +
            " ,[shareInfoToWhomId]" +
            " ,[callBySelfToWhom]" +
            " ,[callBySelfToWhomId]" +
            " ,[arrangeVisitOf]" +
            " ,[arrangeVisitFor]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[shareInfoOn]" +
            " ,[callBySelfOn]" +
            " ,[arrangeVisitOn]" +
            " ,[arrangeVisitReminder]" +
            " ,[InfluencerReminder]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[possiblityConversionId]" +
            " ,[possibilityQualitySatisfactionId]" +
            " ,[possibilityServiceSatisfactionId]" +
            " ,[arrangeVisitTo]" +
            " )" +
            " VALUES (" +
            " @VisitId " +
            " ,@ShareInfoToWhom " +
            " ,@ShareInfoToWhomId " +
            " ,@CallBySelfToWhom " +
            " ,@CallBySelfToWhomId " +
            " ,@ArrangeVisitOf " +
            " ,@ArrangeVisitFor " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@ShareInfoOn " +
            " ,@CallBySelfOn " +
            " ,@ArrangeVisitOn " +
            " ,@ArrangeVisitReminder " +
            " ,@InfluencerReminder " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@PossiblityConversionId " +
            " ,@PossibilityQualitySatisfactionId " +
            " ,@PossibilityServiceSatisfactionId " +
            " ,@ArrangeVisitTo " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           // cmdInsert.Parameters.Add("@IdProjectFollowUpInfo", System.Data.SqlDbType.Int).Value = tblVisitFollowupInfoTO.IdProjectFollowUpInfo;
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitFollowupInfoTO.VisitId;
            cmdInsert.Parameters.Add("@ShareInfoToWhom", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ShareInfoToWhom);
            cmdInsert.Parameters.Add("@ShareInfoToWhomId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ShareInfoToWhomId);
            cmdInsert.Parameters.Add("@CallBySelfToWhom", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.CallBySelfToWhom);
            cmdInsert.Parameters.Add("@CallBySelfToWhomId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.CallBySelfToWhomId);
            cmdInsert.Parameters.Add("@ArrangeVisitOf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitOf);
            cmdInsert.Parameters.Add("@ArrangeVisitFor", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitFor);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitFollowupInfoTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.UpdatedBy);
            cmdInsert.Parameters.Add("@ShareInfoOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ShareInfoOn);
            cmdInsert.Parameters.Add("@CallBySelfOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.CallBySelfOn);
            cmdInsert.Parameters.Add("@ArrangeVisitOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitOn);
            cmdInsert.Parameters.Add("@ArrangeVisitReminder", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitReminder);
            cmdInsert.Parameters.Add("@InfluencerReminder", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.InfluencerReminder);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitFollowupInfoTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.UpdatedOn);
            cmdInsert.Parameters.Add("@PossiblityConversionId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.PossiblityConversionId);
            cmdInsert.Parameters.Add("@PossibilityQualitySatisfactionId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.PossibilityQualitySatisfactionId);
            cmdInsert.Parameters.Add("@PossibilityServiceSatisfactionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.PossibilityServiceSatisfactionId);
            cmdInsert.Parameters.Add("@ArrangeVisitTo", System.Data.SqlDbType.NChar).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitTo);
            //return cmdInsert.ExecuteNonQuery();

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblVisitFollowupInfoTO.IdProjectFollowUpInfo = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return -1;
        }
        #endregion

        #region Updation
        public static int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitFollowupInfoTO, cmdUpdate);
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

        public static int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitFollowupInfoTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblVisitFollowupInfo");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlCommand cmdUpdate)
        {

           String sqlQuery = @" UPDATE [tblVisitFollowupInfo] SET " +

            " [shareInfoToWhom]=@ShareInfoToWhom " +
            " ,[shareInfoToWhomId]=@ShareInfoToWhomId " +
            " ,[callBySelfToWhom]=@CallBySelfToWhom " +
            " ,[callBySelfToWhomId]=@CallBySelfToWhomId " +
            " ,[arrangeVisitOf]=@ArrangeVisitOf " +
            " ,[arrangeVisitFor]=@ArrangeVisitFor " +
            //" ,[createdBy]=@CreatedBy " +
            " ,[updatedBy]=@UpdatedBy " +
            " ,[shareInfoOn]=@ShareInfoOn " +
            " ,[callBySelfOn]=@CallBySelfOn " +
            " ,[arrangeVisitOn]=@ArrangeVisitOn " +
            " ,[arrangeVisitReminder]=@ArrangeVisitReminder " +
            " ,[InfluencerReminder]=@InfluencerReminder " +
            //" ,[createdOn]=@CreatedOn " +
            " ,[updatedOn]=@UpdatedOn " +
            " ,[possiblityConversionId]=@PossiblityConversionId " +
            " ,[possibilityQualitySatisfactionId]=@PossibilityQualitySatisfactionId " +
            " ,[possibilityServiceSatisfactionId]=@PossibilityServiceSatisfactionId " +
            " ,[arrangeVisitTo]=@ArrangeVisitTo " +
            //" WHERE [idVisitFollowUpInfo]=@IdVisitFollowUpInfo AND [visitId]=@VisitId ";
            " WHERE [visitId]=@VisitId ";


            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@idVisitFollowUpInfo", System.Data.SqlDbType.Int).Value = tblVisitFollowupInfoTO.IdProjectFollowUpInfo;
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitFollowupInfoTO.VisitId;
            cmdUpdate.Parameters.Add("@ShareInfoToWhom", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ShareInfoToWhom);
            cmdUpdate.Parameters.Add("@ShareInfoToWhomId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ShareInfoToWhomId);
            cmdUpdate.Parameters.Add("@CallBySelfToWhom", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.CallBySelfToWhom);
            cmdUpdate.Parameters.Add("@CallBySelfToWhomId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.CallBySelfToWhomId);
            cmdUpdate.Parameters.Add("@ArrangeVisitOf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitOf);
            cmdUpdate.Parameters.Add("@ArrangeVisitFor", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitFor);
            //cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitFollowupInfoTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@ShareInfoOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ShareInfoOn);
            cmdUpdate.Parameters.Add("@CallBySelfOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.CallBySelfOn);
            cmdUpdate.Parameters.Add("@ArrangeVisitOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitOn);
            cmdUpdate.Parameters.Add("@ArrangeVisitReminder", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitReminder);
            cmdUpdate.Parameters.Add("@InfluencerReminder", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.InfluencerReminder);
            //cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitFollowupInfoTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@PossiblityConversionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.PossiblityConversionId);
            cmdUpdate.Parameters.Add("@PossibilityQualitySatisfactionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.PossibilityQualitySatisfactionId);
            cmdUpdate.Parameters.Add("@PossibilityServiceSatisfactionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.PossibilityServiceSatisfactionId);
            cmdUpdate.Parameters.Add("@ArrangeVisitTo", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitFollowupInfoTO.ArrangeVisitTo);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProjectFollowUpInfo, cmdDelete);
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

        public static int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProjectFollowUpInfo, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idProjectFollowUpInfo, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitFollowupInfo] " +
            " WHERE idProjectFollowUpInfo = " + idProjectFollowUpInfo + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProjectFollowUpInfo", System.Data.SqlDbType.Int).Value = tblVisitFollowupInfoTO.IdProjectFollowUpInfo;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
