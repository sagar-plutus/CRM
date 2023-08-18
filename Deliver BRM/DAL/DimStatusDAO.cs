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
    public class DimStatusDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimStatus]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimStatusTO> SelectAllDimStatus()
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

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStatusTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public static List<DimStatusTO> SelectAllDimStatus(int txnTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (txnTypeId == 0)
                    cmdSelect.CommandText = SqlSelectQuery();
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE transactionTypeId=" + txnTypeId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStatusTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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
        public static DimStatusTO SelectDimStatusByIotStatusId(Int32 iotStatusId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE iotStatusId = " + iotStatusId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStatusTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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
        
        public static DimStatusTO SelectDimStatus(Int32 idStatus,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idStatus = " + idStatus +" ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStatusTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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
                cmdSelect.Dispose();
            }
        }

        public static List<DimStatusTO> ConvertDTToList(SqlDataReader dimStatusTODT)
        {
            List<DimStatusTO> dimStatusTOList = new List<DimStatusTO>();
            if (dimStatusTODT != null)
            {
                while (dimStatusTODT.Read())
                {
                    DimStatusTO dimStatusTONew = new DimStatusTO();
                    if (dimStatusTODT["idStatus"] != DBNull.Value)
                        dimStatusTONew.IdStatus = Convert.ToInt32(dimStatusTODT["idStatus"].ToString());
                    if (dimStatusTODT["transactionTypeId"] != DBNull.Value)
                        dimStatusTONew.TransactionTypeId = Convert.ToInt32(dimStatusTODT["transactionTypeId"].ToString());
                    if (dimStatusTODT["isActive"] != DBNull.Value)
                        dimStatusTONew.IsActive = Convert.ToInt32(dimStatusTODT["isActive"].ToString());
                    if (dimStatusTODT["statusName"] != DBNull.Value)
                        dimStatusTONew.StatusName = Convert.ToString(dimStatusTODT["statusName"].ToString());
                    if (dimStatusTODT["prevStatusId"] != DBNull.Value)
                        dimStatusTONew.PrevStatusId = Convert.ToInt32(dimStatusTODT["prevStatusId"].ToString());
                    if (dimStatusTODT["statusDesc"] != DBNull.Value)
                        dimStatusTONew.StatusDesc = Convert.ToString(dimStatusTODT["statusDesc"]);
                    if (dimStatusTODT["colorCode"] != DBNull.Value)
                        dimStatusTONew.ColorCode = Convert.ToString(dimStatusTODT["colorCode"]);
                    if (dimStatusTODT["iotStatusId"] != DBNull.Value)
                        dimStatusTONew.IotStatusId = Convert.ToInt32(dimStatusTODT["iotStatusId"].ToString());

                    dimStatusTOList.Add(dimStatusTONew);
                }
            }
            return dimStatusTOList;
        }

        #endregion

        #region Insertion
        public static int InsertDimStatus(DimStatusTO dimStatusTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimStatusTO, cmdInsert);
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

        public static int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimStatusTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(DimStatusTO dimStatusTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimStatus]( " + 
                            "  [idStatus]" +
                            " ,[transactionTypeId]" +
                            " ,[isActive]" +
                            " ,[statusName]" +
                            " ,[prevStatusId]" +
                            " ,[statusDesc]" +
                            " ,[dispSeqNo]" +
                            " ,[colorCode]" +
                            " ,[iotStatusId]" +
                            " )" +
                " VALUES (" +
                            "  @IdStatus " +
                            " ,@TransactionTypeId " +
                            " ,@IsActive " +
                            " ,@StatusName " +
                            " ,@prevStatusId " +
                             " ,@statusDesc " +
                             " ,@dispSeqNo " +
                             " ,@colorCode " +
                             " ,@iotStatusId " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdStatus", System.Data.SqlDbType.Int).Value = dimStatusTO.IdStatus;
            cmdInsert.Parameters.Add("@TransactionTypeId", System.Data.SqlDbType.Int).Value = dimStatusTO.TransactionTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimStatusTO.IsActive;
            cmdInsert.Parameters.Add("@StatusName", System.Data.SqlDbType.NVarChar).Value = dimStatusTO.StatusName;
            cmdInsert.Parameters.Add("@prevStatusId", System.Data.SqlDbType.NVarChar).Value = dimStatusTO.PrevStatusId;
            cmdInsert.Parameters.Add("@statusDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimStatusTO.StatusDesc);
            cmdInsert.Parameters.Add("@dispSeqNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimStatusTO.DispSeqNo);
            cmdInsert.Parameters.Add("@colorCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimStatusTO.ColorCode);
            cmdInsert.Parameters.Add("@iotStatusId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimStatusTO.IotStatusId);

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateDimStatus(DimStatusTO dimStatusTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimStatusTO, cmdUpdate);
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

        public static int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimStatusTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(DimStatusTO dimStatusTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimStatus] SET " + 
            "  [idStatus] = @IdStatus" +
            " ,[transactionTypeId]= @TransactionTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[statusName] = @StatusName" +
            " ,[statusDesc] = @statusDesc " +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStatus", System.Data.SqlDbType.Int).Value = dimStatusTO.IdStatus;
            cmdUpdate.Parameters.Add("@TransactionTypeId", System.Data.SqlDbType.Int).Value = dimStatusTO.TransactionTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimStatusTO.IsActive;
            cmdUpdate.Parameters.Add("@StatusName", System.Data.SqlDbType.NVarChar).Value = dimStatusTO.StatusName;
            cmdUpdate.Parameters.Add("@statusDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimStatusTO.StatusDesc);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteDimStatus(Int32 idStatus)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStatus, cmdDelete);
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

        public static int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStatus, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idStatus, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimStatus] " +
            " WHERE idStatus = " + idStatus +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStatus", System.Data.SqlDbType.Int).Value = dimStatusTO.IdStatus;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
