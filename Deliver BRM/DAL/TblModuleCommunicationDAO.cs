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
    public class TblModuleCommunicationDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblModuleCommunication]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblModuleCommunicationTO> SelectAllTblModuleCommunication()
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
                List<TblModuleCommunicationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
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

        public static List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationById(Int32 srcModuleId, Int32 srcTxnId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE srcModuleId = " + srcModuleId + " AND srcTxnId = " + srcTxnId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleCommunicationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
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

        public static TblModuleCommunicationTO SelectTblModuleCommunication(Int32 idModuleCommunication)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idModuleCommunication = " + idModuleCommunication +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleCommunicationTO> list = ConvertDTToList(reader);
                if (reader != null)
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

        public static List<TblModuleCommunicationTO> SelectAllTblModuleCommunication(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleCommunicationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        #endregion
        
        #region Insertion
        public static int InsertTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO)
        {       
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();

            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblModuleCommunicationTO, cmdInsert);
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

        public static int InsertTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblModuleCommunicationTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblModuleCommunicationTO tblModuleCommunicationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblModuleCommunication]( " + 
            " [srcModuleId]" +
            " ,[srcTxnId]" +
            " ,[srcTxnTypeId]" +
            " ,[destModuleId]" +
            " ,[destTxnId]" +
            " ,[destTxnTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[srcDesc]" +
            " )" +
" VALUES (" +
            " @SrcModuleId " +
            " ,@SrcTxnId " +
            " ,@SrcTxnTypeId " +
            " ,@DestModuleId " +
            " ,@DestTxnId " +
            " ,@DestTxnTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@SrcDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@SrcModuleId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.SrcModuleId;
            cmdInsert.Parameters.Add("@SrcTxnId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.SrcTxnId;
            cmdInsert.Parameters.Add("@SrcTxnTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModuleCommunicationTO.SrcTxnTypeId);
            cmdInsert.Parameters.Add("@DestModuleId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.DestModuleId;
            cmdInsert.Parameters.Add("@DestTxnId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.DestTxnId;
            cmdInsert.Parameters.Add("@DestTxnTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModuleCommunicationTO.DestTxnTypeId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModuleCommunicationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblModuleCommunicationTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblModuleCommunicationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@SrcDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblModuleCommunicationTO.SrcDesc);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblModuleCommunicationTO, cmdUpdate);
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

        public static int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblModuleCommunicationTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblModuleCommunicationTO tblModuleCommunicationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblModuleCommunication] SET " + 
            "  [idModuleCommunication] = @IdModuleCommunication" +
            " ,[srcModuleId]= @SrcModuleId" +
            " ,[srcTxnId]= @SrcTxnId" +
            " ,[srcTxnTypeId]= @SrcTxnTypeId" +
            " ,[destModuleId]= @DestModuleId" +
            " ,[destTxnId]= @DestTxnId" +
            " ,[destTxnTypeId]= @DestTxnTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[srcDesc] = @SrcDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdModuleCommunication", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.IdModuleCommunication;
            cmdUpdate.Parameters.Add("@SrcModuleId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.SrcModuleId;
            cmdUpdate.Parameters.Add("@SrcTxnId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.SrcTxnId;
            cmdUpdate.Parameters.Add("@SrcTxnTypeId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.SrcTxnTypeId;
            cmdUpdate.Parameters.Add("@DestModuleId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.DestModuleId;
            cmdUpdate.Parameters.Add("@DestTxnId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.DestTxnId;
            cmdUpdate.Parameters.Add("@DestTxnTypeId", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.DestTxnTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblModuleCommunicationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblModuleCommunicationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@SrcDesc", System.Data.SqlDbType.NVarChar).Value = tblModuleCommunicationTO.SrcDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblModuleCommunication(Int32 idModuleCommunication)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idModuleCommunication, cmdDelete);
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

        public static int DeleteTblModuleCommunication(Int32 idModuleCommunication, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idModuleCommunication, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idModuleCommunication, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblModuleCommunication] " +
            " WHERE idModuleCommunication = " + idModuleCommunication +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idModuleCommunication", System.Data.SqlDbType.Int).Value = tblModuleCommunicationTO.IdModuleCommunication;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblModuleCommunicationTO> ConvertDTToList(SqlDataReader tblModuleCommunicationTODT)
        {
            List<TblModuleCommunicationTO> tblModuleCommunicationTOList = new List<TblModuleCommunicationTO>();
            if (tblModuleCommunicationTODT != null)
            {
                while (tblModuleCommunicationTODT.Read())
                { 
                    TblModuleCommunicationTO tblModuleCommunicationTONew = new TblModuleCommunicationTO();
                    if (tblModuleCommunicationTODT["idModuleCommunication"] != DBNull.Value)
                        tblModuleCommunicationTONew.IdModuleCommunication = Convert.ToInt32(tblModuleCommunicationTODT["idModuleCommunication"].ToString());
                    if (tblModuleCommunicationTODT["srcModuleId"] != DBNull.Value)
                        tblModuleCommunicationTONew.SrcModuleId = Convert.ToInt32(tblModuleCommunicationTODT["srcModuleId"].ToString());
                    if (tblModuleCommunicationTODT["srcTxnId"] != DBNull.Value)
                        tblModuleCommunicationTONew.SrcTxnId = Convert.ToInt32(tblModuleCommunicationTODT["srcTxnId"].ToString());
                    if (tblModuleCommunicationTODT["srcTxnTypeId"] != DBNull.Value)
                        tblModuleCommunicationTONew.SrcTxnTypeId = Convert.ToInt32(tblModuleCommunicationTODT["srcTxnTypeId"].ToString());
                    if (tblModuleCommunicationTODT["destModuleId"] != DBNull.Value)
                        tblModuleCommunicationTONew.DestModuleId = Convert.ToInt32(tblModuleCommunicationTODT["destModuleId"].ToString());
                    if (tblModuleCommunicationTODT["destTxnId"] != DBNull.Value)
                        tblModuleCommunicationTONew.DestTxnId = Convert.ToInt32(tblModuleCommunicationTODT["destTxnId"].ToString());
                    if (tblModuleCommunicationTODT["destTxnTypeId"] != DBNull.Value)
                        tblModuleCommunicationTONew.DestTxnTypeId = Convert.ToInt32(tblModuleCommunicationTODT["destTxnTypeId"].ToString());
                    if (tblModuleCommunicationTODT["createdBy"] != DBNull.Value)
                        tblModuleCommunicationTONew.CreatedBy = Convert.ToInt32(tblModuleCommunicationTODT["createdBy"].ToString());
                    if (tblModuleCommunicationTODT["updatedBy"] != DBNull.Value)
                        tblModuleCommunicationTONew.UpdatedBy = Convert.ToInt32(tblModuleCommunicationTODT["updatedBy"].ToString());
                    if (tblModuleCommunicationTODT["createdOn"] != DBNull.Value)
                        tblModuleCommunicationTONew.CreatedOn = Convert.ToDateTime(tblModuleCommunicationTODT["createdOn"].ToString());
                    if (tblModuleCommunicationTODT["updatedOn"] != DBNull.Value)
                        tblModuleCommunicationTONew.UpdatedOn = Convert.ToDateTime(tblModuleCommunicationTODT["updatedOn"].ToString());
                    if (tblModuleCommunicationTODT["srcDesc"] != DBNull.Value)
                        tblModuleCommunicationTONew.SrcDesc = Convert.ToString(tblModuleCommunicationTODT["srcDesc"].ToString());
                    tblModuleCommunicationTOList.Add(tblModuleCommunicationTONew);
                }
            }
            return tblModuleCommunicationTOList;
        }

    }
}
