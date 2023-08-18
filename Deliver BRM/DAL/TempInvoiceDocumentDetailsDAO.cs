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
    public class TempInvoiceDocumentDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT invoiceDoc.*,doc.documentDesc,doc.path " +
                                   " FROM [tempInvoiceDocumentDetails] invoiceDoc " +
                                   " LEFT JOIN[tblDocumentDetails] doc on doc.idDocument = invoiceDoc.documentId" +
                                   " UNION ALL " +
                                   " SELECT invoiceDoc.*,doc.documentDesc,doc.path " +
                                   " FROM [finalInvoiceDocumentDetails] invoiceDoc " +
                                   " LEFT JOIN[tblDocumentDetails] doc on doc.idDocument = invoiceDoc.documentId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TempInvoiceDocumentDetailsTO> SelectAllTempInvoiceDocumentDetails()
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

                //cmdSelect.Parameters.Add("@idInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempInvoiceDocumentDetailsTO> list = ConvertDTToList(reader);
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

        public static TempInvoiceDocumentDetailsTO SelectTempInvoiceDocumentDetails(Int32 idInvoiceDocument)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idInvoiceDocument = " + idInvoiceDocument + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempInvoiceDocumentDetailsTO> list = ConvertDTToList(reader);
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

        public static List<TempInvoiceDocumentDetailsTO> SelectAllTempInvoiceDocumentDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempInvoiceDocumentDetailsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TempInvoiceDocumentDetailsTO> ConvertDTToList(SqlDataReader tempInvoiceDocumentDetailsTODT)
        {
            List<TempInvoiceDocumentDetailsTO> tempInvoiceDocumentDetailsTOList = new List<TempInvoiceDocumentDetailsTO>();
            if (tempInvoiceDocumentDetailsTODT != null)
            {
                while (tempInvoiceDocumentDetailsTODT.Read())
                {
                    TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTONew = new TempInvoiceDocumentDetailsTO();
                    if (tempInvoiceDocumentDetailsTODT ["idInvoiceDocument"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.IdInvoiceDocument = Convert.ToInt32(tempInvoiceDocumentDetailsTODT ["idInvoiceDocument"].ToString());
                    if (tempInvoiceDocumentDetailsTODT ["invoiceId"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.InvoiceId = Convert.ToInt32(tempInvoiceDocumentDetailsTODT ["invoiceId"].ToString());
                    if (tempInvoiceDocumentDetailsTODT ["documentId"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.DocumentId = Convert.ToInt32(tempInvoiceDocumentDetailsTODT ["documentId"].ToString());
                    if (tempInvoiceDocumentDetailsTODT ["createdBy"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.CreatedBy = Convert.ToInt32(tempInvoiceDocumentDetailsTODT ["createdBy"].ToString());
                    if (tempInvoiceDocumentDetailsTODT ["updatedBy"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.UpdatedBy = Convert.ToInt32(tempInvoiceDocumentDetailsTODT ["updatedBy"].ToString());
                    if (tempInvoiceDocumentDetailsTODT ["createdOn"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.CreatedOn = Convert.ToDateTime(tempInvoiceDocumentDetailsTODT ["createdOn"].ToString());
                    if (tempInvoiceDocumentDetailsTODT ["updatedOn"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.UpdatedOn = Convert.ToDateTime(tempInvoiceDocumentDetailsTODT ["updatedOn"].ToString());
                    if (tempInvoiceDocumentDetailsTODT["documentDesc"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.DocumentDesc = Convert.ToString(tempInvoiceDocumentDetailsTODT["documentDesc"].ToString());
                    if (tempInvoiceDocumentDetailsTODT["path"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.Path = Convert.ToString(tempInvoiceDocumentDetailsTODT["path"].ToString());
                    if (tempInvoiceDocumentDetailsTODT["isActive"] != DBNull.Value)
                        tempInvoiceDocumentDetailsTONew.IsActive = Convert.ToInt32(tempInvoiceDocumentDetailsTODT["isActive"].ToString());
                    tempInvoiceDocumentDetailsTOList.Add(tempInvoiceDocumentDetailsTONew);
                }
            }
            return tempInvoiceDocumentDetailsTOList;
        }

        public static List<TempInvoiceDocumentDetailsTO> SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(Int32 invoiceId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "Select * from (" + SqlSelectQuery() + ")sq1  WHERE sq1.invoiceId = " + invoiceId + " AND sq1.isActive=1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempInvoiceDocumentDetailsTO> list = ConvertDTToList(reader);
                return list;
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

        public static List<TempInvoiceDocumentDetailsTO> SelectTempInvoiceDocumentDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = "Select * from (" + SqlSelectQuery() + ")sq1  WHERE sq1.invoiceId = " + invoiceId + " AND sq1.isActive=1 ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);

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
        public static int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tempInvoiceDocumentDetailsTO, cmdInsert);
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

        public static int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tempInvoiceDocumentDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempInvoiceDocumentDetails]( " + 
            //"  [idInvoiceDocument]" +
            " [invoiceId]" +
            " ,[documentId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " )" +
" VALUES (" +
            //"  @IdInvoiceDocument " +
            "  @InvoiceId " +
            " ,@DocumentId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            ", @UpdatedOn " +
            " ,@IsActive " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.InvoiceId;
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.DocumentId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tempInvoiceDocumentDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tempInvoiceDocumentDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue( tempInvoiceDocumentDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tempInvoiceDocumentDetailsTO.IdInvoiceDocument = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tempInvoiceDocumentDetailsTO, cmdUpdate);
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

        public static int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tempInvoiceDocumentDetailsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempInvoiceDocumentDetails] SET " + 
            "  [invoiceId] = @InvoiceId" +
            " ,[documentId]= @DocumentId" +
            " ,[createdBy] = @CreatedBy" +
            " ,[updatedBy] = @UpdatedBy" +
            " ,[createdOn] = @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[isActive] = @IsActive" +
            " WHERE   [idInvoiceDocument] = @IdInvoiceDocument" ;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
            cmdUpdate.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.InvoiceId;
            cmdUpdate.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.DocumentId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tempInvoiceDocumentDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tempInvoiceDocumentDetailsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IsActive;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvoiceDocument, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvoiceDocument, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idInvoiceDocument, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceDocumentDetails] " +
            " WHERE idInvoiceDocument = " + idInvoiceDocument +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
            return cmdDelete.ExecuteNonQuery();
        }

        public static int DeleteTblInvoiceDocumentByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommandByInvoiceId(invoiceId, cmdDelete);
            }
            catch (Exception ex)
            {


                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        /// <summary>
        /// /// Vijaymala [28-05-2018]:added to delete invoice document details by invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="cmdDelete"></param>
        /// <returns></returns>
        public static int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceDocumentDetails] " +
            " WHERE invoiceId = " + invoiceId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
            return cmdDelete.ExecuteNonQuery();
        }


        #endregion

    }
}
