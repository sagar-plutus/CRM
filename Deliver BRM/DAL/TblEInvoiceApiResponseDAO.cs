using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.BL;

namespace SalesTrackerAPI.DAL
{
    public class TblEInvoiceApiResponseDAO
    {
        public TblEInvoiceApiResponseDAO()
        {

        }

        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT idResponse, apiId, invoiceId, responseStatus, response, createdBy, createdOn " +
                                  " FROM TempEInvoiceApiResponse";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponse()
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
                List<TblEInvoiceApiResponseTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponse(Int32 apiId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE apiId=" + apiId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceApiResponseTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseList(int idResponse)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idResponse=" + idResponse;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceApiResponseTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(int invoiceId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE invoiceId=" + invoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceApiResponseTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(int invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE invoiceId=" + invoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceApiResponseTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblEInvoiceApiResponseTO> ConvertDTToList(SqlDataReader TblEInvoiceApiResponseTODT)
        {
            List<TblEInvoiceApiResponseTO> TblEInvoiceApiResponseTOList = new List<TblEInvoiceApiResponseTO>();
            if (TblEInvoiceApiResponseTODT != null)
            {
                while(TblEInvoiceApiResponseTODT.Read())
                {
                    TblEInvoiceApiResponseTO TblEInvoiceApiResponseTONew = new TblEInvoiceApiResponseTO();
                    if (TblEInvoiceApiResponseTODT["idResponse"] != DBNull.Value)
                        TblEInvoiceApiResponseTONew.IdResponse = Convert.ToInt32(TblEInvoiceApiResponseTODT["idResponse"].ToString());
                    if (TblEInvoiceApiResponseTODT["apiId"] != DBNull.Value)
                        TblEInvoiceApiResponseTONew.ApiId = Convert.ToInt32(TblEInvoiceApiResponseTODT["apiId"].ToString());
                    if (TblEInvoiceApiResponseTODT["invoiceId"] != DBNull.Value)
                        TblEInvoiceApiResponseTONew.InvoiceId = Convert.ToInt32(TblEInvoiceApiResponseTODT["invoiceId"].ToString());
                    if (TblEInvoiceApiResponseTODT["responseStatus"] != DBNull.Value)
                        TblEInvoiceApiResponseTONew.ResponseStatus = TblEInvoiceApiResponseTODT["responseStatus"].ToString();
                    if (TblEInvoiceApiResponseTODT["response"] != DBNull.Value)
                        TblEInvoiceApiResponseTONew.Response = TblEInvoiceApiResponseTODT["response"].ToString();
                    if (TblEInvoiceApiResponseTODT["createdBy"] != DBNull.Value)
                        TblEInvoiceApiResponseTONew.CreatedBy = Convert.ToInt32(TblEInvoiceApiResponseTODT["createdBy"].ToString());
                    if (TblEInvoiceApiResponseTODT["createdOn"] != DBNull.Value)
                        TblEInvoiceApiResponseTONew.CreatedOn = Convert.ToDateTime(TblEInvoiceApiResponseTODT["createdOn"].ToString());
                    TblEInvoiceApiResponseTOList.Add(TblEInvoiceApiResponseTONew);
                }
            }
            return TblEInvoiceApiResponseTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(TblEInvoiceApiResponseTO, cmdInsert);
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

        public static int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(TblEInvoiceApiResponseTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [TempEInvoiceApiResponse]( " +
                                "  [apiId]" +
                                " ,[invoiceId]" +
                                " ,[responseStatus]" +
                                " ,[response]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " )" +
                    " VALUES (" +
                                "  @ApiId " +
                                " ,@InvoiceId " +
                                " ,@ResponseStatus " +
                                " ,@Response " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdResponse", System.Data.SqlDbType.Int).Value = TblEInvoiceApiResponseTO.IdResponse;
            cmdInsert.Parameters.Add("@ApiId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.ApiId);
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.InvoiceId);
            cmdInsert.Parameters.Add("@ResponseStatus", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.ResponseStatus);
            cmdInsert.Parameters.Add("@Response", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.Response);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = TblEInvoiceApiResponseTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = TblEInvoiceApiResponseTO.CreatedOn;
            
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                TblEInvoiceApiResponseTO.IdResponse = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblEInvoiceApiResponse(Int32 idResponse)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idResponse, cmdDelete);
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

        public static int DeleteTblEInvoiceApiResponse(Int32 idResponse, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idResponse, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idResponse, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [TempEInvoiceApiResponse] " +
            " WHERE idResponse = " + idResponse + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
