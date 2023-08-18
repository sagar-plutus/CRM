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
    public class TblVisitIssueDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitIssueDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllTblVisitIssueDetails()
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

        public static DataTable SelectTblVisitIssueDetails(Int32 idIssue)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idIssue = " + idIssue + " ";
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

        public static DataTable SelectAllTblVisitIssueDetails(SqlConnection conn, SqlTransaction tran)
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

        public static List<TblVisitIssueDetailsTO> SelectVisitIssueDetailsList(Int32 visitId)
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

                SqlDataReader visitIssueDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitIssueDetailsTO> visitIssueDetailsTOList = ConvertDTToList(visitIssueDetailsDT);
                if (visitIssueDetailsTOList != null && visitIssueDetailsTOList.Count > 0)
                    return visitIssueDetailsTOList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitIssueDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblVisitIssueDetailsTO> ConvertDTToList(SqlDataReader visitIssueDetailsDT)
        {
            List<TblVisitIssueDetailsTO> visitIssueDetailsTOList = new List<TblVisitIssueDetailsTO>();
            if (visitIssueDetailsDT != null)
            {
                while (visitIssueDetailsDT.Read())
                {
                    TblVisitIssueDetailsTO visitIssueDetailsTONew = new TblVisitIssueDetailsTO();
                    if (visitIssueDetailsDT["idIssue"] != DBNull.Value)
                        visitIssueDetailsTONew.IdIssue = Convert.ToInt32(visitIssueDetailsDT["idIssue"]);
                    if (visitIssueDetailsDT["visitId"] != DBNull.Value)
                        visitIssueDetailsTONew.VisitId = Convert.ToInt32(visitIssueDetailsDT["visitId"]);
                    if (visitIssueDetailsDT["issueTypeId"] != DBNull.Value)
                        visitIssueDetailsTONew.IssueTypeId = Convert.ToInt32(visitIssueDetailsDT["issueTypeId"]);
                    if (visitIssueDetailsDT["issueReasonId"] != DBNull.Value)
                        visitIssueDetailsTONew.IssueReasonId = Convert.ToInt32(visitIssueDetailsDT["issueReasonId"]);
                    if (visitIssueDetailsDT["issueComment"] != DBNull.Value)
                        visitIssueDetailsTONew.IssueComment = visitIssueDetailsDT["issueComment"].ToString();
                    if (visitIssueDetailsDT["issueImage"] != DBNull.Value)
                        visitIssueDetailsTONew.IssueImage = visitIssueDetailsDT["issueImage"].ToString();
                    if (visitIssueDetailsDT["createdBy"] != DBNull.Value)
                        visitIssueDetailsTONew.CreatedBy = Convert.ToInt32(visitIssueDetailsDT["createdBy"]);
                    if (visitIssueDetailsDT["createdOn"] != DBNull.Value)
                        visitIssueDetailsTONew.CreatedOn = Convert.ToDateTime(visitIssueDetailsDT["createdOn"]);
                    if (visitIssueDetailsDT["updatedBy"] != DBNull.Value)
                        visitIssueDetailsTONew.UpdatedBy = Convert.ToInt32(visitIssueDetailsDT["updatedBy"]);
                    if (visitIssueDetailsDT["updatedOn"] != DBNull.Value)
                        visitIssueDetailsTONew.UpdatedOn = Convert.ToDateTime(visitIssueDetailsDT["updatedOn"]);

                    visitIssueDetailsTOList.Add(visitIssueDetailsTONew);
                }
            }
            return visitIssueDetailsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitIssueDetailsTO, cmdInsert);
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

        public static int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitIssueDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitIssueDetails");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitIssueDetails]( " +
            "  [visitId]" +
            " ,[issueTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[issueImage]" +
            " ,[issueReasonId]" +
            " ,[issueComment]" +
            " )" +
            " VALUES (" +
            "  @VisitId " +
            " ,@IssueTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@IssueImage " +
            " ,@issueReasonId " +
            " ,@IssueComment " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdIssue", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.IdIssue;
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.VisitId;
            cmdInsert.Parameters.Add("@IssueTypeId", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.IssueTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitIssueDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IssueImage", System.Data.SqlDbType.NText).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.IssueImage);
            cmdInsert.Parameters.Add("@issueReasonId", System.Data.SqlDbType.NVarChar).Value = tblVisitIssueDetailsTO.IssueReasonId;            
            cmdInsert.Parameters.Add("@IssueComment", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.IssueComment);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitIssueDetailsTO, cmdUpdate);
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

        public static int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitIssueDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblVisitIssueDetails");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlCommand cmdUpdate)
        {

           String sqlQuery = @" UPDATE [tblVisitIssueDetails] SET " +

            " [issueTypeId]= @IssueTypeId" +
            //" ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            //" ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[issueImage]= @IssueImage" +
            " ,[issueReasonId]= @issueReasonId" +
            " ,[issueComment] = @IssueComment" +
            //" WHERE [idIssue]=@IdIssue AND [visitId]=@VisitId ";
            " WHERE [issueTypeId]=@IssueTypeId AND [visitId]=@VisitId ";


            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdIssue", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.IdIssue;
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.VisitId;
            cmdUpdate.Parameters.Add("@IssueTypeId", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.IssueTypeId;
            //cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.UpdatedBy);
            //cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitIssueDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@IssueImage", System.Data.SqlDbType.NText).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.IssueImage);
            cmdUpdate.Parameters.Add("@issueReasonId", System.Data.SqlDbType.NVarChar).Value = tblVisitIssueDetailsTO.IssueReasonId;
            cmdUpdate.Parameters.Add("@IssueComment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitIssueDetailsTO.IssueComment);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblVisitIssueDetails(Int32 idIssue)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idIssue, cmdDelete);
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
        #region

        // Added by vinod Thorat  on Dated : 22/11/2017

        public static int DeleteTblVisitIssueDetailsWithType(Int32 VisitId, Int32 issueTypeId, Int32 issueReasonId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommandForIssueType(VisitId, issueTypeId, issueReasonId, cmdDelete);
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

        #endregion

        public static int DeleteTblVisitIssueDetails(Int32 idIssue, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idIssue, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idIssue, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitIssueDetails] " +
            " WHERE idIssue = " + idIssue + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idIssue", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.IdIssue;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

        #region Executing the Deleting Issue Type code 


        public static int ExecuteDeletionCommandForIssueType(Int32 visitId, Int32 issueTypeId, Int32 issueReasonId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitIssueDetails] " +
            " WHERE visitId = " + visitId + " and issueTypeId=" + issueTypeId + " and issueReasonId=" + issueReasonId;
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idIssue", System.Data.SqlDbType.Int).Value = tblVisitIssueDetailsTO.IdIssue;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

    }
}
