using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using System.Globalization;

namespace PurchaseTrackerAPI.DAL
{
    public class DimReportTemplateDAO : IDimReportTemplateDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimReportTemplateDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimReportTemplate]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection

        public List<TblPurchaseEnquiryTO> GetAllEnquiryList(DateTime fromDate, DateTime toDate,String purchaseMangerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT itemlistTable.items,tblUser.userDisplayName AS purchaseManagerName,tblProdClassification.prodClassDesc,tblOrganization.firmName AS " +
                    " supplierName,dimStatus.StatusName,tblPurchaseEnquiry.currencyId,dimCurrency.currnecyCode,CASE WHEN ISNULL(tblPurchaseEnquiry.isFixed,0)=0 THEN 'Per MT' ELSE 'Fixed' END AS freightInFixedOrMT , " +
                    " CASE WHEN ISNULL(tblPurchaseEnquiry.cOrNCId,0)=0 THEN 'Enquiry' ELSE 'Order' END AS billType,tblPurchaseEnquiry.* FROM tblPurchaseEnquiry tblPurchaseEnquiry " +
                    " LEFT JOIN tblUser tblUser ON tblUser.iduser = tblPurchaseEnquiry.userId " +
                    " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass = tblPurchaseEnquiry.prodClassId " +
                    " LEFT JOIN tblOrganization tblOrganization ON tblOrganization.idOrganization = tblPurchaseEnquiry.SupplierId " +
                    " LEFT JOIN dimStatus ON dimStatus.idStatus = tblPurchaseEnquiry.statusId  " +
                    " LEFT JOIN dimCurrency dimCurrency ON dimCurrency.idCurrency = tblPurchaseEnquiry.currencyId  " +
                    " LEFT JOIN ( SELECT enq.idPurchaseEnquiry , STUFF((SELECT '; ' + item.itemName FROM tblProductItem item left join tblPurchaseEnquiryDetails " +
                    " enqdtls on enqdtls.prodItemId = item.idProdItem WHERE enqdtls.purchaseEnquiryId = enq.idPurchaseEnquiry FOR XML PATH('')), 1, 1, '') " +
                    " [items] FROM tblPurchaseEnquiry enq GROUP BY enq.idPurchaseEnquiry) itemlistTable ON " +
                    " itemlistTable.idPurchaseEnquiry = tblPurchaseEnquiry.idPurchaseEnquiry" +
                    " WHERE ISNULL(tblPurchaseEnquiry.isConvertToSauda,0)=1 " +
                    " AND CAST(tblPurchaseEnquiry.saudaCreatedOn AS DATE) BETWEEN @fromDate AND  @toDate ";
                if (!String.IsNullOrEmpty(purchaseMangerIds))
                {
                    cmdSelect.CommandText += " AND tblPurchaseEnquiry.userId IN (" + purchaseMangerIds + ")";
                }
                  cmdSelect.CommandText += " order by idPurchaseEnquiry desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToListEnquiry(rdr);
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


        public List<TblPurchaseEnquiryTO> ConvertDTToListEnquiry(SqlDataReader tblPurchaseEnquiryTODT)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = new List<TblPurchaseEnquiryTO>();
            if (tblPurchaseEnquiryTODT != null)
            {
                while (tblPurchaseEnquiryTODT.Read())
                {
                    TblPurchaseEnquiryTO tblPurchaseEnquiryTONew = new TblPurchaseEnquiryTO();
                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    //Deepali
                    if (tblPurchaseEnquiryTODT["currencyId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CurrencyId = Convert.ToInt32(tblPurchaseEnquiryTODT["currencyId"].ToString());
                    if (tblPurchaseEnquiryTODT["currnecyCode"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Currency = tblPurchaseEnquiryTODT["currnecyCode"].ToString();
                                       
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UserId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.COrNCId = Convert.ToInt32(tblPurchaseEnquiryTODT["cOrNCId"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SupplierId = Convert.ToInt32(tblPurchaseEnquiryTODT["supplierId"].ToString());
                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["isConfirmed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConfirmed = Convert.ToInt32(tblPurchaseEnquiryTODT["isConfirmed"].ToString());
                    if (tblPurchaseEnquiryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassId = Convert.ToInt32(tblPurchaseEnquiryTODT["prodClassId"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["padta"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Padta = Convert.ToDouble(tblPurchaseEnquiryTODT["padta"].ToString());
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["updatedOn"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());
                    if (tblPurchaseEnquiryTODT["isConvertToSauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConvertToSauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isConvertToSauda"].ToString());

                    if (tblPurchaseEnquiryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassDesc = Convert.ToString(tblPurchaseEnquiryTODT["prodClassDesc"].ToString());
                    if (tblPurchaseEnquiryTODT["supplierName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SupplierName = Convert.ToString(tblPurchaseEnquiryTODT["supplierName"].ToString());
                    if (tblPurchaseEnquiryTODT["purchaseManagerName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PurchaseManagerName = Convert.ToString(tblPurchaseEnquiryTODT["purchaseManagerName"].ToString());

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    if (tblPurchaseEnquiryTODT["isOpenQtySauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsOpenQtySauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isOpenQtySauda"].ToString());

                    if (tblPurchaseEnquiryTODT["pendingBookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PendingBookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["pendingBookingQty"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["isSpotedVehicle"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsSpotedVehicle = Convert.ToInt32(tblPurchaseEnquiryTODT["isSpotedVehicle"].ToString());

                    if (tblPurchaseEnquiryTODT["saudaCreatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SaudaCreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["saudaCreatedOn"].ToString());

                    if (tblPurchaseEnquiryTODT["saudaCreatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SaudaCreatedOnString = tblPurchaseEnquiryTONew.SaudaCreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);                  

                    //Priyanka [03-01-2019]
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());

                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRateId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["SupplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DealerOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["SupplierId"].ToString());
                    //Need To check....
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CnFOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingPmRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Bookingpmrate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingPmRate"].ToString());

                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["deliveryDays"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DeliveryDays = Convert.ToInt32(tblPurchaseEnquiryTODT["deliveryDays"].ToString());

                    if (tblPurchaseEnquiryTODT["noOfVehicleSched"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.NoOfVehicleSched = Convert.ToInt32(tblPurchaseEnquiryTODT["noOfVehicleSched"].ToString());

                    if (tblPurchaseEnquiryTODT["remark"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Remark = Convert.ToString(tblPurchaseEnquiryTODT["remark"].ToString());

                    if (tblPurchaseEnquiryTODT["StatusName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusName = Convert.ToString(tblPurchaseEnquiryTODT["StatusName"].ToString());

                    if (tblPurchaseEnquiryTODT["freight"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Freight = Convert.ToDouble(tblPurchaseEnquiryTODT["freight"].ToString());

                    if (tblPurchaseEnquiryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsFixed = Convert.ToInt32(tblPurchaseEnquiryTODT["isFixed"].ToString());

                    if (tblPurchaseEnquiryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseEnquiryTODT["transportAmtPerMT"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForC"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForNC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForNC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForNC"].ToString());

                    if (tblPurchaseEnquiryTODT["consumedQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ConsumedQty = Convert.ToDouble(tblPurchaseEnquiryTODT["consumedQty"].ToString());

                    if (tblPurchaseEnquiryTODT["freightInFixedOrMT"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FreightInFixedOrMT = tblPurchaseEnquiryTODT["freightInFixedOrMT"].ToString();

                    if (tblPurchaseEnquiryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.VehicleTypeDesc = tblPurchaseEnquiryTODT["vehicleTypeDesc"].ToString();

                    if (tblPurchaseEnquiryTODT["billType"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BillType = tblPurchaseEnquiryTODT["billType"].ToString();

                    if (tblPurchaseEnquiryTODT["items"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Grades = tblPurchaseEnquiryTODT["items"].ToString();

                    if (tblPurchaseEnquiryTODT["optionalPendingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.OptionalPendingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["optionalPendingQty"].ToString());

                    if (tblPurchaseEnquiryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.VehicleTypeDesc = Convert.ToString(tblPurchaseEnquiryTODT["vehicleTypeDesc"].ToString());

                    tblPurchaseEnquiryTOList.Add(tblPurchaseEnquiryTONew);
                }
            }
            return tblPurchaseEnquiryTOList;
        }

        public List<DimReportTemplateTO> SelectAllDimReportTemplate()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
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

        public DimReportTemplateTO SelectDimReportTemplate(Int32 idReport)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idReport = " + idReport +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
               // String userName = System.Windows.Forms.SystemInformation.UserName;
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimReportTemplateTO> ConvertDTToList(SqlDataReader dimReportTemplateTODT)
        {
            List<DimReportTemplateTO> dimReportTemplateTOList = new List<DimReportTemplateTO>();
            if (dimReportTemplateTODT != null)
            {
                while (dimReportTemplateTODT.Read())
                {
                    DimReportTemplateTO dimReportTemplateTONew = new DimReportTemplateTO();
                    if (dimReportTemplateTODT ["idReport"] != DBNull.Value)
                        dimReportTemplateTONew.IdReport = Convert.ToInt32(dimReportTemplateTODT ["idReport"].ToString());
                    if (dimReportTemplateTODT ["isDisplayMultisheetAllow"] != DBNull.Value)
                        dimReportTemplateTONew.IsDisplayMultisheetAllow = Convert.ToInt32(dimReportTemplateTODT ["isDisplayMultisheetAllow"].ToString());
                    if (dimReportTemplateTODT ["createdBy"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedBy = Convert.ToInt32(dimReportTemplateTODT ["createdBy"].ToString());
                    if (dimReportTemplateTODT ["createdOn"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedOn = Convert.ToDateTime(dimReportTemplateTODT ["createdOn"].ToString());
                    if (dimReportTemplateTODT ["reportName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportName = Convert.ToString(dimReportTemplateTODT ["reportName"].ToString());
                    if (dimReportTemplateTODT ["reportFileName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileName = Convert.ToString(dimReportTemplateTODT ["reportFileName"].ToString());
                    if (dimReportTemplateTODT ["reportFileExtension"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileExtension = Convert.ToString(dimReportTemplateTODT ["reportFileExtension"].ToString());
                    if (dimReportTemplateTODT ["reportPassword"] != DBNull.Value)
                        dimReportTemplateTONew.ReportPassword = Convert.ToString(dimReportTemplateTODT ["reportPassword"].ToString());
                    dimReportTemplateTOList.Add(dimReportTemplateTONew);
                }
            }
            return dimReportTemplateTOList;
        }


        public DimReportTemplateTO SelectDimReportTemplate(String reportName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate WHERE reportName = '" + reportName + "' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@report_name", System.Data.SqlDbType.NVarChar).Value = mstReportTemplateTO.ReportName;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                //String userName = System.Windows.Forms.SystemInformation.UserName;
                //MessageBox.Show("Computer Name:" + computerName + Environment.NewLine + "User Name:" + userName + Environment.NewLine + "Class Name: MstReportTemplateDAO" + Environment.NewLine + "Method Name:SelectMstReportTemplate(MstReportTemplateTO mstReportTemplateTO)" + Environment.NewLine + "Exception Message:" + ex.Message.ToString() + "");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// KISHOR [2014-11-28] Add with conn tran
        /// </summary>
        /// <param name="reportFileName"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<DimReportTemplateTO> isVisibleAllowMultisheetReportList(string reportFileName, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate " +
                                    " WHERE reportFileName = '" + reportFileName + "' AND isDisplayMultisheetAllow ='true'";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                //  VegaERPFrameWork.VErrorList.Add("Error in Class-MstReportTemplateDAO, Method-isVisibleAllowMultisheetReport(string reportFileName, SqlConnection conn, SqlTransaction tran)", VegaERPFrameWork.EMessageType.Error, ex, null);
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                if (rdr != null) rdr.Dispose();
            }
        }

        // <summary>
        /// isVisibleAllowMultisheetReport for display multisheet Report in PDF
        /// </summary>
        /// <param name="mstReportTemplateTO"></param>
        /// <returns></returns>

        public List<DimReportTemplateTO> isVisibleAllowMultisheetReportList(string reportFileName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate " +
                                    " WHERE reportFileName = '" + reportFileName + "' AND isDisplayMultisheetAllow =1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
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
                if (rdr != null) rdr.Dispose();
            }
        }


        #endregion

        #region Insertion
        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimReportTemplateTO, cmdInsert);
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

        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimReportTemplateTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimReportTemplateTO dimReportTemplateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimReportTemplate]( " + 
            "  [idReport]" +
            " ,[isDisplayMultisheetAllow]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[reportName]" +
            " ,[reportFileName]" +
            " ,[reportFileExtension]" +
            " ,[reportPassword]" +
            " )" +
" VALUES (" +
            "  @IdReport " +
            " ,@IsDisplayMultisheetAllow " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@ReportName " +
            " ,@ReportFileName " +
            " ,@ReportFileExtension " +
            " ,@ReportPassword " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
            cmdInsert.Parameters.Add("@IsDisplayMultisheetAllow", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IsDisplayMultisheetAllow;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimReportTemplateTO.CreatedOn;
            cmdInsert.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportName;
            cmdInsert.Parameters.Add("@ReportFileName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileName;
            cmdInsert.Parameters.Add("@ReportFileExtension", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileExtension;
            cmdInsert.Parameters.Add("@ReportPassword", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportPassword;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimReportTemplateTO, cmdUpdate);
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

        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimReportTemplateTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimReportTemplateTO dimReportTemplateTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimReportTemplate] SET " + 
            "  [idReport] = @IdReport" +
            " ,[isDisplayMultisheetAllow]= @IsDisplayMultisheetAllow" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[reportName]= @ReportName" +
            " ,[reportFileName]= @ReportFileName" +
            " ,[reportFileExtension]= @ReportFileExtension" +
            " ,[reportPassword] = @ReportPassword" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
            cmdUpdate.Parameters.Add("@IsDisplayMultisheetAllow", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IsDisplayMultisheetAllow;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimReportTemplateTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportName;
            cmdUpdate.Parameters.Add("@ReportFileName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileName;
            cmdUpdate.Parameters.Add("@ReportFileExtension", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileExtension;
            cmdUpdate.Parameters.Add("@ReportPassword", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportPassword;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimReportTemplate(Int32 idReport)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReport, cmdDelete);
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

        public int DeleteDimReportTemplate(Int32 idReport, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReport, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idReport, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimReportTemplate] " +
            " WHERE idReport = " + idReport +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
