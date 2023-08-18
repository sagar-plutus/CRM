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
    public class TblSmsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSms]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblSmsTO> SelectAllTblSms()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblSmsTO SelectTblSms(Int32 idSms)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSms = " + idSms +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSmsTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblSmsTO> ConvertDTToList(SqlDataReader tblSmsTODT)
        {
            List<TblSmsTO> tblSmsTOList = new List<TblSmsTO>();
            if (tblSmsTODT != null)
            {
                while (tblSmsTODT.Read())
                {
                    TblSmsTO tblSmsTONew = new TblSmsTO();
                    if (tblSmsTODT["idSms"] != DBNull.Value)
                        tblSmsTONew.IdSms = Convert.ToInt32(tblSmsTODT["idSms"].ToString());
                    if (tblSmsTODT["alertInstanceId"] != DBNull.Value)
                        tblSmsTONew.AlertInstanceId = Convert.ToInt32(tblSmsTODT["alertInstanceId"].ToString());
                    if (tblSmsTODT["sentOn"] != DBNull.Value)
                        tblSmsTONew.SentOn = Convert.ToDateTime(tblSmsTODT["sentOn"].ToString());
                    if (tblSmsTODT["mobileNo"] != DBNull.Value)
                        tblSmsTONew.MobileNo = Convert.ToString(tblSmsTODT["mobileNo"].ToString());
                    if (tblSmsTODT["smsTxt"] != DBNull.Value)
                        tblSmsTONew.SmsTxt = Convert.ToString(tblSmsTODT["smsTxt"].ToString());
                    if (tblSmsTODT["replyTxt"] != DBNull.Value)
                        tblSmsTONew.ReplyTxt = Convert.ToString(tblSmsTODT["replyTxt"].ToString());
                    if (tblSmsTODT["sourceTxnDesc"] != DBNull.Value)
                        tblSmsTONew.SourceTxnDesc = Convert.ToString(tblSmsTODT["sourceTxnDesc"].ToString());
                    tblSmsTOList.Add(tblSmsTONew);
                }
            }
            return tblSmsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblSms(TblSmsTO tblSmsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSmsTO, cmdInsert);
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

        public static int InsertTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSmsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblSmsTO tblSmsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSms]( " + 
                            "  [alertInstanceId]" +
                            " ,[sentOn]" +
                            " ,[mobileNo]" +
                            " ,[smsTxt]" +
                            " ,[replyTxt]" +
                            " ,[sourceTxnDesc]" +
                            " )" +
                " VALUES (" +
                            "  @AlertInstanceId " +
                            " ,@SentOn " +
                            " ,@MobileNo " +
                            " ,@SmsTxt " +
                            " ,@ReplyTxt " +
                            " ,@SourceTxnDesc " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSms", System.Data.SqlDbType.Int).Value = tblSmsTO.IdSms;
            cmdInsert.Parameters.Add("@AlertInstanceId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(  tblSmsTO.AlertInstanceId);
            cmdInsert.Parameters.Add("@SentOn", System.Data.SqlDbType.DateTime).Value = tblSmsTO.SentOn;
            cmdInsert.Parameters.Add("@MobileNo", System.Data.SqlDbType.NVarChar).Value = tblSmsTO.MobileNo;
            cmdInsert.Parameters.Add("@SmsTxt", System.Data.SqlDbType.NVarChar,256).Value = tblSmsTO.SmsTxt;
            cmdInsert.Parameters.Add("@ReplyTxt", System.Data.SqlDbType.NVarChar,1000).Value = Constants.GetSqlDataValueNullForBaseValue(tblSmsTO.ReplyTxt);
            cmdInsert.Parameters.Add("@SourceTxnDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblSmsTO.SourceTxnDesc);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblSmsTO.IdSms = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblSms(TblSmsTO tblSmsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSmsTO, cmdUpdate);
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

        public static int UpdateTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSmsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblSmsTO tblSmsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSms] SET " + 
            "  [idSms] = @IdSms" +
            " ,[alertInstanceId]= @AlertInstanceId" +
            " ,[sentOn]= @SentOn" +
            " ,[mobileNo]= @MobileNo" +
            " ,[smsTxt]= @SmsTxt" +
            " ,[replyTxt]= @ReplyTxt" +
            " ,[sourceTxnDesc] = @SourceTxnDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSms", System.Data.SqlDbType.Int).Value = tblSmsTO.IdSms;
            cmdUpdate.Parameters.Add("@AlertInstanceId", System.Data.SqlDbType.Int).Value = tblSmsTO.AlertInstanceId;
            cmdUpdate.Parameters.Add("@SentOn", System.Data.SqlDbType.DateTime).Value = tblSmsTO.SentOn;
            cmdUpdate.Parameters.Add("@MobileNo", System.Data.SqlDbType.NVarChar).Value = tblSmsTO.MobileNo;
            cmdUpdate.Parameters.Add("@SmsTxt", System.Data.SqlDbType.NVarChar).Value = tblSmsTO.SmsTxt;
            cmdUpdate.Parameters.Add("@ReplyTxt", System.Data.SqlDbType.NVarChar).Value = tblSmsTO.ReplyTxt;
            cmdUpdate.Parameters.Add("@SourceTxnDesc", System.Data.SqlDbType.NVarChar).Value = tblSmsTO.SourceTxnDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblSms(Int32 idSms)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSms, cmdDelete);
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

        public static int DeleteTblSms(Int32 idSms, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSms, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idSms, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSms] " +
            " WHERE idSms = " + idSms +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSms", System.Data.SqlDbType.Int).Value = tblSmsTO.IdSms;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
