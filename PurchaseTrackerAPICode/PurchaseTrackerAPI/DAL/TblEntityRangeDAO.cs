using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblEntityRangeDAO : ITblEntityRangeDAO
    {


        private readonly IConnectionString _iConnectionString;
        public TblEntityRangeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblEntityRange]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblEntityRangeTO> SelectAllTblEntityRange()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                List<TblEntityRangeTO> list = ConvertDTToList(reader);
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

        public  TblEntityRangeTO SelectTblEntityRange(Int32 idEntityRange)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idEntityRange = " + idEntityRange +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEntityRangeTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
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
        #region  @Kiran Migration Of Invoice 

        
        public  TblEntityRangeTO SelectEntityRangeFromInvoiceType(Int32 invoiceTypeId,int finYearId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE finYearId="+ finYearId + " AND entityName IN(SELECT entityName FROM dimInvoiceTypes WHERE idInvoiceType=" + invoiceTypeId + ")";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEntityRangeTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        #endregion
        public  TblEntityRangeTO SelectEntityRangeFromInvoiceType(String entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE finYearId=" + finYearId + " AND entityName = '" + entityName + "'";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEntityRangeTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  TblEntityRangeTO SelectTblEntityRangeByEntityName(string entityName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE entityName = " + entityName + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEntityRangeTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

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

        public  List<TblEntityRangeTO> ConvertDTToList(SqlDataReader tblEntityRangeTODT)
        {
            List<TblEntityRangeTO> tblEntityRangeTOList = new List<TblEntityRangeTO>();
            if (tblEntityRangeTODT != null)
            {
                while (tblEntityRangeTODT.Read())
                {
                    TblEntityRangeTO tblEntityRangeTONew = new TblEntityRangeTO();
                    if (tblEntityRangeTODT["idEntityRange"] != DBNull.Value)
                        tblEntityRangeTONew.IdEntityRange = Convert.ToInt32(tblEntityRangeTODT["idEntityRange"].ToString());
                    if (tblEntityRangeTODT["finYearId"] != DBNull.Value)
                        tblEntityRangeTONew.FinYearId = Convert.ToInt32(tblEntityRangeTODT["finYearId"].ToString());
                    if (tblEntityRangeTODT["entityStartValue"] != DBNull.Value)
                        tblEntityRangeTONew.EntityStartValue = Convert.ToInt32(tblEntityRangeTODT["entityStartValue"].ToString());
                    if (tblEntityRangeTODT["entityRange"] != DBNull.Value)
                        tblEntityRangeTONew.EntityRange = Convert.ToInt32(tblEntityRangeTODT["entityRange"].ToString());
                    if (tblEntityRangeTODT["entityEndingValue"] != DBNull.Value)
                        tblEntityRangeTONew.EntityEndingValue = Convert.ToInt32(tblEntityRangeTODT["entityEndingValue"].ToString());
                    if (tblEntityRangeTODT["entityPrevValue"] != DBNull.Value)
                        tblEntityRangeTONew.EntityPrevValue = Convert.ToInt32(tblEntityRangeTODT["entityPrevValue"].ToString());
                    if (tblEntityRangeTODT["incrementBy"] != DBNull.Value)
                        tblEntityRangeTONew.IncrementBy = Convert.ToInt32(tblEntityRangeTODT["incrementBy"].ToString());
                    if (tblEntityRangeTODT["createdOn"] != DBNull.Value)
                        tblEntityRangeTONew.CreatedOn = Convert.ToDateTime(tblEntityRangeTODT["createdOn"].ToString());
                    if (tblEntityRangeTODT["entityName"] != DBNull.Value)
                        tblEntityRangeTONew.EntityName = Convert.ToString(tblEntityRangeTODT["entityName"].ToString());
                    if (tblEntityRangeTODT["entityDesc"] != DBNull.Value)
                        tblEntityRangeTONew.EntityDesc = Convert.ToString(tblEntityRangeTODT["entityDesc"].ToString());
                    if (tblEntityRangeTODT["prefix"] != DBNull.Value)
                        tblEntityRangeTONew.Prefix = Convert.ToString(tblEntityRangeTODT["prefix"].ToString());
                    if (tblEntityRangeTODT["suffix"] != DBNull.Value)
                        tblEntityRangeTONew.Suffix = Convert.ToString(tblEntityRangeTODT["suffix"].ToString());
                    tblEntityRangeTOList.Add(tblEntityRangeTONew);
                }
            }
            return tblEntityRangeTOList;
        }

        public  TblEntityRangeTO SelectTblEntityRangeByEntityName(string entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;

            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE finYearId=" + finYearId + " AND entityName = '" + entityName + "'";

                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEntityRangeTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public  int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblEntityRangeTO, cmdInsert);
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

        public  int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblEntityRangeTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblEntityRangeTO tblEntityRangeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblEntityRange]( " + 
            "  [idEntityRange]" +
            " ,[finYearId]" +
            " ,[entityStartValue]" +
            " ,[entityRange]" +
            " ,[entityEndingValue]" +
            " ,[entityPrevValue]" +
            " ,[incrementBy]" +
            " ,[createdOn]" +
            " ,[entityName]" +
            " ,[entityDesc]" +
            " )" +
" VALUES (" +
            "  @IdEntityRange " +
            " ,@FinYearId " +
            " ,@EntityStartValue " +
            " ,@EntityRange " +
            " ,@EntityEndingValue " +
            " ,@EntityPrevValue " +
            " ,@IncrementBy " +
            " ,@CreatedOn " +
            " ,@EntityName " +
            " ,@EntityDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdEntityRange", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.IdEntityRange;
            cmdInsert.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.FinYearId;
            cmdInsert.Parameters.Add("@EntityStartValue", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityStartValue;
            cmdInsert.Parameters.Add("@EntityRange", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityRange;
            cmdInsert.Parameters.Add("@EntityEndingValue", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityEndingValue;
            cmdInsert.Parameters.Add("@EntityPrevValue", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityPrevValue;
            cmdInsert.Parameters.Add("@IncrementBy", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.IncrementBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblEntityRangeTO.CreatedOn;
            cmdInsert.Parameters.Add("@EntityName", System.Data.SqlDbType.NVarChar).Value = tblEntityRangeTO.EntityName;
            cmdInsert.Parameters.Add("@EntityDesc", System.Data.SqlDbType.NVarChar).Value = tblEntityRangeTO.EntityDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblEntityRangeTO, cmdUpdate);
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

        public  int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblEntityRangeTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblEntityRangeTO tblEntityRangeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblEntityRange] SET " + 
                            "  [finYearId]= @FinYearId" +
                            " ,[entityStartValue]= @EntityStartValue" +
                            " ,[entityRange]= @EntityRange" +
                            " ,[entityEndingValue]= @EntityEndingValue" +
                            " ,[entityPrevValue]= @EntityPrevValue" +
                            " ,[incrementBy]= @IncrementBy" +
                            " ,[createdOn]= @CreatedOn" +
                            " ,[entityName]= @EntityName" +
                            " ,[entityDesc] = @EntityDesc" +
                            " WHERE [idEntityRange] = @IdEntityRange "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdEntityRange", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.IdEntityRange;
            cmdUpdate.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue( tblEntityRangeTO.FinYearId);
            cmdUpdate.Parameters.Add("@EntityStartValue", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityStartValue;
            cmdUpdate.Parameters.Add("@EntityRange", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityRange;
            cmdUpdate.Parameters.Add("@EntityEndingValue", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityEndingValue;
            cmdUpdate.Parameters.Add("@EntityPrevValue", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.EntityPrevValue;
            cmdUpdate.Parameters.Add("@IncrementBy", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.IncrementBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblEntityRangeTO.CreatedOn;
            cmdUpdate.Parameters.Add("@EntityName", System.Data.SqlDbType.NVarChar).Value = tblEntityRangeTO.EntityName;
            cmdUpdate.Parameters.Add("@EntityDesc", System.Data.SqlDbType.NVarChar).Value = tblEntityRangeTO.EntityDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblEntityRange(Int32 idEntityRange)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idEntityRange, cmdDelete);
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

        public  int DeleteTblEntityRange(Int32 idEntityRange, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idEntityRange, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idEntityRange, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblEntityRange] " +
            " WHERE idEntityRange = " + idEntityRange +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idEntityRange", System.Data.SqlDbType.Int).Value = tblEntityRangeTO.IdEntityRange;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
