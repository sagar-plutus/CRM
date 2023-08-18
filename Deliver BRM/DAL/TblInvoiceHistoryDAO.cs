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
    public class TblInvoiceHistoryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tempInvoiceHistory]" +

                                  // Vaibhav [20-Nov-2017] Added to select from finalInvoiceHistory
                                  " UNION ALL "+
                                  " SELECT * FROM [finalInvoiceHistory] ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistory()
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
                List<TblInvoiceHistoryTO> list = ConvertDTToList(reader);
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

        public static TblInvoiceHistoryTO SelectTblInvoiceHistory(Int32 idInvHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery()+ ")sq1 WHERE idInvHistory = " + idInvHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceHistoryTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
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

        public static TblInvoiceHistoryTO SelectTblInvoiceHistoryTORateByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
           
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {

                cmdSelect.CommandText =
                      //" SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE invoiceItemId = " + invoiceItemId + " " +
                      //"AND oldUnitRate IS NOT NULL AND newUnitRate IS NOT NULL ";
                      "   SELECT * FROM (SELECT * FROM [tempInvoiceHistory]  where idInvHistory IN (SELECT TOP 1 idInvHistory FROM tempInvoiceHistory " +
                      "   WHERE invoiceItemId  =  " + invoiceItemId + " " + "   AND oldUnitRate IS NOT NULL AND newUnitRate IS NOT NULL  ORDER BY createdOn DESC  ) " +
                      "   UNION ALL  SELECT *  FROM [finalInvoiceHistory]    where idInvHistory IN (SELECT TOP 1 idInvHistory FROM tempInvoiceHistory  " +
                      "   WHERE invoiceItemId =  " + invoiceItemId + " " + "   AND oldUnitRate IS NOT NULL AND newUnitRate IS NOT NULL  ORDER BY createdOn DESC ))sq1 ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceHistoryTO> list = ConvertDTToList(reader);
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
                //conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryById(Int32 byId, Boolean isByInvoiceId )
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();                
                if (isByInvoiceId)
                {
                    cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE invoiceId = " + byId + " ";
                }
                else
                {
                    cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE invoiceItemId = " + byId + " ";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceHistoryTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0) return list;
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

        public static List<TblInvoiceHistoryTO> ConvertDTToList(SqlDataReader tblInvoiceHistoryTODT)
        {
            List<TblInvoiceHistoryTO> tblInvoiceHistoryTOList = new List<TblInvoiceHistoryTO>();
            if (tblInvoiceHistoryTODT != null)
            {
                while (tblInvoiceHistoryTODT.Read())
                {
                    TblInvoiceHistoryTO tblInvoiceHistoryTONew = new TblInvoiceHistoryTO();
                    if (tblInvoiceHistoryTODT["idInvHistory"] != DBNull.Value)
                        tblInvoiceHistoryTONew.IdInvHistory = Convert.ToInt32(tblInvoiceHistoryTODT["idInvHistory"].ToString());
                    if (tblInvoiceHistoryTODT["invoiceId"] != DBNull.Value)
                        tblInvoiceHistoryTONew.InvoiceId = Convert.ToInt32(tblInvoiceHistoryTODT["invoiceId"].ToString());
                    if (tblInvoiceHistoryTODT["invoiceItemId"] != DBNull.Value)
                        tblInvoiceHistoryTONew.InvoiceItemId = Convert.ToInt32(tblInvoiceHistoryTODT["invoiceItemId"].ToString());
                    if (tblInvoiceHistoryTODT["oldCdStructureId"] != DBNull.Value)
                        tblInvoiceHistoryTONew.OldCdStructureId = Convert.ToInt32(tblInvoiceHistoryTODT["oldCdStructureId"].ToString());
                    if (tblInvoiceHistoryTODT["newCdStructureId"] != DBNull.Value)
                        tblInvoiceHistoryTONew.NewCdStructureId = Convert.ToInt32(tblInvoiceHistoryTODT["newCdStructureId"].ToString());
                    if (tblInvoiceHistoryTODT["statusId"] != DBNull.Value)
                        tblInvoiceHistoryTONew.StatusId = Convert.ToInt32(tblInvoiceHistoryTODT["statusId"].ToString());
                    if (tblInvoiceHistoryTODT["createdBy"] != DBNull.Value)
                        tblInvoiceHistoryTONew.CreatedBy = Convert.ToInt32(tblInvoiceHistoryTODT["createdBy"].ToString());
                    if (tblInvoiceHistoryTODT["statusDate"] != DBNull.Value)
                        tblInvoiceHistoryTONew.StatusDate = Convert.ToDateTime(tblInvoiceHistoryTODT["statusDate"].ToString());
                    if (tblInvoiceHistoryTODT["createdOn"] != DBNull.Value)
                        tblInvoiceHistoryTONew.CreatedOn = Convert.ToDateTime(tblInvoiceHistoryTODT["createdOn"].ToString());
                    if (tblInvoiceHistoryTODT["oldUnitRate"] != DBNull.Value)
                        tblInvoiceHistoryTONew.OldUnitRate = Convert.ToDouble(tblInvoiceHistoryTODT["oldUnitRate"].ToString());
                    if (tblInvoiceHistoryTODT["newUnitRate"] != DBNull.Value)
                        tblInvoiceHistoryTONew.NewUnitRate = Convert.ToDouble(tblInvoiceHistoryTODT["newUnitRate"].ToString());
                    if (tblInvoiceHistoryTODT["oldQty"] != DBNull.Value)
                        tblInvoiceHistoryTONew.OldQty = Convert.ToDouble(tblInvoiceHistoryTODT["oldQty"].ToString());
                    if (tblInvoiceHistoryTODT["newQty"] != DBNull.Value)
                        tblInvoiceHistoryTONew.NewQty = Convert.ToDouble(tblInvoiceHistoryTODT["newQty"].ToString());
                    if (tblInvoiceHistoryTODT["oldBillingAddr"] != DBNull.Value)
                        tblInvoiceHistoryTONew.OldBillingAddr = Convert.ToString(tblInvoiceHistoryTODT["oldBillingAddr"].ToString());
                    if (tblInvoiceHistoryTODT["newBillingAddr"] != DBNull.Value)
                        tblInvoiceHistoryTONew.NewBillingAddr = Convert.ToString(tblInvoiceHistoryTODT["newBillingAddr"].ToString());
                    if (tblInvoiceHistoryTODT["oldConsinAddr"] != DBNull.Value)
                        tblInvoiceHistoryTONew.OldConsinAddr = Convert.ToString(tblInvoiceHistoryTODT["oldConsinAddr"].ToString());
                    if (tblInvoiceHistoryTODT["newConsinAddr"] != DBNull.Value)
                        tblInvoiceHistoryTONew.NewConsinAddr = Convert.ToString(tblInvoiceHistoryTODT["newConsinAddr"].ToString());
                    if (tblInvoiceHistoryTODT["oldEwayBillNo"] != DBNull.Value)
                        tblInvoiceHistoryTONew.OldEwayBillNo = Convert.ToString(tblInvoiceHistoryTODT["oldEwayBillNo"].ToString());
                    if (tblInvoiceHistoryTODT["newEwayBillNo"] != DBNull.Value)
                        tblInvoiceHistoryTONew.NewEwayBillNo = Convert.ToString(tblInvoiceHistoryTODT["newEwayBillNo"].ToString());
                    if (tblInvoiceHistoryTODT["statusRemark"] != DBNull.Value)
                        tblInvoiceHistoryTONew.StatusRemark = Convert.ToString(tblInvoiceHistoryTODT["statusRemark"].ToString());
                    tblInvoiceHistoryTOList.Add(tblInvoiceHistoryTONew);
                }
            }
            return tblInvoiceHistoryTOList;
        }

        public static List<TblInvoiceHistoryTO> SelectTempInvoiceHistory(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE invoiceId = " + invoiceId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceHistoryTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
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


        public static TblInvoiceHistoryTO SelectTblInvoiceHistoryTOCdByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {

                cmdSelect.CommandText =
                    //" SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE invoiceItemId = " + invoiceItemId + " " +
                    //"AND oldCdStructureId IS NOT NULL AND newCdStructureId IS NOT NULL ";
                    "   SELECT * FROM ( SELECT * FROM [tempInvoiceHistory] where idInvHistory IN ( SELECT TOP 1 idInvHistory FROM tempInvoiceHistory " +
                    "   WHERE invoiceItemId =  " + invoiceItemId + " " + "   AND oldCdStructureId IS NOT NULL AND newCdStructureId IS NOT NULL ORDER BY createdOn DESC ) " +
                    "   UNION ALL  SELECT * FROM [finalInvoiceHistory] where idInvHistory IN(SELECT TOP 1 idInvHistory FROM tempInvoiceHistory " +
                    "   WHERE invoiceItemId =  " + invoiceItemId + " " + "   AND oldCdStructureId IS NOT NULL AND newCdStructureId IS NOT NULL ORDER BY createdOn DESC) )sq1 ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceHistoryTO> list = ConvertDTToList(reader);
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
                //conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public static int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceHistoryTO, cmdInsert);
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

        public static int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceHistoryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempInvoiceHistory]( " +
                                "  [invoiceId]" +
                                " ,[invoiceItemId]" +
                                " ,[oldCdStructureId]" +
                                " ,[newCdStructureId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[statusDate]" +
                                " ,[createdOn]" +
                                " ,[oldUnitRate]" +
                                " ,[newUnitRate]" +
                                " ,[oldQty]" +
                                " ,[newQty]" +
                                " ,[oldBillingAddr]" +
                                " ,[newBillingAddr]" +
                                " ,[oldConsinAddr]" +
                                " ,[newConsinAddr]" +
                                " ,[oldEwayBillNo]" +
                                " ,[newEwayBillNo]" +
                                " ,[statusRemark]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceId " +
                                " ,@InvoiceItemId " +
                                " ,@OldCdStructureId " +
                                " ,@NewCdStructureId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@StatusDate " +
                                " ,@CreatedOn " +
                                " ,@OldUnitRate " +
                                " ,@NewUnitRate " +
                                " ,@OldQty " +
                                " ,@NewQty " +
                                " ,@OldBillingAddr " +
                                " ,@NewBillingAddr " +
                                " ,@OldConsinAddr " +
                                " ,@NewConsinAddr " +
                                " ,@OldEwayBillNo " +
                                " ,@NewEwayBillNo " +
                                " ,@StatusRemark " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.InvoiceId;
            cmdInsert.Parameters.Add("@InvoiceItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.InvoiceItemId);
            cmdInsert.Parameters.Add("@OldCdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldCdStructureId);
            cmdInsert.Parameters.Add("@NewCdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewCdStructureId);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@OldUnitRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldUnitRate);
            cmdInsert.Parameters.Add("@NewUnitRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewUnitRate);
            cmdInsert.Parameters.Add("@OldQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldQty);
            cmdInsert.Parameters.Add("@NewQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewQty);
            cmdInsert.Parameters.Add("@OldBillingAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldBillingAddr);
            cmdInsert.Parameters.Add("@NewBillingAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewBillingAddr);
            cmdInsert.Parameters.Add("@OldConsinAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldConsinAddr);
            cmdInsert.Parameters.Add("@NewConsinAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewConsinAddr);
            cmdInsert.Parameters.Add("@OldEwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldEwayBillNo);
            cmdInsert.Parameters.Add("@NewEwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewEwayBillNo);
            cmdInsert.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.StatusRemark);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceHistoryTO.IdInvHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceHistoryTO, cmdUpdate);
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

        public static int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceHistoryTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempInvoiceHistory] SET " + 
                            "  [idInvHistory] = @IdInvHistory" +
                            " ,[invoiceId]= @InvoiceId" +
                            " ,[invoiceItemId]= @InvoiceItemId" +
                            " ,[oldCdStructureId]= @OldCdStructureId" +
                            " ,[newCdStructureId]= @NewCdStructureId" +
                            " ,[statusId]= @StatusId" +
                            " ,[createdBy]= @CreatedBy" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[createdOn]= @CreatedOn" +
                            " ,[oldUnitRate]= @OldUnitRate" +
                            " ,[newUnitRate]= @NewUnitRate" +
                            " ,[oldQty]= @OldQty" +
                            " ,[newQty]= @NewQty" +
                            " ,[oldBillingAddr]= @OldBillingAddr" +
                            " ,[newBillingAddr]= @NewBillingAddr" +
                            " ,[oldConsinAddr]= @OldConsinAddr" +
                            " ,[newConsinAddr]= @NewConsinAddr" +
                            " ,[oldEwayBillNo]= @OldEwayBillNo" +
                            " ,[newEwayBillNo]= @NewEwayBillNo" +
                            " ,[statusRemark] = @StatusRemark" +
                            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
            cmdUpdate.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.InvoiceId;
            cmdUpdate.Parameters.Add("@InvoiceItemId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.InvoiceItemId;
            cmdUpdate.Parameters.Add("@OldCdStructureId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.OldCdStructureId;
            cmdUpdate.Parameters.Add("@NewCdStructureId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.NewCdStructureId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.StatusDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@OldUnitRate", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.OldUnitRate;
            cmdUpdate.Parameters.Add("@NewUnitRate", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.NewUnitRate;
            cmdUpdate.Parameters.Add("@OldQty", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.OldQty;
            cmdUpdate.Parameters.Add("@NewQty", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.NewQty;
            cmdUpdate.Parameters.Add("@OldBillingAddr", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.OldBillingAddr;
            cmdUpdate.Parameters.Add("@NewBillingAddr", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.NewBillingAddr;
            cmdUpdate.Parameters.Add("@OldConsinAddr", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.OldConsinAddr;
            cmdUpdate.Parameters.Add("@NewConsinAddr", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.NewConsinAddr;
            cmdUpdate.Parameters.Add("@OldEwayBillNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.OldEwayBillNo;
            cmdUpdate.Parameters.Add("@NewEwayBillNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.NewEwayBillNo;
            cmdUpdate.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = tblInvoiceHistoryTO.StatusRemark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceHistory(Int32 idInvHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvHistory, cmdDelete);
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

        public static int DeleteTblInvoiceHistory(Int32 idInvHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvHistory, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idInvHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceHistory] " +
            " WHERE idInvHistory = " + idInvHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
            return cmdDelete.ExecuteNonQuery();
        }

        public static int DeleteTblInvoiceHistoryByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
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
        /// /// Vijaymala [17-04-2018]:added to delete invoice history by invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="cmdDelete"></param>
        /// <returns></returns>
        public static int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceHistory] " +
            " WHERE invoiceId = " + invoiceId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

    }
}
