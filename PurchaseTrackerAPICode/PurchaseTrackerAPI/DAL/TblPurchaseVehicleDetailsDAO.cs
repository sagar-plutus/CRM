using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseVehicleDetailsDAO : ITblPurchaseVehicleDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehicleDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Selection

        #region Methods

        public String SqlSelectQuery()
        {
            // String sqlSelectQry = " SELECT tblPurchaseScheduleDetails.*,tblProductItem.displaySequanceNo,tblProductItem.isNonCommercialItem,tblPurchaseScheduleDetails.recovery as 'recovery',vehicleNo,tblPurchaseScheduleSummary.calculatedMetalCost,tblPurchaseScheduleSummary.baseMetalCost,tblPurchaseScheduleSummary.padta, tblProductItem.itemName, tblProdClassification.prodClassDesc, tblProductItem.prodClassId, tblPurchaseEnquiry.bookingRate FROM [tblPurchaseScheduleDetails]" +
            //                       " INNER JOIN tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
            //                       " INNER JOIN tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
            //                       " INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId" +
            //                       " INNER JOIN tblProdClassification ON tblProdClassification.idProdClass = tblProductItem.prodClassId ";

            String sqlSelectQry = " SELECT tblPurchaseScheduleDetails.*,tblProductItem.displaySequanceNo,tblProductItem.isNonCommercialItem,tblPurchaseScheduleDetails.recovery as 'recovery',vehicleNo,tblPurchaseScheduleSummary.calculatedMetalCost,tblPurchaseScheduleSummary.baseMetalCost,tblPurchaseScheduleSummary.padta, tblProductItem.itemName, tblProdClassification.prodClassDesc, tblProductItem.prodClassId, tblPurchaseEnquiry.bookingRate,tblVariables.variableDisplayName  FROM [tblPurchaseScheduleDetails]" +
                                            " INNER JOIN tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
                                            " INNER JOIN tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                                            " INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId" +
                                            " INNER JOIN tblProdClassification ON tblProdClassification.idProdClass = tblProductItem.prodClassId " +
                                            " LEFT JOIN tblVariables ON tblVariables.idVariable = tblPurchaseScheduleDetails.processVarId "; 


            return sqlSelectQry;
        }

        public String SqlSelectQueryNew()
        {
            String sqlSelectQry = "SELECT distinct scheduleDate, vehicleNo, statusId, statusName, tblPurchaseScheduleSummary.purchaseEnquiryId,tblVariables.variableDisplayName " +
                                  " FROM  tblPurchaseScheduleDetails tblPurchaseScheduleDetails" +
                                  " INNER JOIN tblPurchaseScheduleSummary tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = tblPurchaseScheduleSummary.statusId" +
                                  " LEFT JOIN tblUser ON idUser = tblPurchaseScheduleSummary.createdBy" +
                                  " LEFT JOIN tblVariables ON tblVariables.idVariable = tblPurchaseScheduleDetails.processVarId " ;
            return sqlSelectQry;
        }

        #endregion

        public List<TblPurchaseVehicleDetailsTO> GetVehicleDetailsByEnquiryId(Int32 purchaseEnquiryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string orderBy = " ORDER BY tblProductItem.displaySequanceNo";
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseEnquiry.idPurchaseEnquiry = " + purchaseEnquiryId + orderBy;
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToList(reader);
                reader.Close();
                // reader.Dispose();
                return list;
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

        public List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDetailsList(Int32 schedulePurchaseId, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            SqlDataReader reader = null;
            string orderBy = "  ORDER BY tblProductItem.displaySequanceNo";
            try
            {
                // conn.Open();
                // String sqlSelectQry = " SELECT tblPurchaseScheduleDetails.*,tblProductItem.displaySequanceNo,tblProductItem.isNonCommercialItem,tblPurchaseScheduleDetails.recovery as 'recovery',vehicleNo,tblPurchaseScheduleSummary.calculatedMetalCost,tblPurchaseScheduleSummary.baseMetalCost,tblPurchaseScheduleSummary.padta, tblProductItem.itemName, tblProdClassification.prodClassDesc, tblProductItem.prodClassId, tblPurchaseEnquiry.bookingRate FROM [tblPurchaseScheduleDetails]" +
                //                   " INNER JOIN tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
                //                   " INNER JOIN tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                //                   " INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId" +
                //                   " INNER JOIN tblProdClassification ON tblProdClassification.idProdClass = tblProductItem.prodClassId ";

                String sqlSelectQry = SqlSelectQuery();
                cmdSelect.CommandText = sqlSelectQry + " WHERE purchaseScheduleSummaryId =" + schedulePurchaseId + orderBy;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToList(reader);

                return list;


            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Close();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDetailsList(Int32 schedulePurchaseId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string orderBy = "  ORDER BY tblProductItem.displaySequanceNo";
            try
            {
                conn.Open();
                // String sqlSelectQry = " SELECT tblPurchaseScheduleDetails.*,tblProductItem.displaySequanceNo,tblProductItem.isNonCommercialItem,tblPurchaseScheduleDetails.recovery as 'recovery',vehicleNo,tblPurchaseScheduleSummary.calculatedMetalCost,tblPurchaseScheduleSummary.baseMetalCost,tblPurchaseScheduleSummary.padta, tblProductItem.itemName, tblProdClassification.prodClassDesc, tblProductItem.prodClassId, tblPurchaseEnquiry.bookingRate FROM [tblPurchaseScheduleDetails]" +
                //                   " INNER JOIN tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
                //                   " INNER JOIN tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                //                   " INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId" +
                //                   " INNER JOIN tblProdClassification ON tblProdClassification.idProdClass = tblProductItem.prodClassId ";

                String sqlSelectQry = SqlSelectQuery();

                cmdSelect.CommandText = sqlSelectQry + " WHERE purchaseScheduleSummaryId =" + schedulePurchaseId + " " + orderBy;
                cmdSelect.Connection = conn;
                // cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();

            }
        }

        public List<TblPurchaseVehicleDetailsTO> SelectVehicleItemDetailsByScheduleSummaryIds(string schedulePurchaseIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string orderBy = "  ORDER BY tblProductItem.displaySequanceNo";
            try
            {
                conn.Open();
                // String sqlSelectQry = " SELECT tblPurchaseScheduleDetails.*,tblProductItem.displaySequanceNo,tblProductItem.isNonCommercialItem,tblPurchaseScheduleDetails.recovery as 'recovery',vehicleNo,tblPurchaseScheduleSummary.calculatedMetalCost,tblPurchaseScheduleSummary.baseMetalCost,tblPurchaseScheduleSummary.padta, tblProductItem.itemName, tblProdClassification.prodClassDesc, tblProductItem.prodClassId, tblPurchaseEnquiry.bookingRate FROM [tblPurchaseScheduleDetails]" +
                //                   " INNER JOIN tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
                //                   " INNER JOIN tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                //                   " INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId" +
                //                   " INNER JOIN tblProdClassification ON tblProdClassification.idProdClass = tblProductItem.prodClassId ";

                String sqlSelectQry = SqlSelectQuery();

                cmdSelect.CommandText = sqlSelectQry + " WHERE purchaseScheduleSummaryId IN( " + schedulePurchaseIds + " ) " + orderBy;
                cmdSelect.Connection = conn;
                // cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToList(reader);
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
                conn.Close();

            }
        }
        public List<TblPurchaseVehicleDetailsTO> SelectVehicleItemDetailsByScheduleSummaryIds(string schedulePurchaseIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string orderBy = "  ORDER BY tblProductItem.displaySequanceNo";
            try
            {
                // String sqlSelectQry = " SELECT tblPurchaseScheduleDetails.*,tblProductItem.displaySequanceNo,tblProductItem.isNonCommercialItem,tblPurchaseScheduleDetails.recovery as 'recovery',vehicleNo,tblPurchaseScheduleSummary.calculatedMetalCost,tblPurchaseScheduleSummary.baseMetalCost,tblPurchaseScheduleSummary.padta, tblProductItem.itemName, tblProdClassification.prodClassDesc, tblProductItem.prodClassId, tblPurchaseEnquiry.bookingRate FROM [tblPurchaseScheduleDetails]" +
                //                   " INNER JOIN tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
                //                   " INNER JOIN tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                //                   " INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId" +
                //                   " INNER JOIN tblProdClassification ON tblProdClassification.idProdClass = tblProductItem.prodClassId ";

                String sqlSelectQry = SqlSelectQuery();

                cmdSelect.CommandText = sqlSelectQry + " WHERE purchaseScheduleSummaryId IN( " + schedulePurchaseIds + " ) " + orderBy;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToList(reader);
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
        public List<TblPurchaseVehicleDetailsTO> GetPurchaseScheduleDetailsBySpotEntryVehicleId(Int32 spoEntryVehicleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string orderBy = "  ORDER BY tblProductItem.displaySequanceNo";
            try
            {
                conn.Open();
                // String sqlSelectQry = " SELECT tblPurchaseScheduleDetails.*,tblProductItem.displaySequanceNo,tblProductItem.isNonCommercialItem,tblPurchaseScheduleDetails.recovery as 'recovery',vehicleNo,tblPurchaseScheduleSummary.calculatedMetalCost,tblPurchaseScheduleSummary.baseMetalCost,tblPurchaseScheduleSummary.padta, tblProductItem.itemName, tblProdClassification.prodClassDesc, tblProductItem.prodClassId, tblPurchaseEnquiry.bookingRate FROM [tblPurchaseScheduleDetails]" +
                //                   " INNER JOIN tblPurchaseScheduleSummary ON tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseScheduleDetails.purchaseScheduleSummaryId" +
                //                   " INNER JOIN tblPurchaseEnquiry ON tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId " +
                //                   " INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseScheduleDetails.prodItemId" +
                //                   " INNER JOIN tblProdClassification ON tblProdClassification.idProdClass = tblProductItem.prodClassId ";

                String sqlSelectQry = SqlSelectQuery();

                cmdSelect.CommandText = sqlSelectQry + " WHERE spotEntryVehicleId =" + spoEntryVehicleId + orderBy;
                cmdSelect.Connection = conn;
                // cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();

            }
        }
        public List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsList(DateTime fromDate, String UserId) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT distinct vehicleNo,* FROM tblPurchaseScheduleSummary" +
                                        //" INNER JOIN tblPurchaseEnquirySchedule ON tblPurchaseEnquirySchedule.idSchedulePurchase = tblPurchaseVehicleDetails.schedulePurchaseId"
                                        " WHERE cast(scheduleDate as date) = @fromDate";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToList(reader, true);
                reader.Dispose();
                return list;
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

        public List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsList(String userid, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = "SELECT idSpotEntryVehicle,firmName,SupplierId,vehicleNo,LocationId,userLogin,a.createdBy,a.createdOn,comments FROM [tblSpotEntryVehicleDetails] a INNER JOIN tbluser b on b.idUser=a.createdBy INNER JOIN tblOrganization c ON c.idOrganization=a.SupplierId ORDER BY cast(a.createdOn as date) DESC";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToListNew(reader);
                reader.Dispose();
                if (list != null)
                    return list;
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

        public List<TblPurchaseVehicleDetailsTO> ConvertDTToListNew(SqlDataReader tblPurchaseVehicleDetailsTODT)
        {
            List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
            if (tblPurchaseVehicleDetailsTODT != null)
            {
                while (tblPurchaseVehicleDetailsTODT.Read())
                {
                    TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTONew = new TblPurchaseVehicleDetailsTO();

                    if (tblPurchaseVehicleDetailsTODT["SupplierId"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.SupplierId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["SupplierId"].ToString());
                    if (tblPurchaseVehicleDetailsTODT["LocationId"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.LocationId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["LocationId"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["comments"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.Remark = Convert.ToString(tblPurchaseVehicleDetailsTODT["comments"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["createdBy"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.CreatedBy = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["createdBy"].ToString());
                    if (tblPurchaseVehicleDetailsTODT["createdOn"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseVehicleDetailsTODT["createdOn"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.VehicleNo = Convert.ToString(tblPurchaseVehicleDetailsTODT["vehicleNo"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["firmName"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.FirmName = Convert.ToString(tblPurchaseVehicleDetailsTODT["firmName"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["userLogin"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.Username = Convert.ToString(tblPurchaseVehicleDetailsTODT["userLogin"].ToString());

                    tblPurchaseVehicleDetailsTOList.Add(tblPurchaseVehicleDetailsTONew);
                }
            }
            return tblPurchaseVehicleDetailsTOList;
        }

        public List<TblPurchaseVehicleDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseVehicleDetailsTODT, Boolean isReadAll = true)
        {
            List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
            if (tblPurchaseVehicleDetailsTODT != null)
            {
                while (tblPurchaseVehicleDetailsTODT.Read())
                {
                    TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTONew = new TblPurchaseVehicleDetailsTO();
                    //if (isReadAll)
                    {
                        if (tblPurchaseVehicleDetailsTODT["idPurchaseScheduleDetails"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.IdVehiclePurchase = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["idPurchaseScheduleDetails"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.SchedulePurchaseId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["purchaseScheduleSummaryId"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["prodItemId"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.ProdItemId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["prodItemId"].ToString());

                        if (tblPurchaseVehicleDetailsTODT["Qty"] != DBNull.Value)
                        {
                            tblPurchaseVehicleDetailsTONew.Qty = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["Qty"].ToString());
                        }
                        if (tblPurchaseVehicleDetailsTODT["remark"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.Remark = Convert.ToString(tblPurchaseVehicleDetailsTODT["remark"].ToString());

                        if (tblPurchaseVehicleDetailsTODT["isTransfered"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.IsTransfered = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["isTransfered"].ToString());

                        if (tblPurchaseVehicleDetailsTODT["transferedFrmScheduleId"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.TransferedFrmScheduleId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["transferedFrmScheduleId"].ToString());

                        if (tblPurchaseVehicleDetailsTODT["isEnquiryOrConfirm"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.CorNcId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["isEnquiryOrConfirm"].ToString());

                        // if (tblPurchaseVehicleDetailsTODT["createdBy"] != DBNull.Value)
                        //     tblPurchaseVehicleDetailsTONew.CreatedBy = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["createdBy"].ToString());
                        // if (tblPurchaseVehicleDetailsTODT["createdOn"] != DBNull.Value)
                        //     tblPurchaseVehicleDetailsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseVehicleDetailsTODT["createdOn"].ToString());

                        // if (tblPurchaseVehicleDetailsTODT["updatedBy"] != DBNull.Value)
                        //     tblPurchaseVehicleDetailsTONew.UpdatedBy = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["updatedBy"].ToString());
                        // if (tblPurchaseVehicleDetailsTODT["updatedOn"] != DBNull.Value)
                        //     tblPurchaseVehicleDetailsTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseVehicleDetailsTODT["updatedOn"].ToString());

                        if (tblPurchaseVehicleDetailsTODT["itemName"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.ItemName = Convert.ToString(tblPurchaseVehicleDetailsTODT["itemName"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["prodClassDesc"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.ProdClassDesc = Convert.ToString(tblPurchaseVehicleDetailsTODT["prodClassDesc"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["prodClassId"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.ProdClassId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["prodClassId"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["bookingRate"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.DeclaredRate = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["bookingRate"].ToString());

                        if (tblPurchaseVehicleDetailsTODT["rate"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.Rate = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["rate"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["productAomunt"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.ProductAomunt = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["productAomunt"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["productRecovery"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.ProductRecovery = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["productRecovery"].ToString());
                        if (tblPurchaseVehicleDetailsTODT["vehicleNo"] != DBNull.Value)
                            tblPurchaseVehicleDetailsTONew.VehicleNo = Convert.ToString(tblPurchaseVehicleDetailsTODT["vehicleNo"].ToString());

                    }

                    if (tblPurchaseVehicleDetailsTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseVehicleDetailsTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["baseMetalCost"].ToString());
                    if (tblPurchaseVehicleDetailsTODT["padta"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.Padta = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["padta"].ToString());
                    if (tblPurchaseVehicleDetailsTODT["recovery"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.Recovery = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["recovery"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["isNonCommercialItem"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.IsNonCommercialItem = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["isNonCommercialItem"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["metalCost"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.MetalCost = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["metalCost"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["totalCost"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.TotalCost = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["totalCost"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["totalProduct"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.TotalProduct = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["totalProduct"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["gradePadta"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.GradePadta = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["gradePadta"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["displaySequanceNo"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.DisplaySequanceNo = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["displaySequanceNo"].ToString());


                    if (tblPurchaseVehicleDetailsTODT["purchaseEnqDtlsId"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.PurchaseEnqDtlsId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["purchaseEnqDtlsId"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["itemBookingRate"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.ItemBookingRate = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["itemBookingRate"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["recImpurities"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.RecImpurities = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["recImpurities"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["processVarId"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.ProcessVarId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["processVarId"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["processVarValue"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.ProcessVarValue = Convert.ToDouble(tblPurchaseVehicleDetailsTODT["processVarValue"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["variableDisplayName"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.ProcessVarDisplayName = Convert.ToString(tblPurchaseVehicleDetailsTODT["variableDisplayName"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["purEnqId"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.PurEnqId = Convert.ToInt32(tblPurchaseVehicleDetailsTODT["purEnqId"].ToString());

                    if (tblPurchaseVehicleDetailsTODT["purEnqDisplayNo"] != DBNull.Value)
                        tblPurchaseVehicleDetailsTONew.PurEnqDisplayNo = Convert.ToString(tblPurchaseVehicleDetailsTODT["purEnqDisplayNo"].ToString());


                    tblPurchaseVehicleDetailsTOList.Add(tblPurchaseVehicleDetailsTONew);
                }
            }
            return tblPurchaseVehicleDetailsTOList;
        }

        public List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsByStatus(string statusId, DateTime fromDate, String vehicleNo, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQueryNew() + ")sq1 WHERE sq1.statusId IN(" + statusId + ")";
                if (vehicleNo == "0" || vehicleNo == null)
                    cmdSelect.CommandText = SqlSelectQueryNew() + " WHERE tblPurchaseScheduleSummary.statusId IN(" + statusId + ") AND cast(scheduleDate as date) = @fromDate";
                else
                    cmdSelect.CommandText = SqlSelectQueryNew() + " WHERE tblPurchaseScheduleSummary.statusId IN(" + statusId + ") AND cast(scheduleDate as date) = @fromDate  AND vehicleNo = @vehicleNo";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                if (vehicleNo != "0" && vehicleNo != null)
                    cmdSelect.Parameters.Add("@vehicleNo", System.Data.SqlDbType.NVarChar).Value = vehicleNo;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleDetailsTO> list = ConvertDTToListOld(sqlReader);
                // sqlReader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseVehicleDetailsTO> ConvertDTToListOld(SqlDataReader tblLoadingTODT)
        {
            List<TblPurchaseVehicleDetailsTO> tblLoadingTOList = new List<TblPurchaseVehicleDetailsTO>();
            if (tblLoadingTODT != null)
            {
                while (tblLoadingTODT.Read())
                {
                    TblPurchaseVehicleDetailsTO tblLoadingTONew = new TblPurchaseVehicleDetailsTO();
                    if (tblLoadingTODT["vehicleNo"] != DBNull.Value)
                        tblLoadingTONew.VehicleNo = tblLoadingTODT["vehicleNo"].ToString();
                    if (tblLoadingTODT["scheduleDate"] != DBNull.Value)
                        tblLoadingTONew.CreatedOn = Convert.ToDateTime(tblLoadingTODT["scheduleDate"].ToString());
                    if (tblLoadingTODT["statusName"] != DBNull.Value)
                        tblLoadingTONew.StatusName = tblLoadingTODT["statusName"].ToString();
                    if (tblLoadingTODT["statusId"] != DBNull.Value)
                        tblLoadingTONew.StatusId = Convert.ToInt32(tblLoadingTODT["statusId"].ToString());

                    if (tblLoadingTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblLoadingTONew.PurchaseEnquiryId = Convert.ToInt32(tblLoadingTODT["purchaseEnquiryId"].ToString());

                    tblLoadingTOList.Add(tblLoadingTONew);
                }
            }
            return tblLoadingTOList;
        }

        #endregion

        #region Insertion

        public int InsertTblPurchaseVehicleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseVehicleDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int SaveVehicleSpotEntry(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Close();
                conn.Open();
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandNew(tblPurchaseVehicleDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                //conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO tblPurchaseScheduleDetails ( 
                            [purchaseScheduleSummaryId]
                            ,[prodItemId]
                            ,[qty]
                            ,[rate]
                            ,[productAomunt]
                            ,[productRecovery]
                            ,[remark]
                            ,[IsTransfered]
                            ,[IsEnquiryOrConfirm]
                            ,[TransferedFrmScheduleId] 
                            ,[metalCost]
                             ,[totalCost]
                             ,[totalProduct]
                             ,[gradePadta] 
                             ,[recovery]
                             ,[purchaseEnqDtlsId]
                             ,[itemBookingRate]
                             ,[recImpurities]
                             ,[processVarId]
                             ,[processVarValue] 
                             ,[purEnqId] 
                             ,[purEnqDisplayNo]" +


                              " )" +
                " VALUES (" +
                            "  @SchedulePurchaseId " +
                            " ,@ProdItemId " +
                            //" ,@VehicleNo " +
                            " ,@Qty " +
                            " ,@Rate " +
                            " ,@ProductAomunt " +
                            " ,@ProductRecovery " +
                            " ,@Remark " +
                            " ,@IsTransfered " +
                            " ,@IsEnquiryOrConfirm " +
                            " ,@TransferedFrmScheduleId " +
                            " ,@MetalCost " +
                            " ,@TotalCost " +
                            " ,@TotalProduct " +
                            " ,@GradePadta " +
                            " ,@Recovery " +
                            " ,@PurchaseEnqDtlsId " +
                            " ,@ItemBookingRate " +
                            " ,@RecImpurities " +
                            " ,@ProcessVarId " +
                            " ,@ProcessVarValue " +
                            " ,@PurEnqId " +
                            " ,@PurEnqDisplayNo " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@SchedulePurchaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.SchedulePurchaseId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.ProdItemId);
            //cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.VehicleNo);
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.Qty);
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.Rate);
            cmdInsert.Parameters.Add("@ProductAomunt", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.ProductAomunt);
            cmdInsert.Parameters.Add("@ProductRecovery", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.ProductRecovery);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.Remark);
            cmdInsert.Parameters.Add("@IsTransfered", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.IsTransfered);
            cmdInsert.Parameters.Add("@IsEnquiryOrConfirm", System.Data.SqlDbType.Int).Value = (tblPurchaseVehicleDetailsTO.CorNcId);
            cmdInsert.Parameters.Add("@TransferedFrmScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.TransferedFrmScheduleId);
            cmdInsert.Parameters.Add("@MetalCost", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.MetalCost);
            cmdInsert.Parameters.Add("@TotalCost", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.TotalCost);
            cmdInsert.Parameters.Add("@TotalProduct", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.TotalProduct);
            cmdInsert.Parameters.Add("@GradePadta", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.GradePadta);
            cmdInsert.Parameters.Add("@Recovery", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.Recovery);
            cmdInsert.Parameters.Add("@PurchaseEnqDtlsId", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.PurchaseEnqDtlsId);
            cmdInsert.Parameters.Add("@ItemBookingRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.ItemBookingRate);
            cmdInsert.Parameters.Add("@RecImpurities", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.RecImpurities);
            cmdInsert.Parameters.Add("@ProcessVarId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.ProcessVarId);
            cmdInsert.Parameters.Add("@ProcessVarValue", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.ProcessVarValue);
            cmdInsert.Parameters.Add("@PurEnqId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.PurEnqId);
            cmdInsert.Parameters.Add("@PurEnqDisplayNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.PurEnqDisplayNo);

            //cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleDetailsTO.CreatedBy;
            //cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseVehicleDetailsTO.CreatedOn;
            //cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.UpdatedBy);
            //cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.UpdatedOn);
            //cmdInsert.Parameters.Add("@Recovery", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.Recovery);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblPurchaseVehicleDetailsTO.IdVehiclePurchase = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        public int ExecuteInsertionCommandNew(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [tblSpotEntryVehicleDetails]( " +
                  " [SupplierId]" +
                  " ,[vehicleNo]" +
                  " ,[LocationId]" +
                  " ,[createdBy]" +
                  " ,[createdOn]" +
                  " ,[comments]" +
                  " )" +
                " VALUES (" +
                            "  @SupplierId " +
                            " ,@vehicleNo " +
                            " ,@LocationId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@comments " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            cmdInsert.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.SupplierId);
            cmdInsert.Parameters.Add("@vehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.VehicleNo);
            cmdInsert.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.LocationId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseVehicleDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.Remark);
            return cmdInsert.ExecuteNonQuery();
        }

        #endregion

        #region Updation
        public int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseVehicleDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseVehicleDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseScheduleDetails] SET " +
            //"  [idPurchaseScheduleDetails] = @IdPurchaseScheduleDetails" +
            "  [purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[qty]= @Qty" +
            " ,[rate]= @Rate" +
            " ,[productAomunt]= @ProductAomunt" +
            " ,[productRecovery]= @ProductRecovery" +
            " ,[remark] = @Remark" +
            " ,[isTransfered] = @IsTransfered" +
            " ,[isEnquiryOrConfirm] = @IsEnquiryOrConfirm" +
            " ,[transferedFrmScheduleId] = @TransferedFrmScheduleId" +
            " ,[metalCost] = @MetalCost" +
            " ,[totalCost] = @TotalCost" +
            " ,[totalProduct] = @TotalProduct" +
            " ,[gradePadta] = @GradePadta" +
            " ,[recovery] = @Recovery" +
            " ,[purchaseEnqDtlsId] = @PurchaseEnqDtlsId" +
            " ,[itemBookingRate] = @ItemBookingRate" +
            " ,[recImpurities] = @RecImpurities" +
            " ,[processVarId] = @ProcessVarId" +
            " ,[processVarValue] = @ProcessVarValue" +
            " ,[purEnqId] = @PurEnqId" +
            " ,[purEnqDisplayNo] = @PurEnqDisplayNo" +
            " WHERE idPurchaseScheduleDetails = @IdPurchaseScheduleDetails ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseScheduleDetails", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleDetailsTO.IdVehiclePurchase;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleDetailsTO.SchedulePurchaseId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleDetailsTO.ProdItemId;
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehicleDetailsTO.Qty;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehicleDetailsTO.Rate;
            cmdUpdate.Parameters.Add("@ProductAomunt", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehicleDetailsTO.ProductAomunt;
            cmdUpdate.Parameters.Add("@ProductRecovery", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehicleDetailsTO.ProductRecovery;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.Remark);
            cmdUpdate.Parameters.Add("@IsTransfered", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.IsTransfered);
            cmdUpdate.Parameters.Add("@IsEnquiryOrConfirm", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.CorNcId);
            cmdUpdate.Parameters.Add("@TransferedFrmScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.TransferedFrmScheduleId);
            cmdUpdate.Parameters.Add("@MetalCost", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.MetalCost);
            cmdUpdate.Parameters.Add("@TotalCost", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.TotalCost);
            cmdUpdate.Parameters.Add("@TotalProduct", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.TotalProduct);
            cmdUpdate.Parameters.Add("@GradePadta", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.GradePadta);
            cmdUpdate.Parameters.Add("@Recovery", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.Recovery);
            cmdUpdate.Parameters.Add("@PurchaseEnqDtlsId", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.PurchaseEnqDtlsId);
            cmdUpdate.Parameters.Add("@ItemBookingRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.ItemBookingRate);
            cmdUpdate.Parameters.Add("@RecImpurities", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.RecImpurities);
            cmdUpdate.Parameters.Add("@ProcessVarId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.ProcessVarId);
            cmdUpdate.Parameters.Add("@ProcessVarValue", System.Data.SqlDbType.Decimal).Value = (tblPurchaseVehicleDetailsTO.ProcessVarValue);
            cmdUpdate.Parameters.Add("@PurEnqId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.PurEnqId);
            cmdUpdate.Parameters.Add("@PurEnqDisplayNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleDetailsTO.PurEnqDisplayNo);

            return cmdUpdate.ExecuteNonQuery();
        }


        #endregion

        #region Deletion

        public int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSchedulePurchase, cmdDelete);
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

        public int DeleteTblPurchaseVehicleDetails2(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                //conn.Open();
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand2(purchaseScheduleSummaryId, cmdDelete);
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
        public int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase)
        {
            SqlCommand cmdDelete = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSchedulePurchase, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idSchedulePurchase, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseScheduleDetails] " +
            " WHERE idPurchaseScheduleDetails = " + idSchedulePurchase + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            return cmdDelete.ExecuteNonQuery();
        }
        public int ExecuteDeletionCommand2(Int32 purchaseScheduleSummaryId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseScheduleDetails] " +
            " WHERE purchaseScheduleSummaryId = " + purchaseScheduleSummaryId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllVehicleItemDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseScheduleDetails WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                return cmdDelete.ExecuteNonQuery();
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

        #endregion
    }
}
