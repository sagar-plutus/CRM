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
    public class TblLoadingQuotaTransferDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblLoadingQuotaTransfer]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingQuotaTransferTO> SelectAllTblLoadingQuotaTransfer()
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
                List<TblLoadingQuotaTransferTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingQuotaTransferTO SelectTblLoadingQuotaTransfer(Int32 idTransferNote)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTransferNote = " + idTransferNote + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaTransferTO> list = ConvertDTToList(reader);
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
                reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingQuotaTransferTO> ConvertDTToList(SqlDataReader tblLoadingQuotaTransferTODT)
        {
            List<TblLoadingQuotaTransferTO> tblLoadingQuotaTransferTOList = new List<TblLoadingQuotaTransferTO>();
            if (tblLoadingQuotaTransferTODT != null)
            {
                while (tblLoadingQuotaTransferTODT.Read())
                {
                    TblLoadingQuotaTransferTO tblLoadingQuotaTransferTONew = new TblLoadingQuotaTransferTO();
                    if (tblLoadingQuotaTransferTODT["idTransferNote"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.IdTransferNote = Convert.ToInt32(tblLoadingQuotaTransferTODT["idTransferNote"].ToString());
                    if (tblLoadingQuotaTransferTODT["fromCnfOrgId"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.FromCnfOrgId = Convert.ToInt32(tblLoadingQuotaTransferTODT["fromCnfOrgId"].ToString());
                    if (tblLoadingQuotaTransferTODT["toCnfOrgId"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.ToCnfOrgId = Convert.ToInt32(tblLoadingQuotaTransferTODT["toCnfOrgId"].ToString());
                    if (tblLoadingQuotaTransferTODT["againstLoadingQuotaId"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.AgainstLoadingQuotaId = Convert.ToInt32(tblLoadingQuotaTransferTODT["againstLoadingQuotaId"].ToString());
                    if (tblLoadingQuotaTransferTODT["createdBy"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.CreatedBy = Convert.ToInt32(tblLoadingQuotaTransferTODT["createdBy"].ToString());
                    if (tblLoadingQuotaTransferTODT["updatedBy"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.UpdatedBy = Convert.ToInt32(tblLoadingQuotaTransferTODT["updatedBy"].ToString());
                    if (tblLoadingQuotaTransferTODT["createdOn"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.CreatedOn = Convert.ToDateTime(tblLoadingQuotaTransferTODT["createdOn"].ToString());
                    if (tblLoadingQuotaTransferTODT["updatedOn"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.UpdatedOn = Convert.ToDateTime(tblLoadingQuotaTransferTODT["updatedOn"].ToString());
                    if (tblLoadingQuotaTransferTODT["transferQty"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.TransferQty = Convert.ToDouble(tblLoadingQuotaTransferTODT["transferQty"].ToString());
                    if (tblLoadingQuotaTransferTODT["transferDesc"] != DBNull.Value)
                        tblLoadingQuotaTransferTONew.TransferDesc = Convert.ToString(tblLoadingQuotaTransferTODT["transferDesc"].ToString());
                    tblLoadingQuotaTransferTOList.Add(tblLoadingQuotaTransferTONew);
                }
            }
            return tblLoadingQuotaTransferTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingQuotaTransferTO, cmdInsert);
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

        public static int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingQuotaTransferTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLoadingQuotaTransfer]( " + 
                            "  [fromCnfOrgId]" +
                            " ,[toCnfOrgId]" +
                            " ,[againstLoadingQuotaId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[transferQty]" +
                            " ,[transferDesc]" +
                            " )" +
                " VALUES (" +
                            "  @FromCnfOrgId " +
                            " ,@ToCnfOrgId " +
                            " ,@AgainstLoadingQuotaId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@TransferQty " +
                            " ,@TransferDesc " + 
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdTransferNote", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.IdTransferNote;
            cmdInsert.Parameters.Add("@FromCnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.FromCnfOrgId;
            cmdInsert.Parameters.Add("@ToCnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.ToCnfOrgId;
            cmdInsert.Parameters.Add("@AgainstLoadingQuotaId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.AgainstLoadingQuotaId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaTransferTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaTransferTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaTransferTO.UpdatedOn);
            cmdInsert.Parameters.Add("@TransferQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaTransferTO.TransferQty;
            cmdInsert.Parameters.Add("@TransferDesc", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaTransferTO.TransferDesc;
            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingQuotaTransferTO.IdTransferNote = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingQuotaTransferTO, cmdUpdate);
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

        public static int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingQuotaTransferTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLoadingQuotaTransfer] SET " + 
            "  [idTransferNote] = @IdTransferNote" +
            " ,[fromCnfOrgId]= @FromCnfOrgId" +
            " ,[toCnfOrgId]= @ToCnfOrgId" +
            " ,[againstLoadingQuotaId]= @AgainstLoadingQuotaId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[transferQty]= @TransferQty" +
            " ,[transferDesc] = @TransferDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTransferNote", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.IdTransferNote;
            cmdUpdate.Parameters.Add("@FromCnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.FromCnfOrgId;
            cmdUpdate.Parameters.Add("@ToCnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.ToCnfOrgId;
            cmdUpdate.Parameters.Add("@AgainstLoadingQuotaId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.AgainstLoadingQuotaId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaTransferTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaTransferTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@TransferQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaTransferTO.TransferQty;
            cmdUpdate.Parameters.Add("@TransferDesc", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaTransferTO.TransferDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTransferNote, cmdDelete);
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

        public static int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTransferNote, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idTransferNote, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLoadingQuotaTransfer] " +
            " WHERE idTransferNote = " + idTransferNote +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTransferNote", System.Data.SqlDbType.Int).Value = tblLoadingQuotaTransferTO.IdTransferNote;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
