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
    public class TblConfigParamHistoryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblConfigParamHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistory()
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
                List<TblConfigParamHistoryTO> list = ConvertDTToList(reader);
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

        public static TblConfigParamHistoryTO SelectTblConfigParamHistory(Int32 idParamHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idParamHistory = " + idParamHistory + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamHistoryTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
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

        public static List<TblConfigParamHistoryTO> ConvertDTToList(SqlDataReader tblConfigParamHistoryTODT)
        {
            List<TblConfigParamHistoryTO> tblConfigParamHistoryTOList = new List<TblConfigParamHistoryTO>();
            if (tblConfigParamHistoryTODT != null)
            {
                while (tblConfigParamHistoryTODT.Read())
                {
                    TblConfigParamHistoryTO tblConfigParamHistoryTONew = new TblConfigParamHistoryTO();
                    if (tblConfigParamHistoryTODT["idParamHistory"] != DBNull.Value)
                        tblConfigParamHistoryTONew.IdParamHistory = Convert.ToInt32(tblConfigParamHistoryTODT["idParamHistory"].ToString());
                    if (tblConfigParamHistoryTODT["configParamId"] != DBNull.Value)
                        tblConfigParamHistoryTONew.ConfigParamId = Convert.ToInt32(tblConfigParamHistoryTODT["configParamId"].ToString());
                    if (tblConfigParamHistoryTODT["createdBy"] != DBNull.Value)
                        tblConfigParamHistoryTONew.CreatedBy = Convert.ToInt32(tblConfigParamHistoryTODT["createdBy"].ToString());
                    if (tblConfigParamHistoryTODT["createdOn"] != DBNull.Value)
                        tblConfigParamHistoryTONew.CreatedOn = Convert.ToDateTime(tblConfigParamHistoryTODT["createdOn"].ToString());
                    if (tblConfigParamHistoryTODT["configParamName"] != DBNull.Value)
                        tblConfigParamHistoryTONew.ConfigParamName = Convert.ToString(tblConfigParamHistoryTODT["configParamName"].ToString());
                    if (tblConfigParamHistoryTODT["configParamOldVal"] != DBNull.Value)
                        tblConfigParamHistoryTONew.ConfigParamOldVal = Convert.ToString(tblConfigParamHistoryTODT["configParamOldVal"].ToString());
                    if (tblConfigParamHistoryTODT["configParamNewVal"] != DBNull.Value)
                        tblConfigParamHistoryTONew.ConfigParamNewVal = Convert.ToString(tblConfigParamHistoryTODT["configParamNewVal"].ToString());
                    tblConfigParamHistoryTOList.Add(tblConfigParamHistoryTONew);
                }
            }
            return tblConfigParamHistoryTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblConfigParamHistoryTO, cmdInsert);
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

        public static int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblConfigParamHistoryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblConfigParamHistory]( " + 
                            "  [configParamId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[configParamName]" +
                            " ,[configParamOldVal]" +
                            " ,[configParamNewVal]" +
                            " )" +
                " VALUES (" +
                            "  @ConfigParamId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@ConfigParamName " +
                            " ,@ConfigParamOldVal " +
                            " ,@ConfigParamNewVal " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdParamHistory", System.Data.SqlDbType.Int).Value = tblConfigParamHistoryTO.IdParamHistory;
            cmdInsert.Parameters.Add("@ConfigParamId", System.Data.SqlDbType.Int).Value = tblConfigParamHistoryTO.ConfigParamId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblConfigParamHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblConfigParamHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@ConfigParamName", System.Data.SqlDbType.NVarChar).Value = tblConfigParamHistoryTO.ConfigParamName;
            cmdInsert.Parameters.Add("@ConfigParamOldVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamHistoryTO.ConfigParamOldVal;
            cmdInsert.Parameters.Add("@ConfigParamNewVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamHistoryTO.ConfigParamNewVal;
            if( cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblConfigParamHistoryTO.IdParamHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblConfigParamHistoryTO, cmdUpdate);
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

        public static int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblConfigParamHistoryTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblConfigParamHistory] SET " + 
            "  [idParamHistory] = @IdParamHistory" +
            " ,[configParamId]= @ConfigParamId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[configParamName]= @ConfigParamName" +
            " ,[configParamOldVal]= @ConfigParamOldVal" +
            " ,[configParamNewVal] = @ConfigParamNewVal" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdParamHistory", System.Data.SqlDbType.Int).Value = tblConfigParamHistoryTO.IdParamHistory;
            cmdUpdate.Parameters.Add("@ConfigParamId", System.Data.SqlDbType.Int).Value = tblConfigParamHistoryTO.ConfigParamId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblConfigParamHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblConfigParamHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ConfigParamName", System.Data.SqlDbType.NVarChar).Value = tblConfigParamHistoryTO.ConfigParamName;
            cmdUpdate.Parameters.Add("@ConfigParamOldVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamHistoryTO.ConfigParamOldVal;
            cmdUpdate.Parameters.Add("@ConfigParamNewVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamHistoryTO.ConfigParamNewVal;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblConfigParamHistory(Int32 idParamHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idParamHistory, cmdDelete);
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

        public static int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idParamHistory, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idParamHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblConfigParamHistory] " +
            " WHERE idParamHistory = " + idParamHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idParamHistory", System.Data.SqlDbType.Int).Value = tblConfigParamHistoryTO.IdParamHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
