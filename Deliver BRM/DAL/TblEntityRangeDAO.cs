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
    public class TblEntityRangeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblEntityRange]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblEntityRangeTO> SelectAllTblEntityRange()
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

        public static TblEntityRangeTO SelectTblEntityRange(Int32 idEntityRange)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static TblEntityRangeTO SelectEntityRangeFromInvoiceType(Int32 invoiceTypeId,int finYearId,SqlConnection conn,SqlTransaction tran)
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


        public static TblEntityRangeTO SelectEntityRangeFromInvoiceType(String entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
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
        public static TblEntityRangeTO SelectEntityRangeFromInvoiceType(String entityName, int finYearId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE finYearId=" + finYearId + " AND entityName = '" + entityName + "'";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;

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
                conn.Close();
            }
        }
        public static TblEntityRangeTO SelectTblEntityRangeByEntityName(string entityName)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static TblEntityRangeTO SelectTblEntityRangeByEntityName(string entityName, int finYearId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE entityName = " + entityName + " ";
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE finYearId=" + finYearId + " AND entityName = '" + entityName + "'";
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

        public static TblEntityRangeTO SelectTblEntityRangeByEntityName(string entityName, int finYearId, SqlConnection conn,SqlTransaction tran)
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

        public static List<TblEntityRangeTO> ConvertDTToList(SqlDataReader tblEntityRangeTODT)
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
                        tblEntityRangeTONew.Prefix = Convert.ToString(tblEntityRangeTODT["prefix"]);

                    tblEntityRangeTOList.Add(tblEntityRangeTONew);
                }
            }
            return tblEntityRangeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteInsertionCommand(TblEntityRangeTO tblEntityRangeTO, SqlCommand cmdInsert)
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
            " ,[prefix]" +

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
            " ,@prefix " +
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
            cmdInsert.Parameters.Add("@prefix", System.Data.SqlDbType.NVarChar).Value = tblEntityRangeTO.Prefix;

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteUpdationCommand(TblEntityRangeTO tblEntityRangeTO, SqlCommand cmdUpdate)
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
                            " ,[prefix] = @prefix " +
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
            cmdUpdate.Parameters.Add("@prefix", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEntityRangeTO.Prefix);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblEntityRange(Int32 idEntityRange)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static int DeleteTblEntityRange(Int32 idEntityRange, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteDeletionCommand(Int32 idEntityRange, SqlCommand cmdDelete)
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
