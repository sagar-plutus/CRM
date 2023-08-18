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
    public class TblSessionDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSession]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static TblSessionTO SelectAllTblSession()
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
                List<TblSessionTO> TblSessionList = ConvertDTToList(dr);
                if (TblSessionList != null && TblSessionList.Count == 1)
                {
                    return TblSessionList[0];
                }
                else
                    return null;
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


        public static List<TblSessionTO> SelectAllTblSessionData()
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
                List<TblSessionTO> TblSessionList = ConvertDTToList(dr);
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


        public static TblSessionTO SelectTblSession(int idsession)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idsession = " + idsession +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSessionTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list[0];
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
        public static TblSessionTO getSessionAllreadyExist(Int32 CreateUserId, Int32 ConversionUserId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE createUserId=" + CreateUserId + "AND conversionUserId=" + ConversionUserId + "AND isEndSession = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSessionTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list[0];
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

        public static TblSessionTO SelectAllTblSession(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSessionTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                {
                    return list[0];
                }
                else
                    return null;
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
        public static int InsertTblSession(TblSessionTO tblSessionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSessionTO, cmdInsert);
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

        public static int InsertTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSessionTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblSessionTO tblSessionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSession]( " + 
            "  [startTime]" +
            " ,[endTime]" +
            " ,[duration]" +
            " ,[createUserId]" +
            " ,[isActive]" +
             " ,[isEndSession]" +
             " ,[transactionType]" +
            " ,[moduleId]" +
            " ,[refId]" +
            " )" +
" VALUES (" +
            "  @StartTime " +
            " ,@EndTime " +
            " ,@Duration " +
            " ,@CreateUserId " +
            " ,@IsActive " +
            " ,@IsEndSession " +
            " ,@TransactionType " +
            " ,@ModuleId " +
            " ,@RefId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime).Value = tblSessionTO.StartTime;
            cmdInsert.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime).Value = tblSessionTO.EndTime;
           // cmdInsert.Parameters.Add("@Idsession", System.Data.SqlDbType.Int).Value = tblSessionTO.Idsession;
            cmdInsert.Parameters.Add("@Duration", System.Data.SqlDbType.Int).Value = tblSessionTO.Duration;
            cmdInsert.Parameters.Add("@CreateUserId", System.Data.SqlDbType.Int).Value = tblSessionTO.CreateUserId;
           // cmdInsert.Parameters.Add("@ConversionUserId", System.Data.SqlDbType.Int).Value = tblSessionTO.ConversionUserId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSessionTO.IsActive;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblSessionTO.ModuleId;
            cmdInsert.Parameters.Add("@RefId", System.Data.SqlDbType.Int).Value = tblSessionTO.RefId;
            cmdInsert.Parameters.Add("@TransactionType", System.Data.SqlDbType.NVarChar).Value = tblSessionTO.TransactionType;
            cmdInsert.Parameters.Add("@IsEndSession", System.Data.SqlDbType.Int).Value = tblSessionTO.IsEndSession;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblSessionTO.Idsession = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblSession(TblSessionTO tblSessionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSessionTO, cmdUpdate);
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

        public static int UpdateTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSessionTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblSessionTO tblSessionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSession] SET " +
            "  [startTime] = @StartTime" +
            " ,[endTime]= @EndTime" +
            " ,[duration]= @Duration" +
            " ,[createUserId]= @CreateUserId" +
            " ,[isActive] = @IsActive" +
            " ,[isEndSession] = @IsEndSession" +
            " WHERE [idsession] = @Idsession";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime).Value = tblSessionTO.StartTime;
            cmdUpdate.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime).Value = tblSessionTO.EndTime;
            cmdUpdate.Parameters.Add("@Idsession", System.Data.SqlDbType.Int).Value = tblSessionTO.Idsession;
            cmdUpdate.Parameters.Add("@Duration", System.Data.SqlDbType.Int).Value = tblSessionTO.Duration;
            cmdUpdate.Parameters.Add("@CreateUserId", System.Data.SqlDbType.Int).Value = tblSessionTO.CreateUserId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSessionTO.IsActive;
            cmdUpdate.Parameters.Add("@IsEndSession", System.Data.SqlDbType.Int).Value = tblSessionTO.IsEndSession;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblSession(int idsession)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idsession, cmdDelete);
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

        public static int DeleteTblSession()
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
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }
 

        public static int DeleteTblSession(int idsession, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idsession, cmdDelete);
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

        public static int ExecuteDeletionCommand(int idsession, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSession] " +
            " WHERE idsession = " + idsession +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idsession", System.Data.SqlDbType.Int).Value = tblSessionTO.Idsession;
            return cmdDelete.ExecuteNonQuery();
        }

        public static int ExecuteDeletionCommand(SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "TRUNCATE TABLE [tblSession]";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idsession", System.Data.SqlDbType.Int).Value = tblSessionTO.Idsession;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblSessionTO> ConvertDTToList(SqlDataReader tblSessionTODT)
        {
            List<TblSessionTO> tblSessionTOList = new List<TblSessionTO>();
            if (tblSessionTODT != null)
            {
                while (tblSessionTODT.Read())
                {
                    TblSessionTO tblSessionTONew = new TblSessionTO();
                    if (tblSessionTODT["startTime"] != DBNull.Value)
                        tblSessionTONew.StartTime = Convert.ToDateTime(tblSessionTODT["startTime"].ToString());
                    if (tblSessionTODT["endTime"] != DBNull.Value)
                        tblSessionTONew.EndTime = Convert.ToDateTime(tblSessionTODT["endTime"].ToString());
                    if (tblSessionTODT["idsession"] != DBNull.Value)
                        tblSessionTONew.Idsession = Convert.ToInt32(tblSessionTODT["idsession"].ToString());
                    if (tblSessionTODT["duration"] != DBNull.Value)
                        tblSessionTONew.Duration = Convert.ToInt32(tblSessionTODT["duration"].ToString());
                    if (tblSessionTODT["createUserId"] != DBNull.Value)
                        tblSessionTONew.CreateUserId = Convert.ToInt32(tblSessionTODT["createUserId"].ToString());
                    if (tblSessionTODT["isActive"] != DBNull.Value)
                        tblSessionTONew.IsActive = Convert.ToInt32(tblSessionTODT["isActive"].ToString());
                    if (tblSessionTODT["isEndSession"] != DBNull.Value)
                        tblSessionTONew.IsEndSession = Convert.ToInt32(tblSessionTODT["isEndSession"].ToString());
                    tblSessionTOList.Add(tblSessionTONew);
                }
            }
            return tblSessionTOList;
        }

    }
}
