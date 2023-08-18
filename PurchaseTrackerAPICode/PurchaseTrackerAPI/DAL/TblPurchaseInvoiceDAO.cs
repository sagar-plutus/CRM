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
    public class TblPurchaseInvoiceDAO : ITblPurchaseInvoiceDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tbluser.userdisplayname,* FROM [tblPurchaseInvoice] tblPurchaseInvoice " +
                                  " LEFT JOIN tbluser tbluser on tbluser.iduser = tblPurchaseInvoice.createdBy "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoice()
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
                List<TblPurchaseInvoiceTO> list = ConvertDTToList(reader);
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


        public  List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoice.purSchSummaryId  =" + rootPurchaseSchId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceTO> list = ConvertDTToList(reader);
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

        //Prajakta[2019-02-27] Added
        public  List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoice.purSchSummaryId  =" + rootPurchaseSchId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }



        public  TblPurchaseInvoiceTO SelectTblPurchaseInvoice(Int64 idInvoicePurchase)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idInvoicePurchase = " + idInvoicePurchase +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idInvoicePurchase", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceTO.IdInvoicePurchase;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseInvoiceTO SelectTblPurchaseInvoice(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idInvoicePurchase = " + idInvoicePurchase + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoice(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceTO> list = ConvertDTToList(reader);
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

        //Added By Gokul
        public int SelectPurchaseInvoiceByInvoiceIdandFinYear(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, Boolean isFromEdit, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = SqlSelectQuery();
                if (!isFromEdit)
                {
                    cmdSelect.CommandText = "select * from tblPurchaseInvoice where invoiceNo=" + "'" + tblPurchaseInvoiceTO.InvoiceNo + "'" + " and finYearId=" + tblPurchaseInvoiceTO.FinYearId + " and supplierId=" + tblPurchaseInvoiceTO.SupplierId;

                }
                else
                {
                    cmdSelect.CommandText = "select * from tblPurchaseInvoice where invoiceNo=" + "'" + tblPurchaseInvoiceTO.InvoiceNo + "'" + " and finYearId=" + tblPurchaseInvoiceTO.FinYearId + " and supplierId=" + tblPurchaseInvoiceTO.SupplierId + " and idInvoicePurchase != "+ tblPurchaseInvoiceTO.IdInvoicePurchase;

                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader.HasRows)
                    return 0;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdSelect.Dispose();
                reader.Dispose();
            }

        }

        public  List<TblPurchaseInvoiceTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceTODT)
        {
            List<TblPurchaseInvoiceTO> tblPurchaseInvoiceTOList = new List<TblPurchaseInvoiceTO>();
            if (tblPurchaseInvoiceTODT != null)
            {
                while (tblPurchaseInvoiceTODT.Read())
                {
                    TblPurchaseInvoiceTO tblPurchaseInvoiceTONew = new TblPurchaseInvoiceTO();
                    if (tblPurchaseInvoiceTODT["purchaseInvTypeId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.PurchaseInvTypeId = Convert.ToInt32(tblPurchaseInvoiceTODT["purchaseInvTypeId"].ToString());
                    if (tblPurchaseInvoiceTODT["finYearId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.FinYearId = Convert.ToInt32(tblPurchaseInvoiceTODT["finYearId"].ToString());
                    if (tblPurchaseInvoiceTODT["supplierId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.SupplierId = Convert.ToInt32(tblPurchaseInvoiceTODT["supplierId"].ToString());
                    if (tblPurchaseInvoiceTODT["brokerId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.BrokerId = Convert.ToInt32(tblPurchaseInvoiceTODT["brokerId"].ToString());
                    if (tblPurchaseInvoiceTODT["invoiceToOrgId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.InvoiceToOrgId = Convert.ToInt32(tblPurchaseInvoiceTODT["invoiceToOrgId"].ToString());
                    if (tblPurchaseInvoiceTODT["transportOrgId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.TransportOrgId = Convert.ToInt32(tblPurchaseInvoiceTODT["transportOrgId"].ToString());
                    if (tblPurchaseInvoiceTODT["transportModeId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.TransportModeId = Convert.ToInt32(tblPurchaseInvoiceTODT["transportModeId"].ToString());
                    if (tblPurchaseInvoiceTODT["currencyId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.CurrencyId = Convert.ToInt32(tblPurchaseInvoiceTODT["currencyId"].ToString());
                    if (tblPurchaseInvoiceTODT["statusId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.StatusId = Convert.ToInt32(tblPurchaseInvoiceTODT["statusId"].ToString());
                    if (tblPurchaseInvoiceTODT["createdBy"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.CreatedBy = Convert.ToInt32(tblPurchaseInvoiceTODT["createdBy"].ToString());
                    if (tblPurchaseInvoiceTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.UpdatedBy = Convert.ToInt32(tblPurchaseInvoiceTODT["updatedBy"].ToString());
                    if (tblPurchaseInvoiceTODT["invoiceDate"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.InvoiceDate = Convert.ToDateTime(tblPurchaseInvoiceTODT["invoiceDate"].ToString());
                    if (tblPurchaseInvoiceTODT["lrDate"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.LrDate = Convert.ToDateTime(tblPurchaseInvoiceTODT["lrDate"].ToString());
                    if (tblPurchaseInvoiceTODT["statusDate"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.StatusDate = Convert.ToDateTime(tblPurchaseInvoiceTODT["statusDate"].ToString());
                    if (tblPurchaseInvoiceTODT["createdOn"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.CreatedOn = Convert.ToDateTime(tblPurchaseInvoiceTODT["createdOn"].ToString());
                    if (tblPurchaseInvoiceTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseInvoiceTODT["updatedOn"].ToString());
                    if (tblPurchaseInvoiceTODT["currencyRate"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.CurrencyRate = Convert.ToDouble(tblPurchaseInvoiceTODT["currencyRate"].ToString());
                    if (tblPurchaseInvoiceTODT["billQty"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.BillQty = Convert.ToDouble(tblPurchaseInvoiceTODT["billQty"].ToString());
                    if (tblPurchaseInvoiceTODT["discountPct"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.DiscountPct = Convert.ToDouble(tblPurchaseInvoiceTODT["discountPct"].ToString());
                    if (tblPurchaseInvoiceTODT["discountAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.DiscountAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["discountAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["basicAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.BasicAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["basicAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["taxableAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.TaxableAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["taxableAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["cgstAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.CgstAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["cgstAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["sgstAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.SgstAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["sgstAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["igstAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.IgstAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["igstAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["freightPct"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.FreightPct = Convert.ToDouble(tblPurchaseInvoiceTODT["freightPct"].ToString());
                    if (tblPurchaseInvoiceTODT["freightAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.FreightAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["freightAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["otherExpAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.OtherExpAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["otherExpAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["tcsAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.TcsAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["tcsAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["transportorAdvAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.TransportorAdvAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["transportorAdvAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["roundOffAmt"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.RoundOffAmt = Convert.ToDouble(tblPurchaseInvoiceTODT["roundOffAmt"].ToString());
                    if (tblPurchaseInvoiceTODT["grandTotal"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.GrandTotal = Convert.ToDouble(tblPurchaseInvoiceTODT["grandTotal"].ToString());
                    if (tblPurchaseInvoiceTODT["idInvoicePurchase"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.IdInvoicePurchase = Convert.ToInt64(tblPurchaseInvoiceTODT["idInvoicePurchase"].ToString());
                    if (tblPurchaseInvoiceTODT["invoiceNo"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.InvoiceNo = Convert.ToString(tblPurchaseInvoiceTODT["invoiceNo"].ToString());
                    if (tblPurchaseInvoiceTODT["electronicRefNo"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.ElectronicRefNo = Convert.ToString(tblPurchaseInvoiceTODT["electronicRefNo"].ToString());
                    if (tblPurchaseInvoiceTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.VehicleNo = Convert.ToString(tblPurchaseInvoiceTODT["vehicleNo"].ToString());
                    if (tblPurchaseInvoiceTODT["lrNumber"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.LrNumber = Convert.ToString(tblPurchaseInvoiceTODT["lrNumber"].ToString());
                    if (tblPurchaseInvoiceTODT["roadPermitNo"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.RoadPermitNo = Convert.ToString(tblPurchaseInvoiceTODT["roadPermitNo"].ToString());
                    if (tblPurchaseInvoiceTODT["supplierName"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.SupplierName = Convert.ToString(tblPurchaseInvoiceTODT["supplierName"].ToString());
                    if (tblPurchaseInvoiceTODT["brokerName"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.BrokerName = Convert.ToString(tblPurchaseInvoiceTODT["brokerName"].ToString());
                    if (tblPurchaseInvoiceTODT["transportorName"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.TransportorName = Convert.ToString(tblPurchaseInvoiceTODT["transportorName"].ToString());
                    if (tblPurchaseInvoiceTODT["transportorForm"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.TransportorForm = Convert.ToString(tblPurchaseInvoiceTODT["transportorForm"].ToString());
                    if (tblPurchaseInvoiceTODT["airwayBillNo"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.AirwayBillNo = Convert.ToString(tblPurchaseInvoiceTODT["airwayBillNo"].ToString());
                    if (tblPurchaseInvoiceTODT["location"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.Location = Convert.ToString(tblPurchaseInvoiceTODT["location"].ToString());
                    if (tblPurchaseInvoiceTODT["narration"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.Narration = Convert.ToString(tblPurchaseInvoiceTODT["narration"].ToString());
                    if (tblPurchaseInvoiceTODT["remark"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.Remark = Convert.ToString(tblPurchaseInvoiceTODT["remark"].ToString());
                    if (tblPurchaseInvoiceTODT["purSchSummaryId"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.PurSchSummaryId = Convert.ToInt32(tblPurchaseInvoiceTODT["purSchSummaryId"].ToString());
                    if (tblPurchaseInvoiceTODT["ewayBillDate"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.EwayBillDate = Convert.ToDateTime(tblPurchaseInvoiceTODT["ewayBillDate"].ToString());
                    if (tblPurchaseInvoiceTODT["ewayBillExpiryDate"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.EwayBillExpiryDate = Convert.ToDateTime(tblPurchaseInvoiceTODT["ewayBillExpiryDate"].ToString());

                    if (tblPurchaseInvoiceTODT["userdisplayname"] != DBNull.Value)
                        tblPurchaseInvoiceTONew.CreatedByName = Convert.ToString(tblPurchaseInvoiceTODT["userdisplayname"].ToString());

                    tblPurchaseInvoiceTOList.Add(tblPurchaseInvoiceTONew);
                }
            }
            return tblPurchaseInvoiceTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseInvoiceTO, cmdInsert);
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

        public  int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseInvoiceTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseInvoice]( " + 
            "  [purchaseInvTypeId]" +
            " ,[finYearId]" +
            " ,[supplierId]" +
            " ,[brokerId]" +
            " ,[invoiceToOrgId]" +
            " ,[transportOrgId]" +
            " ,[transportModeId]" +
            " ,[currencyId]" +
            " ,[statusId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[invoiceDate]" +
            " ,[lrDate]" +
            " ,[statusDate]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[currencyRate]" +
            " ,[billQty]" +
            " ,[discountPct]" +
            " ,[discountAmt]" +
            " ,[basicAmt]" +
            " ,[taxableAmt]" +
            " ,[cgstAmt]" +
            " ,[sgstAmt]" +
            " ,[igstAmt]" +
            " ,[freightPct]" +
            " ,[freightAmt]" +
            " ,[otherExpAmt]" +
            " ,[tcsAmt]" +
            " ,[transportorAdvAmt]" +
            " ,[roundOffAmt]" +
            " ,[grandTotal]" +
            //" ,[idInvoicePurchase]" +
            " ,[invoiceNo]" +
            " ,[electronicRefNo]" +
            " ,[vehicleNo]" +
            " ,[lrNumber]" +
            " ,[roadPermitNo]" +
            " ,[supplierName]" +
            " ,[brokerName]" +
            " ,[transportorName]" +
            " ,[transportorForm]" +
            " ,[airwayBillNo]" +
            " ,[location]" +
            " ,[narration]" +
            " ,[remark]" +
            " ,[purSchSummaryId]" +
            " ,[ewayBillDate]" +
            " ,[ewayBillExpiryDate]" +
            " )" +
" VALUES (" +
            "  @PurchaseInvTypeId " +
            " ,@FinYearId " +
            " ,@SupplierId " +
            " ,@BrokerId " +
            " ,@InvoiceToOrgId " +
            " ,@TransportOrgId " +
            " ,@TransportModeId " +
            " ,@CurrencyId " +
            " ,@StatusId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@InvoiceDate " +
            " ,@LrDate " +
            " ,@StatusDate " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@CurrencyRate " +
            " ,@BillQty " +
            " ,@DiscountPct " +
            " ,@DiscountAmt " +
            " ,@BasicAmt " +
            " ,@TaxableAmt " +
            " ,@CgstAmt " +
            " ,@SgstAmt " +
            " ,@IgstAmt " +
            " ,@FreightPct " +
            " ,@FreightAmt " +
            " ,@OtherExpAmt " +
            " ,@TcsAmt " +
            " ,@TransportorAdvAmt " +
            " ,@RoundOffAmt " +
            " ,@GrandTotal " +
            //" ,@IdInvoicePurchase " +
            " ,@InvoiceNo " +
            " ,@ElectronicRefNo " +
            " ,@VehicleNo " +
            " ,@LrNumber " +
            " ,@RoadPermitNo " +
            " ,@SupplierName " +
            " ,@BrokerName " +
            " ,@TransportorName " +
            " ,@TransportorForm " +
            " ,@AirwayBillNo " +
            " ,@Location " +
            " ,@Narration " +
            " ,@Remark " +
            " ,@PurSchSummaryId " +
            " ,@EwayBillDate" +
            " ,@EwayBillExpiryDate" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@PurchaseInvTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.PurchaseInvTypeId;
            cmdInsert.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.FinYearId;
            cmdInsert.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.SupplierId;
            cmdInsert.Parameters.Add("@BrokerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.BrokerId);
            cmdInsert.Parameters.Add("@InvoiceToOrgId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.InvoiceToOrgId;
            cmdInsert.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportOrgId);
            cmdInsert.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportModeId);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.CurrencyId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.UpdatedBy);
            cmdInsert.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.InvoiceDate);
            cmdInsert.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.LrDate);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.UpdatedOn);
            cmdInsert.Parameters.Add("@CurrencyRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.CurrencyRate);
            cmdInsert.Parameters.Add("@BillQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.BillQty);
            cmdInsert.Parameters.Add("@DiscountPct", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.DiscountPct);
            cmdInsert.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.DiscountAmt);
            cmdInsert.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.BasicAmt);
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TaxableAmt);
            cmdInsert.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.CgstAmt);
            cmdInsert.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.SgstAmt);
            cmdInsert.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.IgstAmt);
            cmdInsert.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.FreightPct);
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.FreightAmt);
            cmdInsert.Parameters.Add("@OtherExpAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.OtherExpAmt);
            cmdInsert.Parameters.Add("@TcsAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TcsAmt);
            cmdInsert.Parameters.Add("@TransportorAdvAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportorAdvAmt);
            cmdInsert.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.RoundOffAmt);
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.GrandTotal);
            //cmdInsert.Parameters.Add("@IdInvoicePurchase", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceTO.IdInvoicePurchase;
            cmdInsert.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.InvoiceNo);
            cmdInsert.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.ElectronicRefNo);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.VehicleNo);
            cmdInsert.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.LrNumber);
            cmdInsert.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.RoadPermitNo);
            cmdInsert.Parameters.Add("@SupplierName", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceTO.SupplierName;
            cmdInsert.Parameters.Add("@BrokerName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.BrokerName);
            cmdInsert.Parameters.Add("@TransportorName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportorName);
            cmdInsert.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportorForm);
            cmdInsert.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.AirwayBillNo);
            cmdInsert.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.Location);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.Narration);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.Remark);
            cmdInsert.Parameters.Add("@PurSchSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.PurSchSummaryId;
            cmdInsert.Parameters.Add("@EwayBillDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.EwayBillDate);
            cmdInsert.Parameters.Add("@EwayBillExpiryDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.EwayBillExpiryDate);

            //return cmdInsert.ExecuteNonQuery();
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblPurchaseInvoiceTO.IdInvoicePurchase = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseInvoiceTO, cmdUpdate);
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

        public  int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseInvoiceTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseInvoice] SET " + 
            "  [purchaseInvTypeId] = @PurchaseInvTypeId" +
            " ,[finYearId]= @FinYearId" +
            " ,[supplierId]= @SupplierId" +
            " ,[brokerId]= @BrokerId" +
            " ,[invoiceToOrgId]= @InvoiceToOrgId" +
            " ,[transportOrgId]= @TransportOrgId" +
            " ,[transportModeId]= @TransportModeId" +
            " ,[currencyId]= @CurrencyId" +
            " ,[statusId]= @StatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[invoiceDate]= @InvoiceDate" +
            " ,[lrDate]= @LrDate" +
            " ,[statusDate]= @StatusDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[currencyRate]= @CurrencyRate" +
            " ,[billQty]= @BillQty" +
            " ,[discountPct]= @DiscountPct" +
            " ,[discountAmt]= @DiscountAmt" +
            " ,[basicAmt]= @BasicAmt" +
            " ,[taxableAmt]= @TaxableAmt" +
            " ,[cgstAmt]= @CgstAmt" +
            " ,[sgstAmt]= @SgstAmt" +
            " ,[igstAmt]= @IgstAmt" +
            " ,[freightPct]= @FreightPct" +
            " ,[freightAmt]= @FreightAmt" +
            " ,[otherExpAmt]= @OtherExpAmt" +
            " ,[tcsAmt]= @TcsAmt" +
            " ,[transportorAdvAmt]= @TransportorAdvAmt" +
            " ,[roundOffAmt]= @RoundOffAmt" +
            " ,[grandTotal]= @GrandTotal" +
            //" ,[idInvoicePurchase]= @IdInvoicePurchase" +
            " ,[invoiceNo]= @InvoiceNo" +
            " ,[electronicRefNo]= @ElectronicRefNo" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[lrNumber]= @LrNumber" +
            " ,[roadPermitNo]= @RoadPermitNo" +
            " ,[supplierName]= @SupplierName" +
            " ,[brokerName]= @BrokerName" +
            " ,[transportorName]= @TransportorName" +
            " ,[transportorForm]= @TransportorForm" +
            " ,[airwayBillNo]= @AirwayBillNo" +
            " ,[location]= @Location" +
            " ,[narration]= @Narration" +
            " ,[remark] = @Remark" +
            " ,[purSchSummaryId] = @PurSchSummaryId" +
            " ,[ewayBillDate] = @EwayBillDate"+
            " ,[ewayBillExpiryDate] = @EwayBillExpiryDate" +
            " WHERE [idInvoicePurchase]= @IdInvoicePurchase "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@PurchaseInvTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.PurchaseInvTypeId;
            cmdUpdate.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.FinYearId;
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.SupplierId;
            cmdUpdate.Parameters.Add("@BrokerId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.BrokerId;
            cmdUpdate.Parameters.Add("@InvoiceToOrgId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.InvoiceToOrgId;
            cmdUpdate.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportOrgId);
            cmdUpdate.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportModeId);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.CurrencyId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceTO.InvoiceDate;
            cmdUpdate.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.LrDate);
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceTO.StatusDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@CurrencyRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceTO.CurrencyRate;
            cmdUpdate.Parameters.Add("@BillQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.BillQty);
            cmdUpdate.Parameters.Add("@DiscountPct", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.DiscountPct);
            cmdUpdate.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.DiscountAmt);
            cmdUpdate.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.BasicAmt);
            cmdUpdate.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TaxableAmt);
            cmdUpdate.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.CgstAmt);
            cmdUpdate.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.SgstAmt);
            cmdUpdate.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.IgstAmt);
            cmdUpdate.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.FreightPct);
            cmdUpdate.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.FreightAmt);
            cmdUpdate.Parameters.Add("@OtherExpAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.OtherExpAmt);
            cmdUpdate.Parameters.Add("@TcsAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TcsAmt);
            cmdUpdate.Parameters.Add("@TransportorAdvAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportorAdvAmt);
            cmdUpdate.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.RoundOffAmt);
            cmdUpdate.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.GrandTotal);
            cmdUpdate.Parameters.Add("@IdInvoicePurchase", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceTO.IdInvoicePurchase;
            cmdUpdate.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.InvoiceNo);
            cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.ElectronicRefNo);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceTO.VehicleNo;
            cmdUpdate.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.LrNumber);
            cmdUpdate.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.RoadPermitNo);
            cmdUpdate.Parameters.Add("@SupplierName", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceTO.SupplierName;
            cmdUpdate.Parameters.Add("@BrokerName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.BrokerName);
            cmdUpdate.Parameters.Add("@TransportorName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportorName);
            cmdUpdate.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.TransportorForm);
            cmdUpdate.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.AirwayBillNo);
            cmdUpdate.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.Location);
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.Narration);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.Remark);
            cmdUpdate.Parameters.Add("@PurSchSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.PurSchSummaryId;
            cmdUpdate.Parameters.Add("@EwayBillDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceTO.EwayBillDate;
            cmdUpdate.Parameters.Add("@EwayBillExpiryDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceTO.EwayBillExpiryDate);
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdatePOAndGrrNoForInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseInvoice] SET " +
           " [poId]= @PoId" +
           " ,[grrId]= @GrrId" +
           " WHERE [idInvoicePurchase]= @IdInvoicePurchase";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoicePurchase", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.IdInvoicePurchase;
                cmdUpdate.Parameters.Add("@PoId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.PoId;
                cmdUpdate.Parameters.Add("@GrrId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceTO.GrrId;

                return cmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvoicePurchase, cmdDelete);
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

        public  int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvoicePurchase, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int64 idInvoicePurchase, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoice] " +
            " WHERE idInvoicePurchase = " + idInvoicePurchase +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvoicePurchase", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceTO.IdInvoicePurchase;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
