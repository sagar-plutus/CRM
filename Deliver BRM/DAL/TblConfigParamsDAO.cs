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
    public class TblConfigParamsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblConfigParams]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblConfigParamsTO> SelectAllTblConfigParams()
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
                List<TblConfigParamsTO> list = ConvertDTToList(sqlReader);
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

        public static TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE 1=1 "
                    + "and configParamName = '" + configParamName + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToList(sqlReader);
                TblConfigParamsTO tblConfigParamsTO = new TblConfigParamsTO();
                if (list.Count > 0)
                {
                    tblConfigParamsTO = list[0];
                }
                sqlReader.Dispose();
                return tblConfigParamsTO;
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

        public static TblConfigParamsTO SelectTblConfigParams(Int32 idConfigParam)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idConfigParam = " + idConfigParam +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public static TblConfigParamsTO SelectTblConfigParams(String configParamName,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE configParamName ='" + configParamName + "'";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
                cmdSelect.Dispose();
            }
        }

        public static List<TblConfigParamsTO> ConvertDTToList(SqlDataReader tblConfigParamsTODT)
        {
            List<TblConfigParamsTO> tblConfigParamsTOList = new List<TblConfigParamsTO>();
            if (tblConfigParamsTODT != null)
            {
                while (tblConfigParamsTODT.Read())
                {
                    TblConfigParamsTO tblConfigParamsTONew = new TblConfigParamsTO();
                    if (tblConfigParamsTODT["idConfigParam"] != DBNull.Value)
                        tblConfigParamsTONew.IdConfigParam = Convert.ToInt32(tblConfigParamsTODT["idConfigParam"].ToString());
                    if (tblConfigParamsTODT["configParamValType"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamValType = Convert.ToInt32(tblConfigParamsTODT["configParamValType"].ToString());
                    if (tblConfigParamsTODT["createdOn"] != DBNull.Value)
                        tblConfigParamsTONew.CreatedOn = Convert.ToDateTime(tblConfigParamsTODT["createdOn"].ToString());
                    if (tblConfigParamsTODT["configParamName"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamName = Convert.ToString(tblConfigParamsTODT["configParamName"].ToString());
                    if (tblConfigParamsTODT["configParamVal"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamVal = Convert.ToString(tblConfigParamsTODT["configParamVal"].ToString());
                    tblConfigParamsTOList.Add(tblConfigParamsTONew);
                }
            }
            return tblConfigParamsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblConfigParamsTO, cmdInsert);
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

        public static int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblConfigParamsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblConfigParamsTO tblConfigParamsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblConfigParams]( " + 
            "  [idConfigParam]" +
            " ,[configParamValType]" +
            " ,[createdOn]" +
            " ,[configParamName]" +
            " ,[configParamVal]" +
            " )" +
" VALUES (" +
            "  @IdConfigParam " +
            " ,@ConfigParamValType " +
            " ,@CreatedOn " +
            " ,@ConfigParamName " +
            " ,@ConfigParamVal " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdConfigParam", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.IdConfigParam;
            cmdInsert.Parameters.Add("@ConfigParamValType", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.ConfigParamValType;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblConfigParamsTO.CreatedOn;
            cmdInsert.Parameters.Add("@ConfigParamName", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamName;
            cmdInsert.Parameters.Add("@ConfigParamVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamVal;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblConfigParamsTO, cmdUpdate);
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

        public static int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblConfigParamsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblConfigParamsTO tblConfigParamsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblConfigParams] SET " + 
                                "  [configParamValType]= @ConfigParamValType" +
                                " ,[createdOn]= @CreatedOn" +
                                " ,[configParamName]= @ConfigParamName" +
                                " ,[configParamVal] = @ConfigParamVal" +
                                " WHERE  [idConfigParam] = @IdConfigParam"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdConfigParam", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.IdConfigParam;
            cmdUpdate.Parameters.Add("@ConfigParamValType", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.ConfigParamValType;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime; 
            cmdUpdate.Parameters.Add("@ConfigParamName", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamName;
            cmdUpdate.Parameters.Add("@ConfigParamVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamVal;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblConfigParams(Int32 idConfigParam)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idConfigParam, cmdDelete);
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

        public static int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idConfigParam, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idConfigParam, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblConfigParams] " +
            " WHERE idConfigParam = " + idConfigParam +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idConfigParam", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.IdConfigParam;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
