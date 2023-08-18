using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI;

namespace SalesTrackerAPI.DAL

{
    public class TblInvoiceChangeOrgHistoryDAO 
    {
        
        
        public  static int InsertTblInvoiceChangeOrgHistory(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceChangeOrgHistoryTO, cmdInsert);
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

        public static int InsertTblInvoiceChangeOrgHistory(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceChangeOrgHistoryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblInvoiceChangeOrgHistory]( " + 
          //  "  [idInvoiceChangeOrgHistory]" +
            " [invoiceId]" +
            " ,[dupInvoiceId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[actionDesc]" +
            " )" +
" VALUES (" +
           // "  @IdInvoiceChangeOrgHistory " +
            " @InvoiceId " +
            " ,@DupInvoiceId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@ActionDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

         
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceChangeOrgHistoryTO.InvoiceId;
            cmdInsert.Parameters.Add("@DupInvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceChangeOrgHistoryTO.DupInvoiceId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceChangeOrgHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceChangeOrgHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@ActionDesc", System.Data.SqlDbType.NVarChar).Value = tblInvoiceChangeOrgHistoryTO.ActionDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        
      
        }

     }
