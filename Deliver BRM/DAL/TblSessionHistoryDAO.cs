using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Data.SqlClient;
using System.Data;

namespace SalesTrackerAPI.DAL
{
    public class TblSessionHistoryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSessionHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static TblSessionHistoryTO SelectAllTblSessionHistory()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSessionHistoryTO> TblSessionList = ConvertDTToList(dr);
                if (TblSessionList != null && TblSessionList.Count == 1)
                {
                    return TblSessionList[0];
                }
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
        public static List<TblSessionHistoryTO> SelectAllTblSessionHistoryData()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSessionHistoryTO> TblSessionList = ConvertDTToList(dr);
                if (TblSessionList != null && TblSessionList.Count == 1)
                {
                    return TblSessionList;
                }
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

        public static List<TblSessionHistoryTO> SelectTblSessionHistory(Int32 idSessionHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSessionHistory = " + idSessionHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSessionHistoryTO> list = ConvertDTToList(sqlReader);
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

        public static DataTable SelectAllTblSessionHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSessionHistory", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.IdSessionHistory;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        #endregion
        
        #region Insertion
        public static int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSessionHistoryTO, cmdInsert);
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

        public static int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSessionHistoryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblSessionHistoryTO tblSessionHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSessionHistory]( " + 
            " [sessionId]" +
            " ,[converionMediaType]" +
            " ,[conversionBody]" +
            " ,[otherDesc]" +
            " ,[conversionUserId]" +
            " ,[senderUserId]" +
            " ,[sendOn]" +
            " )" +
" VALUES (" +
            " @SessionId " +
            " ,@ConverionMediaType " +
            " ,@ConversionBody " +
            " ,@OtherDesc " +
            " ,@ConversionUserId " +
            " ,@SenderUserId " +
            " , @SendOn" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSessionHistory", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.IdSessionHistory;
            cmdInsert.Parameters.Add("@SessionId", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.SessionId;
            cmdInsert.Parameters.Add("@ConversionUserId", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.ConversionUserId;
            cmdInsert.Parameters.Add("@SenderUserId", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.SenderUserId;
            cmdInsert.Parameters.Add("@ConverionMediaType", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.ConverionMediaType;
            cmdInsert.Parameters.Add("@ConversionBody", System.Data.SqlDbType.NVarChar).Value = tblSessionHistoryTO.ConversionBody;
            cmdInsert.Parameters.Add("@SendOn", System.Data.SqlDbType.NVarChar).Value = tblSessionHistoryTO.SendOn;
            cmdInsert.Parameters.Add("@OtherDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblSessionHistoryTO.OtherDesc);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSessionHistoryTO, cmdUpdate);
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

        public static int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSessionHistoryTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblSessionHistoryTO tblSessionHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSessionHistory] SET " + 
            " [sessionId]= @SessionId" +
            " ,[converionMediaType]= @ConverionMediaType" +
            " ,[conversionBody]= @ConversionBody" +
            " ,[conversionUserId]= @ConversionUserId" +
            " ,[sendOn]= @SendOn" +
            " ,[senderUserId]= @SenderUserId" + 
            " ,[otherDesc] = @OtherDesc" +
            " WHERE conversionUserId =  @SenderUserId AND senderUserId = @ConversionUserId AND  sendOn = @SendOn";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IdSessionHistory", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.IdSessionHistory;
            cmdUpdate.Parameters.Add("@SessionId", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.SessionId;
            cmdUpdate.Parameters.Add("@ConversionUserId", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.ConversionUserId;
            cmdUpdate.Parameters.Add("@SenderUserId", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.SenderUserId;
            cmdUpdate.Parameters.Add("@ConverionMediaType", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.ConverionMediaType;
            cmdUpdate.Parameters.Add("@SendOn", System.Data.SqlDbType.NVarChar).Value = tblSessionHistoryTO.SendOn;
            cmdUpdate.Parameters.Add("@ConversionBody", System.Data.SqlDbType.NVarChar).Value = tblSessionHistoryTO.ConversionBody;
            cmdUpdate.Parameters.Add("@OtherDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblSessionHistoryTO.OtherDesc);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblSessionHistory(Int32 idSessionHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSessionHistory, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblSessionHistory()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(cmdDelete);
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

        public static int DeleteTblSessionHistory(Int32 idSessionHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSessionHistory, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idSessionHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSessionHistory] " +
            " WHERE idSessionHistory = " + idSessionHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSessionHistory", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.IdSessionHistory;
            return cmdDelete.ExecuteNonQuery();
        }

        public static int ExecuteDeletionCommand(SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "TRUNCATE TABLE [tblSessionHistory]";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSessionHistory", System.Data.SqlDbType.Int).Value = tblSessionHistoryTO.IdSessionHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblSessionHistoryTO> ConvertDTToList(SqlDataReader tblSessionHistoryTODT)
        {
            List<TblSessionHistoryTO> tblSessionHistoryTOList = new List<TblSessionHistoryTO>();
            if (tblSessionHistoryTODT != null)
            {
               while(tblSessionHistoryTODT.Read())
                {
                    TblSessionHistoryTO tblSessionHistoryTONew = new TblSessionHistoryTO();
                    if (tblSessionHistoryTODT["idSessionHistory"] != DBNull.Value)
                        tblSessionHistoryTONew.IdSessionHistory = Convert.ToInt32(tblSessionHistoryTODT["idSessionHistory"].ToString());
                    if (tblSessionHistoryTODT["sessionId"] != DBNull.Value)
                        tblSessionHistoryTONew.SessionId = Convert.ToInt32(tblSessionHistoryTODT["sessionId"].ToString());
                    if (tblSessionHistoryTODT["converionMediaType"] != DBNull.Value)
                        tblSessionHistoryTONew.ConverionMediaType = Convert.ToInt32(tblSessionHistoryTODT["converionMediaType"].ToString());
                    if (tblSessionHistoryTODT["conversionBody"] != DBNull.Value)
                        tblSessionHistoryTONew.ConversionBody = Convert.ToString(tblSessionHistoryTODT["conversionBody"].ToString());
                    if (tblSessionHistoryTODT["otherDesc"] != DBNull.Value)
                        tblSessionHistoryTONew.OtherDesc = Convert.ToString(tblSessionHistoryTODT["otherDesc"].ToString());
                    tblSessionHistoryTOList.Add(tblSessionHistoryTONew);
                }
            }
            return tblSessionHistoryTOList;
        }
    }
}
