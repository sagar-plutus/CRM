using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblFeedbackDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT feedback.* , pages.pageName,pageEle.elementDisplayName ,userDtl.userDisplayName " +
                                  " FROM tblFeedback feedback " +
                                  " LEFT JOIN tblPages pages ON pages.idPage = feedback.pageId " +
                                  " LEFT JOIN tblPageElements pageEle ON pageEle.idPageElement = feedback.pageEleId " +
                                  " LEFT JOIN tblUser userDtl ON userDtl.idUser = feedback.createdBy";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblFeedbackTO> SelectAllTblFeedback()
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
                return ConvertDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblFeedbackTO SelectTblFeedback(Int32 idFeedback)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idFeedback = " + idFeedback + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblFeedbackTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblFeedbackTO> SelectAllTblFeedback(int userId, DateTime frmDt, DateTime toDt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (userId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(feedback.createdOn AS DATE) BETWEEN @fromDate AND @toDate  ORDER BY feedback.createdOn";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE feedback.createdBy=" + userId + "AND  CAST(feedback.createdOn AS DATE) BETWEEN @fromDate AND @toDate ORDER BY feedback.createdOn";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt.ToString(Constants.AzureDateFormat);
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblFeedbackTO> ConvertDTToList(SqlDataReader tblFeedbackTODT)
        {
            List<TblFeedbackTO> tblFeedbackTOList = new List<TblFeedbackTO>();
            if (tblFeedbackTODT != null)
            {
                while (tblFeedbackTODT.Read())
                {
                    TblFeedbackTO tblFeedbackTONew = new TblFeedbackTO();
                    if (tblFeedbackTODT["idFeedback"] != DBNull.Value)
                        tblFeedbackTONew.IdFeedback = Convert.ToInt32(tblFeedbackTODT["idFeedback"].ToString());
                    if (tblFeedbackTODT["pageId"] != DBNull.Value)
                        tblFeedbackTONew.PageId = Convert.ToInt32(tblFeedbackTODT["pageId"].ToString());
                    if (tblFeedbackTODT["pageEleId"] != DBNull.Value)
                        tblFeedbackTONew.PageEleId = Convert.ToInt32(tblFeedbackTODT["pageEleId"].ToString());
                    if (tblFeedbackTODT["isAttended"] != DBNull.Value)
                        tblFeedbackTONew.IsAttended = Convert.ToInt32(tblFeedbackTODT["isAttended"].ToString());
                    if (tblFeedbackTODT["createdBy"] != DBNull.Value)
                        tblFeedbackTONew.CreatedBy = Convert.ToInt32(tblFeedbackTODT["createdBy"].ToString());
                    if (tblFeedbackTODT["createdOn"] != DBNull.Value)
                        tblFeedbackTONew.CreatedOn = Convert.ToDateTime(tblFeedbackTODT["createdOn"].ToString());
                    if (tblFeedbackTODT["rating"] != DBNull.Value)
                        tblFeedbackTONew.Rating = Convert.ToDouble(tblFeedbackTODT["rating"].ToString());
                    if (tblFeedbackTODT["description"] != DBNull.Value)
                        tblFeedbackTONew.Description = Convert.ToString(tblFeedbackTODT["description"].ToString());
                    if (tblFeedbackTODT["replyDesc"] != DBNull.Value)
                        tblFeedbackTONew.ReplyDesc = Convert.ToString(tblFeedbackTODT["replyDesc"].ToString());

                    if (tblFeedbackTODT["pageName"] != DBNull.Value)
                        tblFeedbackTONew.PageName = Convert.ToString(tblFeedbackTODT["pageName"].ToString());
                    if (tblFeedbackTODT["elementDisplayName"] != DBNull.Value)
                        tblFeedbackTONew.PageEleDesc = Convert.ToString(tblFeedbackTODT["elementDisplayName"].ToString());
                    if (tblFeedbackTODT["userDisplayName"] != DBNull.Value)
                        tblFeedbackTONew.CreatedByUserName = Convert.ToString(tblFeedbackTODT["userDisplayName"].ToString());

                    tblFeedbackTOList.Add(tblFeedbackTONew);
                }
            }
            return tblFeedbackTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblFeedbackTO, cmdInsert);
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

        public static int InsertTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblFeedbackTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblFeedbackTO tblFeedbackTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblFeedback]( " +
                            "  [pageId]" +
                            " ,[pageEleId]" +
                            " ,[isAttended]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[rating]" +
                            " ,[description]" +
                            " ,[replyDesc]" +
                            " )" +
                " VALUES (" +
                            "  @PageId " +
                            " ,@PageEleId " +
                            " ,@IsAttended " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@Rating " +
                            " ,@Description " +
                            " ,@ReplyDesc " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdFeedback", System.Data.SqlDbType.Int).Value = tblFeedbackTO.IdFeedback;
            cmdInsert.Parameters.Add("@PageId", System.Data.SqlDbType.Int).Value = tblFeedbackTO.PageId;
            cmdInsert.Parameters.Add("@PageEleId", System.Data.SqlDbType.Int).Value = tblFeedbackTO.PageEleId;
            cmdInsert.Parameters.Add("@IsAttended", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFeedbackTO.IsAttended);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblFeedbackTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblFeedbackTO.CreatedOn;
            cmdInsert.Parameters.Add("@Rating", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblFeedbackTO.Rating);
            cmdInsert.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblFeedbackTO.Description;
            cmdInsert.Parameters.Add("@ReplyDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblFeedbackTO.ReplyDesc);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblFeedbackTO.IdFeedback = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblFeedbackTO, cmdUpdate);
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

        public static int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblFeedbackTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblFeedbackTO tblFeedbackTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblFeedback] SET " +
                            "  [pageId]= @PageId" +
                            " ,[pageEleId]= @PageEleId" +
                            " ,[isAttended]= @IsAttended" +
                            " ,[rating]= @Rating" +
                            " ,[description]= @Description" +
                            " ,[replyDesc] = @ReplyDesc" +
                            " WHERE [idFeedback] = @IdFeedback ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdFeedback", System.Data.SqlDbType.Int).Value = tblFeedbackTO.IdFeedback;
            cmdUpdate.Parameters.Add("@PageId", System.Data.SqlDbType.Int).Value = tblFeedbackTO.PageId;
            cmdUpdate.Parameters.Add("@PageEleId", System.Data.SqlDbType.Int).Value = tblFeedbackTO.PageEleId;
            cmdUpdate.Parameters.Add("@IsAttended", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFeedbackTO.IsAttended);
            cmdUpdate.Parameters.Add("@Rating", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblFeedbackTO.Rating);
            cmdUpdate.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblFeedbackTO.Description;
            cmdUpdate.Parameters.Add("@ReplyDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblFeedbackTO.ReplyDesc);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblFeedback(Int32 idFeedback)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idFeedback, cmdDelete);
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

        public static int DeleteTblFeedback(Int32 idFeedback, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idFeedback, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idFeedback, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [tblFeedback] " +
                                    " WHERE idFeedback = " + idFeedback + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idFeedback", System.Data.SqlDbType.Int).Value = tblFeedbackTO.IdFeedback;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
