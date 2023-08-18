using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class TblUserPwdHistoryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblUserPwdHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistory()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserPwdHistoryTO> list = ConvertDTToList(sqlReader);
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

        public static TblUserPwdHistoryTO SelectTblUserPwdHistory(Int32 idUserPwdHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idUserPwdHistory = " + idUserPwdHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserPwdHistoryTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserPwdHistoryTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblUserPwdHistoryTO> ConvertDTToList(SqlDataReader tblUserPwdHistoryTODT)
        {
            List<TblUserPwdHistoryTO> tblUserPwdHistoryTOList = new List<TblUserPwdHistoryTO>();
            if (tblUserPwdHistoryTODT != null)
            {
                while (tblUserPwdHistoryTODT.Read())
                {
                    TblUserPwdHistoryTO tblUserPwdHistoryTONew = new TblUserPwdHistoryTO();
                    if (tblUserPwdHistoryTODT["idUserPwdHistory"] != DBNull.Value)
                        tblUserPwdHistoryTONew.IdUserPwdHistory = Convert.ToInt32(tblUserPwdHistoryTODT["idUserPwdHistory"].ToString());
                    if (tblUserPwdHistoryTODT["userId"] != DBNull.Value)
                        tblUserPwdHistoryTONew.UserId = Convert.ToInt32(tblUserPwdHistoryTODT["userId"].ToString());
                    if (tblUserPwdHistoryTODT["createdBy"] != DBNull.Value)
                        tblUserPwdHistoryTONew.CreatedBy = Convert.ToInt32(tblUserPwdHistoryTODT["createdBy"].ToString());
                    if (tblUserPwdHistoryTODT["createdOn"] != DBNull.Value)
                        tblUserPwdHistoryTONew.CreatedOn = Convert.ToDateTime(tblUserPwdHistoryTODT["createdOn"].ToString());
                    if (tblUserPwdHistoryTODT["newPwd"] != DBNull.Value)
                        tblUserPwdHistoryTONew.NewPwd = Convert.ToString(tblUserPwdHistoryTODT["newPwd"].ToString());
                    if (tblUserPwdHistoryTODT["oldPwd"] != DBNull.Value)
                        tblUserPwdHistoryTONew.OldPwd = Convert.ToString(tblUserPwdHistoryTODT["oldPwd"].ToString());
                    tblUserPwdHistoryTOList.Add(tblUserPwdHistoryTONew);
                }
            }
            return tblUserPwdHistoryTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserPwdHistoryTO, cmdInsert);
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

        public static int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserPwdHistoryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserPwdHistory]( " + 
            //"  [idUserPwdHistory]" +
            " [userId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[newPwd]" +
            " ,[oldPwd]" +
            " )" +
" VALUES (" +
            //"  @IdUserPwdHistory " +
            " @UserId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@NewPwd " +
            " ,@OldPwd " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdUserPwdHistory", System.Data.SqlDbType.Int).Value = tblUserPwdHistoryTO.IdUserPwdHistory;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserPwdHistoryTO.UserId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserPwdHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserPwdHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@NewPwd", System.Data.SqlDbType.NVarChar).Value = tblUserPwdHistoryTO.NewPwd;
            cmdInsert.Parameters.Add("@OldPwd", System.Data.SqlDbType.NVarChar).Value = tblUserPwdHistoryTO.OldPwd;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = SalesTrackerAPI.StaticStuff.Constants.IdentityColumnQuery;
                tblUserPwdHistoryTO.IdUserPwdHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserPwdHistoryTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserPwdHistoryTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserPwdHistory] SET " + 
            "  [idUserPwdHistory] = @IdUserPwdHistory" +
            " ,[userId]= @UserId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[newPwd]= @NewPwd" +
            " ,[oldPwd] = @OldPwd" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUserPwdHistory", System.Data.SqlDbType.Int).Value = tblUserPwdHistoryTO.IdUserPwdHistory;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserPwdHistoryTO.UserId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserPwdHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserPwdHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@NewPwd", System.Data.SqlDbType.NVarChar).Value = tblUserPwdHistoryTO.NewPwd;
            cmdUpdate.Parameters.Add("@OldPwd", System.Data.SqlDbType.NVarChar).Value = tblUserPwdHistoryTO.OldPwd;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblUserPwdHistory(Int32 idUserPwdHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUserPwdHistory, cmdDelete);
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

        public static int DeleteTblUserPwdHistory(Int32 idUserPwdHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUserPwdHistory, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idUserPwdHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUserPwdHistory] " +
            " WHERE idUserPwdHistory = " + idUserPwdHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUserPwdHistory", System.Data.SqlDbType.Int).Value = tblUserPwdHistoryTO.IdUserPwdHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
