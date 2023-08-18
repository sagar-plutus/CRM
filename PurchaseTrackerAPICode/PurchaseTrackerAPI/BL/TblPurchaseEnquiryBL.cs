using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.DashboardModels;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseEnquiryBL : ITblPurchaseEnquiryBL
    {
        #region Selection
        private readonly INotification notify;
        private readonly ITblPurchaseVehicleDetailsDAO _iTblPurchaseVehicleDetailsDAO;
        private readonly ITblGradeWiseTargetQtyBL _iTblGradeWiseTargetQtyBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ITblGlobalRatePurchaseBL _iTblGlobalRatePurchaseBL;
        private readonly ITblPurchaseBookingActionsBL _iTblPurchaseBookingActionsBL;
        private readonly ITblGradeExpressionDtlsBL _iTblGradeExpressionDtlsBL;
        private readonly ITblPurchaseEnquiryDetailsBL _iTblPurchaseEnquiryDetailsBL;
        private readonly ITblPurchaseEnquiryScheduleBL _iTblPurchaseEnquiryScheduleBL;
        private readonly ITblPurchaseEnquiryDAO _iTblPurchaseEnquiryDAO;
        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        private readonly Idimensionbl _idimensionBL;
        private readonly ITblPurchaseEnquiryDetailsDAO _iTblPurchaseEnquiryDetailsDAO;
        private readonly ITblPurchaseParityDetailsBL _iTblPurchaseParityDetailsBL;
        private readonly ITblPurchaseVehicleSpotEntryBL _iTblPurchaseVehicleSpotEntryBL;
        private readonly ITblSpotVehicleMaterialDtlsBL _iTblSpotVehicleMaterialDtlsBL;
        private readonly ITblRateBandDeclarationPurchaseBL _iTblRateBandDeclarationPurchaseBL;
        private readonly ITblPurchaseVehicleDetailsBL _iTblPurchaseVehicleDetailsBL;
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblVariablesBL _iTblVariablesBL;
        private readonly ITblPurchaseEnquiryHistoryDAO _iTblPurchaseEnquiryHistoryDAO;
        private readonly ITblPurchaseManagerSupplierBL _iTblPurchaseManagerSupplierBL;
        private readonly ITblPurchaseEnqVehDescBL _iTblPurchaseEnqVehDescBL;

        private readonly ITblpurchaseEnqShipmemtDtlsBL _iTblpurchaseEnqShipmemtDtlsBL;
        private readonly ITblpurchaseEnqShipmemtDtlsExtBL _iITblpurchaseEnqShipmemtDtlsExtBL;


        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly IRunReport _iRunReport;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblProdClassificationBL _iTblProdClassificationBL;
        private static readonly object EnquiryLock = new object();
        public TblPurchaseEnquiryBL(ITblVariablesBL iTblVariablesBL,
                                    IConnectionString iConnectionString, Icommondao icommondao
                                    , ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL,
                                     Idimensionbl idimensionbl
                                    , ITblPurchaseEnquiryDAO iTblPurchaseEnquiryDAO
                                    , ITblPurchaseEnquiryDetailsDAO iTblPurchaseEnquiryDetailsDAO
                                     , ITblGradeExpressionDtlsBL iTblGradeExpressionDtlsBL
                                    , ITblPurchaseBookingActionsBL iTblPurchaseBookingActionsBL
                                    , ITblGlobalRatePurchaseBL iTblGlobalRatePurchaseBL,
                                      ITblOrganizationBL iTblOrganizationBL
                                    , INotification notification
                                    , ITblConfigParamsBL iTblConfigParamsBL
                                    , ITblGradeWiseTargetQtyBL iTblGradeWiseTargetQtyBL
                                    , ITblPurchaseVehicleDetailsDAO iTblPurchaseVehicleDetailsDAO
                                    , ITblPurchaseEnquiryDetailsBL iTblPurchaseEnquiryDetailsBL
                                    , ITblPurchaseParityDetailsBL iTblPurchaseParityDetailsBL
                                    , ITblPurchaseVehicleSpotEntryBL iTblPurchaseVehicleSpotEntryBL
                                    , ITblSpotVehicleMaterialDtlsBL iTblSpotVehicleMaterialDtlsBL
                                    , ITblRateBandDeclarationPurchaseBL iTblRateBandDeclarationPurchaseBL
                                   , ITblPurchaseVehicleDetailsBL iTblPurchaseVehicleDetailsBL
                                    , ITblPurchaseEnquiryScheduleBL iTblPurchaseEnquiryScheduleBL
                                    , ITblPurchaseEnquiryHistoryDAO iTblPurchaseEnquiryHistoryDAO
                                    , ITblPurchaseManagerSupplierBL iTblPurchaseManagerSupplierBL
                                    ,ITblPurchaseEnqVehDescBL iTblPurchaseEnqVehDescBL
                                    , ITblpurchaseEnqShipmemtDtlsBL iTblpurchaseEnqShipmemtDtlsBL
                                     , ITblpurchaseEnqShipmemtDtlsExtBL iITblpurchaseEnqShipmemtDtlsExtBL
            , IDimReportTemplateBL iDimReportTemplateBL
            , IRunReport iRunReport
            , ITblConfigParamsDAO iTblConfigParamsDAO
            , ITblProdClassificationBL iTblProdClassificationBL
                                    )
        {
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iRunReport = iRunReport;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            notify = notification;
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblPurchaseEnquiryScheduleBL = iTblPurchaseEnquiryScheduleBL;
            _iTblPurchaseVehicleDetailsBL = iTblPurchaseVehicleDetailsBL;
            _iTblRateBandDeclarationPurchaseBL = iTblRateBandDeclarationPurchaseBL;
            _iTblSpotVehicleMaterialDtlsBL = iTblSpotVehicleMaterialDtlsBL;
            _iTblPurchaseVehicleSpotEntryBL = iTblPurchaseVehicleSpotEntryBL;
            _iTblPurchaseParityDetailsBL = iTblPurchaseParityDetailsBL;
            _iTblPurchaseEnquiryDetailsBL = iTblPurchaseEnquiryDetailsBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblGlobalRatePurchaseBL = iTblGlobalRatePurchaseBL;
            _iTblPurchaseBookingActionsBL = iTblPurchaseBookingActionsBL;
            _iTblGradeExpressionDtlsBL = iTblGradeExpressionDtlsBL;
            _iTblPurchaseEnquiryDetailsDAO = iTblPurchaseEnquiryDetailsDAO;
            _iTblPurchaseEnquiryDAO = iTblPurchaseEnquiryDAO;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _idimensionBL = idimensionbl;
            _iTblGradeWiseTargetQtyBL = iTblGradeWiseTargetQtyBL;
            _iTblPurchaseVehicleDetailsDAO = iTblPurchaseVehicleDetailsDAO;
            _iTblPurchaseEnquiryHistoryDAO = iTblPurchaseEnquiryHistoryDAO;
            _iTblPurchaseManagerSupplierBL = iTblPurchaseManagerSupplierBL;
            _iTblVariablesBL = iTblVariablesBL;
            _iTblPurchaseEnqVehDescBL = iTblPurchaseEnqVehDescBL;
            _iTblpurchaseEnqShipmemtDtlsBL = iTblpurchaseEnqShipmemtDtlsBL;
            _iITblpurchaseEnqShipmemtDtlsExtBL = iITblpurchaseEnqShipmemtDtlsExtBL;
            _iTblProdClassificationBL = iTblProdClassificationBL;
        }

        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiry(Int32 idPurchaseEnquiry)
        {
            TblPurchaseEnquiryTO tblPurchaseEnquiryTO = _iTblPurchaseEnquiryDAO.SelectTblPurchaseEnquiry(idPurchaseEnquiry);
            //get supplier wise stateid
            // Int32 stateId = BL.TblPurchaseManagerSupplierBL.GetSupplierStateId(tblPurchaseEnquiryTO.SupplierId);
            if (tblPurchaseEnquiryTO != null)
            {

                tblPurchaseEnquiryTO.TblPurchaseEnqVehDescTOList = _iTblPurchaseEnqVehDescBL.SelectAllTblPurchaseEnqVehDesc(tblPurchaseEnquiryTO.IdPurchaseEnquiry);

                //Prajakta[2019-01-10] Added to get wt rate details for enquiry
                List<TblPurchaseEnquiryHistoryTO> enquiryHistoryTOList = _iTblPurchaseEnquiryHistoryDAO.SelectAllStatusHistoryOfBookingDetails(tblPurchaseEnquiryTO.IdPurchaseEnquiry);
                if (enquiryHistoryTOList != null && enquiryHistoryTOList.Count > 0)
                {
                    tblPurchaseEnquiryTO.WtActualRate = enquiryHistoryTOList[enquiryHistoryTOList.Count - 1].WtActualRate;
                }

                List<TblPurchaseEnquiryDetailsTO> tblEnquiryDetailsTOList = _iTblPurchaseEnquiryDetailsDAO.SelectAllTblEnquiryDetailsList(idPurchaseEnquiry, tblPurchaseEnquiryTO.StateId);
                if (tblEnquiryDetailsTOList == null || tblEnquiryDetailsTOList.Count == 0)
                    tblEnquiryDetailsTOList = new List<TblPurchaseEnquiryDetailsTO>();


                //Get Booking Parity details
                if (tblEnquiryDetailsTOList != null && tblEnquiryDetailsTOList.Count > 0)
                {
                    //Prajakta[2019-04-05] Added to get grade expression details List
                    _iTblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblEnquiryDetailsTOList);

                    string prodItemIds = string.Join(", ", tblEnquiryDetailsTOList.Select(i => i.ProdItemId));

                    if (!string.IsNullOrEmpty(prodItemIds))
                    {
                        List<TblPurchaseParityDetailsTO> parityList = _iTblPurchaseParityDetailsBL.GetBookingItemsParityDtls(prodItemIds, tblPurchaseEnquiryTO.SaudaCreatedOn, tblPurchaseEnquiryTO.StateId);
                        if (parityList != null && parityList.Count > 0)
                        {
                            for (int k = 0; k < tblEnquiryDetailsTOList.Count; k++)
                            {
                                List<TblPurchaseParityDetailsTO> resList = parityList.Where(s => s.ProdItemId == tblEnquiryDetailsTOList[k].ProdItemId).ToList();
                                if (resList != null && resList.Count > 0)
                                {
                                    TblPurchaseParityDetailsTO parityDetailsTO = resList[0];
                                    tblEnquiryDetailsTOList[k].ParityAmt = parityDetailsTO.ParityAmt;
                                    tblEnquiryDetailsTOList[k].NonConfParityAmt = parityDetailsTO.NonConfParityAmt;
                                    if (tblPurchaseEnquiryTO.IsConvertToSauda == 0)
                                        tblEnquiryDetailsTOList[k].Recovery = parityDetailsTO.Recovery;
                                }

                            }
                        }

                    }

                    // for (int q = 0; q < tblEnquiryDetailsTOList.Count; q++)
                    // {

                    //     // DateTime date=new DateTime();
                    //     // string d = tblPurchaseEnquiryTO.CreatedOn.ToString();
                    //     // if (Constants.IsDateTime(d))
                    //     //     date = Convert.ToDateTime(Convert.ToDateTime(d).ToString(Constants.AzureDateFormat));




                    //     List<TblPurchaseParityDetailsTO> parityList = BL.TblPurchaseParityDetailsBL.GetBookingItemsParityDtls(tblEnquiryDetailsTOList[q].ProdItemId, tblPurchaseEnquiryTO.SaudaCreatedOn);
                    //     if (parityList != null && parityList.Count > 0)
                    //     {
                    //         TblPurchaseParityDetailsTO parityDetailsTO = parityList[0];
                    //         tblEnquiryDetailsTOList[q].ParityAmt = parityDetailsTO.ParityAmt;
                    //         tblEnquiryDetailsTOList[q].NonConfParityAmt = parityDetailsTO.NonConfParityAmt;
                    //     }

                    //     //Get Grade Expression Details
                    //     // tblEnquiryDetailsTOList[q].GradeExpressionDtlsTOList = _iTblGradeExpressionDtlsBL.SelectGradeExpressionDtls(tblEnquiryDetailsTOList[q].IdPurchaseEnquiryDetails.ToString());
                    //     // if (tblEnquiryDetailsTOList[q].GradeExpressionDtlsTOList == null)
                    //     //     tblEnquiryDetailsTOList[q].GradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();

                    // }
                }

                tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList = tblEnquiryDetailsTOList;

                tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO = new TblPurchaseVehicleSpotEntryTO();
                if (tblPurchaseEnquiryTO.VehicleSpotEntryId > 0)
                {
                    tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO = _iTblPurchaseVehicleSpotEntryBL.SelectTblPurchaseVehicleSpotEntryTO(tblPurchaseEnquiryTO.VehicleSpotEntryId);
                }
                else
                {
                    List<TblPurchaseVehicleSpotEntryTO> tempSpotList = _iTblPurchaseVehicleSpotEntryBL.SelectTblPurchaseEnqVehEntryTOList(tblPurchaseEnquiryTO.IdPurchaseEnquiry);
                    if (tempSpotList != null && tempSpotList.Count != 0)
                    {
                        tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO = tempSpotList[0];
                    }
                    else
                    {
                        //tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO = new TblPurchaseVehicleSpotEntryTO();
                        tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO = null;

                    }

                    if (tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO != null)
                    {
                        tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO.SpotVehMatDtlsTOList = _iTblSpotVehicleMaterialDtlsBL.SelectAllSpotVehMatDtlsBySpotVehId(tblPurchaseEnquiryTO.PurchaseVehicleSpotEntryTO.IdVehicleSpotEntry);
                    }
                }

                //Get ScheduleList
                // List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = BL.TblPurchaseScheduleSummaryBL.GetScheduleDetailsByPurchaseEnquiryIdForDisplay(tblPurchaseEnquiryTO.IdPurchaseEnquiry);
                // if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                // {
                //     tblPurchaseEnquiryTO.BookingScheduleTOList = tblPurchaseScheduleSummaryTOList;
                // }

                // List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = BL.TblPurchaseScheduleSummaryBL.SelectAllVehicleDetailsListForPurchaseEnquiry(tblPurchaseEnquiryTO.IdPurchaseEnquiry);
                // if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                // {

                //     for (int i = 0; i < tblPurchaseScheduleSummaryTOList.Count; i++)
                //     {

                //         TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = tblPurchaseScheduleSummaryTOList[i];
                //         List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();

                //         //Prajakta[2019-04-05] Added to get new schedule qty
                //         List<TblPurchaseScheduleSummaryTO> tempTOList=BL.TblPurchaseScheduleSummaryBL.SelectVehicleScheduleByRootAndStatusId(tblPurchaseScheduleSummaryTO.ActualRootScheduleId,(Int32)Constants.TranStatusE.New,0);
                //         if(tempTOList!=null && tempTOList.Count==1)
                //         {
                //             tblPurchaseScheduleSummaryTO.Qty=tempTOList[0].Qty;
                //         }

                //         //Prajakta [2019-01-14] schedule latest records are shown
                //         if (tblPurchaseScheduleSummaryTO.RootScheduleId > 0)
                //         {
                //             tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.RootScheduleId);

                //         }
                //         else
                //         {
                //             tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary);
                //         }


                //         if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                //         {
                //             tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;

                //             _iTblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList);
                //         }
                //     }

                //     tblPurchaseEnquiryTO.BookingScheduleTOList = tblPurchaseScheduleSummaryTOList;
                // }

                #region Get Shipment Details If Available

                tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList = _iTblpurchaseEnqShipmemtDtlsBL.SelectTblpurchaseEnqShipmemtDtlsTOByEnqId(tblPurchaseEnquiryTO.IdPurchaseEnquiry);

                if (tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList != null && tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList.Count>0)
                {
                    for (int ship = 0; ship < tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList.Count; ship++)
                    {
                        tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList = _iITblpurchaseEnqShipmemtDtlsExtBL.SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList[ship].IdShipmentDtls);
                    }
                }
                #endregion

            }
            return tblPurchaseEnquiryTO;

        }

        public List<TblPurchaseEnquiryTO> GetEnquiryItemDtlsFromBookingIds(string saudaIds)
        {
            List<TblPurchaseEnquiryTO> enquiryTOList = new List<TblPurchaseEnquiryTO>();
            if(!String.IsNullOrEmpty(saudaIds))
            {
                List<TblPurchaseEnquiryTO> enquiryList = _iTblPurchaseEnquiryDAO.SelectSaudaListBySaudaIds(saudaIds);
                if(enquiryList != null && enquiryList.Count > 0)
                {
                   List<TblPurchaseEnquiryDetailsTO> enquiryItemDetilsTOList = _iTblPurchaseEnquiryDetailsDAO.SelectEnquiryDetailsListBySaudaIds(saudaIds);
                    if(enquiryItemDetilsTOList != null && enquiryItemDetilsTOList.Count > 0)
                    {
                        List<Int32> saudaIdList = saudaIds.Split(',').Select(Int32.Parse).ToList();
                        if(saudaIdList != null && saudaIdList.Count >0)
                        {
                            for (int i = 0; i < saudaIdList.Count; i++)
                            {
                                Int32 enquiryId = saudaIdList[i];

                                TblPurchaseEnquiryTO tempEnqTO = enquiryList.Where(a => a.IdPurchaseEnquiry == enquiryId).FirstOrDefault();
                                if(tempEnqTO != null)
                                {
                                    tempEnqTO.PurchaseEnquiryDetailsTOList = enquiryItemDetilsTOList.Where(a => a.PurchaseEnquiryId == enquiryId).ToList();
                                }

                                enquiryTOList.Add(tempEnqTO);
                            }
                        }
                    }
                }

            }
            return enquiryTOList;
        }

        public List<TblPurchaseEnquiryTO> SelectSaudaListBySaudaIds(String saudaIds)
        {
            return _iTblPurchaseEnquiryDAO.SelectSaudaListBySaudaIds(saudaIds);
        }

        public List<TblPurchaseEnquiryTO> SelectAllBookingsListForAcceptance(String cnfId, TblUserRoleTO tblUserRoleTO, Int32 isGetPendSaudaToClose, Int32 IsOrderOREnq)
        {
            return _iTblPurchaseEnquiryDAO.SelectAllBookingsListForAcceptance(cnfId, tblUserRoleTO, isGetPendSaudaToClose, IsOrderOREnq);
        }

        public List<TblProdClassificationTO> GetProductClassListByItemCatg(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            List<TblProdClassificationTO> TblProdClassificationTOCatlist = _iTblProdClassificationBL.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);
            List<TblProdClassificationTO> TblProdClassificationTOSpecificationlist = new List<TblProdClassificationTO>();
            if (TblProdClassificationTOCatlist != null && TblProdClassificationTOCatlist.Count > 0)
            {
                string catStr = (string.Join(",", TblProdClassificationTOCatlist.Select(x => x.IdProdClass.ToString()).ToArray()));

                List<TblProdClassificationTO> TblProdClassificationTOSubCatlist = _iTblProdClassificationBL.SelectAllTblProdClassification(catStr, "SC");
                if (TblProdClassificationTOSubCatlist != null && TblProdClassificationTOSubCatlist.Count > 0)
                {
                    string subCatStr = (string.Join(",", TblProdClassificationTOSubCatlist.Select(x => x.IdProdClass.ToString()).ToArray()));

                    TblProdClassificationTOSpecificationlist = _iTblProdClassificationBL.SelectAllTblProdClassification(subCatStr, "S");
                }

            }
            return TblProdClassificationTOSpecificationlist;

        }
        public List<TblPurchaseEnquiryTO> GetMaterialTypeWiseTotalPendingQty(Int32 SpotedId, String userId, Int32 organizationId, Int32 statusId, Int32 isConvertToSauda, Int32 isPending, Int32 materialTypeId, string cOrNcId, DateTime fromDate, DateTime toDate, Int32 isSkipDateFilter = 0) //, String userRoleTO)
        {
            List<TblPurchaseEnquiryTO> lstTblPurchaseEnquiry = new List<TblPurchaseEnquiryTO>();

            List<TblProdClassificationTO> TblProdClassificationTOSpecificationlist = GetProductClassListByItemCatg(Constants.ItemProdCategoryE.SCRAP_OR_WASTE);
            if(TblProdClassificationTOSpecificationlist == null)
            {
                return null;
            }
            for (int i = 0; i < TblProdClassificationTOSpecificationlist.Count; i++)
            {
                TblProdClassificationTO tblProdClassificationTO = TblProdClassificationTOSpecificationlist[i];
                TblPurchaseEnquiryTO tblPurchaseEnquiry = new TblPurchaseEnquiryTO();
                tblPurchaseEnquiry.ProdClassId = tblProdClassificationTO.IdProdClass;
                tblPurchaseEnquiry.ProdClassDesc = tblProdClassificationTO.ProdClassDesc;
                tblPurchaseEnquiry.PendingQty = 0;
                lstTblPurchaseEnquiry.Add(tblPurchaseEnquiry);
            }

            List<TblPurchaseEnquiryTO> tempLstTblPurchaseEnquiry = GetAllEnquiryList(SpotedId, userId, organizationId, statusId, fromDate, toDate, isConvertToSauda, isPending, materialTypeId, cOrNcId, isSkipDateFilter); //, tblUserRoleTO);
            if (tempLstTblPurchaseEnquiry != null)
            {
                for (int i = 0; i < tempLstTblPurchaseEnquiry.Count; i++)
                {
                    TblPurchaseEnquiryTO tempTblPurchaseEnquiry = tempLstTblPurchaseEnquiry[i];
                    TblPurchaseEnquiryTO tblPurchaseEnquiryTO = SelectTblPurchaseEnquiry(tempTblPurchaseEnquiry.IdPurchaseEnquiry);
                    //for (int j = 0; j < tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count; j++)
                    //{
                    //TblPurchaseEnquiryDetailsTO tempTblPurchaseEnquiryDetails = tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[j];
                    if (tempTblPurchaseEnquiry.PendingBookingQty > 0)
                    {
                        TblPurchaseEnquiryTO matchingTblPurchaseEnquiry = lstTblPurchaseEnquiry.Where(e => e.ProdClassId == tempTblPurchaseEnquiry.ProdClassId).FirstOrDefault();
                        if (matchingTblPurchaseEnquiry == null)
                        {
                            TblPurchaseEnquiryTO tblPurchaseEnquiry = new TblPurchaseEnquiryTO();
                            tblPurchaseEnquiry.ProdClassId = tempTblPurchaseEnquiry.ProdClassId;
                            tblPurchaseEnquiry.ProdClassDesc = tempTblPurchaseEnquiry.ProdClassDesc;
                            tblPurchaseEnquiry.PendingQty = tempTblPurchaseEnquiry.PendingBookingQty;
                            lstTblPurchaseEnquiry.Add(tblPurchaseEnquiry);
                        }
                        else
                        {
                            matchingTblPurchaseEnquiry.PendingQty += tempTblPurchaseEnquiry.PendingBookingQty;
                        }
                        //}   
                    }
                }
            }
            return lstTblPurchaseEnquiry;
        }
        public List<TblPurchaseEnquiryTO> GetAllEnquiryList(Int32 SpotedId, String userId, Int32 organizationId, Int32 statusId, DateTime fromDate, DateTime toDate, Int32 isConvertToSauda, Int32 isPending, Int32 materialTypeId,string cOrNcId, Int32 isSkipDateFilter) //, TblUserRoleTO tblUserRoleTO)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = new List<TblPurchaseEnquiryTO>();
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOListFiltered = new List<TblPurchaseEnquiryTO>();
            tblPurchaseEnquiryTOList = _iTblPurchaseEnquiryDAO.GetAllEnquiryList(userId, organizationId, statusId, fromDate, toDate, isConvertToSauda, isPending, materialTypeId, cOrNcId, isSkipDateFilter); //, tblUserRoleTO);

            if (SpotedId == Convert.ToInt32(Constants.ShowSpotedVehicleE.SHOW_SPOTED_ENTRIES))
            {
                tblPurchaseEnquiryTOList = tblPurchaseEnquiryTOList.Where(a => a.IsSpotedVehicle == SpotedId).ToList();
            }
            else if (SpotedId == Convert.ToInt32(Constants.ShowSpotedVehicleE.SHOW_NON_SPOTED_ENTRIES))
            {
                tblPurchaseEnquiryTOList = tblPurchaseEnquiryTOList.Where(a => a.IsSpotedVehicle == SpotedId).ToList();
            }

            // for (int i = 0; i < tblPurchaseEnquiryTOList.Count; i++)
            // {
            //     List<TblPurchaseVehicleSpotEntryTO> tblPurchaseEnquirySoptEntryTO = BL.TblPurchaseVehicleSpotEntryBL.SelectTblPurchaseEnqVehEntryTOList(tblPurchaseEnquiryTOList[i].IdPurchaseEnquiry);
            //     if (SpotedId == Convert.ToInt32(Constants.ShowSpotedVehicleE.SHOW_ALL_ENTRIES))
            //     {
            //         if (tblPurchaseEnquirySoptEntryTO != null && tblPurchaseEnquirySoptEntryTO.Count != 0)
            //         {
            //             tblPurchaseEnquiryTOList[i].PurchaseVehicleSpotEntryTO = tblPurchaseEnquirySoptEntryTO[0];
            //             tblPurchaseEnquiryTOList[i].VehicleSpotEntryId = tblPurchaseEnquirySoptEntryTO[0].IdVehicleSpotEntry;
            //             tblPurchaseEnquiryTOListFiltered.Add(tblPurchaseEnquiryTOList[i]);
            //         }
            //         else
            //         {
            //             tblPurchaseEnquiryTOList[i].PurchaseVehicleSpotEntryTO = new TblPurchaseVehicleSpotEntryTO();
            //             tblPurchaseEnquiryTOListFiltered.Add(tblPurchaseEnquiryTOList[i]);
            //         }
            //     }
            //     else if (SpotedId == Convert.ToInt32(Constants.ShowSpotedVehicleE.SHOW_NON_SPOTED_ENTRIES))
            //     {
            //         if (tblPurchaseEnquirySoptEntryTO != null && tblPurchaseEnquirySoptEntryTO.Count != 0)
            //         {
            //         }
            //         else
            //         {
            //             tblPurchaseEnquiryTOList[i].PurchaseVehicleSpotEntryTO = new TblPurchaseVehicleSpotEntryTO();
            //             tblPurchaseEnquiryTOListFiltered.Add(tblPurchaseEnquiryTOList[i]);
            //         }
            //     }
            //     else if (SpotedId == Convert.ToInt32(Constants.ShowSpotedVehicleE.SHOW_SPOTED_ENTRIES))
            //     {
            //         if (tblPurchaseEnquirySoptEntryTO != null && tblPurchaseEnquirySoptEntryTO.Count != 0)
            //         {
            //             tblPurchaseEnquiryTOList[i].PurchaseVehicleSpotEntryTO = tblPurchaseEnquirySoptEntryTO[0];
            //             tblPurchaseEnquiryTOList[i].VehicleSpotEntryId = tblPurchaseEnquirySoptEntryTO[0].IdVehicleSpotEntry;
            //             tblPurchaseEnquiryTOListFiltered.Add(tblPurchaseEnquiryTOList[i]);
            //         }
            //         else
            //         {
            //         }
            //     }

            // }


            return tblPurchaseEnquiryTOList;
        }

        public List<TblRecycleDocumentTO> SelectAllDocumentIdFromSpotEntryId(int transId, int transTypeId)
        {
            return _iTblPurchaseEnquiryDAO.SelectAllDocumentIdFromSpotEntryId(transId, transTypeId);
        }

        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblPurchaseEnquiryTO(purchaseEnquiryId);

        }

        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblPurchaseEnquiryTO(purchaseEnquiryId, conn, tran);

        }
        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 userId, Int32 supplierId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblPurchaseEnquiryTO(userId, supplierId, prodClassId, conn, tran);

        }
        public List<TblPurchaseEnquiryTO> SelectTblPurchaseEnquirySpotEntryTO(Int32 spotEntryId) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblPurchaseEnquirySpotEntryTO(spotEntryId);

        }

        public List<TblPurchaseEnquiryTO> GetSupplierWithMaterialHistList(Int32 supplierId, Int32 lastNRecords)
        {
            return _iTblPurchaseEnquiryDAO.GetSupplierWithMaterialHistList(supplierId, lastNRecords);

        }
        // public  List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId)
        // {
        //     return _iTblPurchaseEnquiryDAO.GetSupplierWiseSaudaDetails(supplierId, statusId);

        // }
        public List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOReturnList = new List<TblPurchaseEnquiryTO>();
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = _iTblPurchaseEnquiryDAO.GetSupplierWiseSaudaDetails(supplierId, statusId);
            if (tblPurchaseEnquiryTOList != null && tblPurchaseEnquiryTOList.Count > 0)
            {
                //Prajakta[2019-04-22] Commented
                // for (int i = 0; i < tblPurchaseEnquiryTOList.Count; i++)
                // {
                //     List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = BL.TblPurchaseScheduleSummaryBL.SelectEnquiryScheduleSummary(tblPurchaseEnquiryTOList[i].IdPurchaseEnquiry);
                //     if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                //     {
                //         Double totalScheduleQty = tblPurchaseScheduleSummaryTOList.Sum(a => a.Qty);
                //         tblPurchaseEnquiryTOList[i].PendingBookingQty = tblPurchaseEnquiryTOList[i].BookingQty - totalScheduleQty;
                //     }
                //     else
                //     {
                //         tblPurchaseEnquiryTOList[i].PendingBookingQty = tblPurchaseEnquiryTOList[i].BookingQty;
                //     }
                //     tblPurchaseEnquiryTOList[i].PendingBookingQty = Math.Round(tblPurchaseEnquiryTOList[i].PendingBookingQty, 3);
                // }

                //tblPurchaseEnquiryTOList=tblPurchaseEnquiryTOList.Where(a=>a.PendingBookingQty>0).ToList();
                if (tblPurchaseEnquiryTOList != null && tblPurchaseEnquiryTOList.Count > 0)
                {
                    for (int i = 0; i < tblPurchaseEnquiryTOList.Count; i++)
                    {
                        // TblPurchaseEnquiryTO tblPurchaseEnquiryTO = new TblPurchaseEnquiryTO();
                        // if (tblPurchaseEnquiryTOList[i].BookingQty == 0)
                        // {
                        //     tblPurchaseEnquiryTO = tblPurchaseEnquiryTOList[i];
                        //     tblPurchaseEnquiryTOReturnList.Add(tblPurchaseEnquiryTO);
                        // }
                        // else
                        // {
                        //     if (tblPurchaseEnquiryTOList[i].PendingBookingQty > 0)
                        //     {
                        //         tblPurchaseEnquiryTO = tblPurchaseEnquiryTOList[i];
                        //         tblPurchaseEnquiryTOReturnList.Add(tblPurchaseEnquiryTO);
                        //     }
                        // }

                        TblPurchaseEnquiryTO tblPurchaseEnquiryTO = new TblPurchaseEnquiryTO();
                        //if (tblPurchaseEnquiryTOList[i].BookingQty == 0 || tblPurchaseEnquiryTOList[i].PendingBookingQty > 0)

                        if(Startup.IsForBRM)
                        {
                            if(tblPurchaseEnquiryTOList[i].PendingBookingQty > 0)
                            {
                                tblPurchaseEnquiryTO = tblPurchaseEnquiryTOList[i];
                                tblPurchaseEnquiryTOReturnList.Add(tblPurchaseEnquiryTO);
                            }
                        }
                        else
                        {
                            tblPurchaseEnquiryTO = tblPurchaseEnquiryTOList[i];
                            tblPurchaseEnquiryTOReturnList.Add(tblPurchaseEnquiryTO);
                        }

                    }
                }
            }

            return tblPurchaseEnquiryTOReturnList;

        }

        public TblPurchaseEnquiryTO SelectTblBookingsForPurchaseTO(Int32 idPurchaseEnquiry, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblBookingsForPurchase(idPurchaseEnquiry, conn, tran);

        }

        public List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE)
        {
            return _iTblPurchaseEnquiryDAO.SelectAllTblOrganization(orgTypeE);
        }

        public Int32 SelectMaxEnquiryNo(Int32 finYear, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.SelectMaxEnquiryNo(finYear, conn, tran);
        }


        //Priyanka [03-01-2019]
        //public  List<TblPurchaseEnquiryTO> SelectAllBookingsListForAcceptance(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        //{
        //    return _iTblPurchaseEnquiryDAO.SelectAllBookingsListForAcceptance(cnfId, tblUserRoleTO);
        //}

        public TblPurchaseEnquiryTO SelectTblBookingsTO(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblBookings(idBooking, conn, tran);
        }

        public BookingInfo SelectBookingDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date)
        {
            try
            {
                return _iTblPurchaseEnquiryDAO.SelectBookingDashboardInfo(tblUserRoleTO, orgId, date);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //Prajakta[2020-09-20] Added to get material wise sauda qty and avg price
        public List<BookingInfo> SelectMaterialWiseEnqOrSaudaInfoForDashboard(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date, Int32 isConvertToSauda)
        {
            try
            {
                return _iTblPurchaseEnquiryDAO.SelectMaterialWiseEnqOrSaudaInfoForDashboard(tblUserRoleTO, orgId, date, isConvertToSauda);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BookingInfo SelectBookingSaudaDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date)
        {
            try
            {
                return _iTblPurchaseEnquiryDAO.SelectBookingSaudaDashboardInfo(tblUserRoleTO, orgId, date);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BookingInfo SelectTodayRtaeDashboardInfo(TblUserRoleTO tblUserRoleTO, Int32 orgId, DateTime date)
        {
            try
            {
                return _iTblPurchaseEnquiryDAO.SelectTodayRtaeDashboardInfo(tblUserRoleTO, orgId, date);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Insertion

        public ResultMessage SaveNewPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Dictionary<string, object> enquiryDtlsDict = new Dictionary<string, object>();
            DateTime serverDateTime = _iCommonDAO.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                lock (EnquiryLock)
                {
                    #region 1. Check enquiry are Open Or Closed. If Closed Then Do Not Save the request

                    TblPurchaseBookingActionsTO bookingStatusTO = _iTblPurchaseBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                    if (bookingStatusTO == null || bookingStatusTO.BookingStatus == "CLOSE")
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Sorry..Record Could not be saved. Bookings are closed";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Bookings are closed";
                        return resultMessage;
                    }

                    #endregion


                    #region 2. Save Enquiry Request First

                    TblRateBandDeclarationPurchaseTO existingRateBandTO = _iTblRateBandDeclarationPurchaseBL.SelectTblRateBandDeclaration(tblPurchaseEnquiryTO.RateBandDeclarationPurchaseId, conn, tran);
                    if (existingRateBandTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record Could not be saved. Rate Not Found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    TblGlobalRatePurchaseTO globalRatePurchaseTO = _iTblGlobalRatePurchaseBL.SelectTblGlobalRatePurchaseTO(existingRateBandTO.GlobalRatePurchaseId, conn, tran);
                    if (globalRatePurchaseTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record Could not be saved. Rate Declaration Not Found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.Result = 0;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    Double allowedRate = globalRatePurchaseTO.Rate - existingRateBandTO.RateBandCosting;
                    Double bookingRateWithOrcAmt = tblPurchaseEnquiryTO.BookingRate;

                    tblPurchaseEnquiryTO.PendingBookingQty = tblPurchaseEnquiryTO.BookingQty;
                    tblPurchaseEnquiryTO.OptionalPendingQty = tblPurchaseEnquiryTO.PendingBookingQty;


                    TblOrganizationTO OrgTO = _iTblOrganizationBL.SelectTblOrganizationTO(tblPurchaseEnquiryTO.SupplierId, conn, tran);
                    if (OrgTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record Could not be saved. supplier Details not found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.Result = 0;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    //Check if open sauda is already exists for sleected PM,Supplier,Material Type
                    if (tblPurchaseEnquiryTO.IsOpenQtySauda == 1)
                    {
                        TblPurchaseEnquiryTO tblPurchaseEnquiryTempTO = SelectTblPurchaseEnquiryTO(tblPurchaseEnquiryTO.UserId, tblPurchaseEnquiryTO.SupplierId, tblPurchaseEnquiryTO.ProdClassId, conn, tran);
                        if (tblPurchaseEnquiryTempTO != null)
                        {
                            if (tblPurchaseEnquiryTempTO.IsOpenQtySauda == 1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Already open qty sauda is created for Purchase Manager - " + tblPurchaseEnquiryTempTO.PurchaseManagerName + " and Supplier - " + tblPurchaseEnquiryTempTO.SupplierName;
                                resultMessage.DisplayMessage = "Already open qty sauda is created for Purchase Manager - " + tblPurchaseEnquiryTempTO.PurchaseManagerName + " and Supplier - " + tblPurchaseEnquiryTempTO.SupplierName;
                                resultMessage.Result = 0;
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }

                        }
                    }

                    #region 4. Check If Booking Item details falls within taregt qty. Update Grade Wise Target Qty

                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_SHOW_GRADE_WISE_TRAGET_DETAILS, conn, tran);
                    if (tblConfigParamsTO != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 1 && tblPurchaseEnquiryTO.IsUpdateGradeDtls == 1)
                        {
                            List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyUpdateTOList = new List<TblGradeWiseTargetQtyTO>();
                            List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyInsertTOList = new List<TblGradeWiseTargetQtyTO>();
                            Boolean isUpdateGradeDtls = CheckIfGradeWithinTargetQty(tblPurchaseEnquiryTO, tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList, tblGradeWiseTargetQtyUpdateTOList, tblGradeWiseTargetQtyInsertTOList, conn, tran);
                            if (!isUpdateGradeDtls)
                            {
                                //Go for approval
                                tblPurchaseEnquiryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL);
                                tblPurchaseEnquiryTO.AuthReasons += " |" + Constants.AuthReasonIdsE.TARGET_QTY.ToString();
                                tblPurchaseEnquiryTO.Comments += " |" + Constants.AuthReasonIdsE.TARGET_QTY.ToString();
                                tblPurchaseEnquiryTO.IsConvertToSauda = 0;
                            }

                            if (tblGradeWiseTargetQtyUpdateTOList != null && tblGradeWiseTargetQtyUpdateTOList.Count > 0)
                            {
                                for (int p = 0; p < tblGradeWiseTargetQtyUpdateTOList.Count; p++)
                                {
                                    result = _iTblGradeWiseTargetQtyBL.UpdateTblGradeWiseTargetQty(tblGradeWiseTargetQtyUpdateTOList[p], conn, tran);
                                    if (result <= 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.Text = "Error While Updating Grade Wise Balance Qty Details in Function SaveNewBooking";
                                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }
                                }
                            }

                            if (tblGradeWiseTargetQtyInsertTOList != null && tblGradeWiseTargetQtyInsertTOList.Count > 0)
                            {
                                for (int g = 0; g < tblGradeWiseTargetQtyInsertTOList.Count; g++)
                                {
                                    result = _iTblGradeWiseTargetQtyBL.InsertTblGradeWiseTargetQty(tblGradeWiseTargetQtyInsertTOList[g], conn, tran);
                                    if (result <= 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.Text = "Error While Inserting Grade Wise Balance Qty Details in Function SaveNewBooking";
                                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }
                                }
                            }

                        }
                    }

                    #endregion


                    #region  Check If padta calculations are in configuration limits
                    int Result = 0;
                    TblConfigParamsTO tblConfigParamsTOPadta = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_PADTA_CONFIG_LIMIT_FOR_ENQUIRY_AND_SAUDA_APPROVAL, conn, tran);
                    if (tblConfigParamsTOPadta != null && tblPurchaseEnquiryTO.IsConvertToSauda == 1)
                    {
                        if (tblConfigParamsTOPadta.ConfigParamVal.ToString() == "0")
                        {
                            Result = 1;
                        }
                        else
                        {
                            int minLimit;
                            int maxLimit;
                            string[] arr = tblConfigParamsTOPadta.ConfigParamVal.Split(',');
                            minLimit = Convert.ToInt32(arr[0]);
                            maxLimit = Convert.ToInt32(arr[1]);

                            double padta = 0;

                            padta = tblPurchaseEnquiryTO.Padta;

                            if (tblPurchaseEnquiryTO.COrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
                            {
                                padta = tblPurchaseEnquiryTO.Padta;
                            }
                            if (minLimit <= padta && padta <= maxLimit)
                            {
                                Result = 1;
                            }
                            else
                            {
                                tblPurchaseEnquiryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL);
                                tblPurchaseEnquiryTO.AuthReasons += " |" + Constants.AuthReasonIdsE.PADTA_OUT_OF_BAND.ToString();
                                tblPurchaseEnquiryTO.IsConvertToSauda = 0;
                            }
                        }
                    }

                    #endregion

                    //Prajakta[2018-11-20] Added
                    //Get Max Enquiry No
                    DimFinYearTO DimFinYearTO = _idimensionBL.GetCurrentFinancialYear(serverDateTime, conn, tran);
                    if (DimFinYearTO != null)
                    {
                        tblPurchaseEnquiryTO.EnqNo = SelectMaxEnquiryNo(DimFinYearTO.IdFinYear, conn, tran);
                        tblPurchaseEnquiryTO.FinYear = DimFinYearTO.IdFinYear;
                        tblPurchaseEnquiryTO.EnqDisplayNo = DimFinYearTO.IdFinYear + "/" + tblPurchaseEnquiryTO.EnqNo;
                    }

                    if (tblPurchaseEnquiryTO.CurrencyId == null || tblPurchaseEnquiryTO.CurrencyId <= 0)
                    {
                        tblPurchaseEnquiryTO.CurrencyId = (Int32)Constants.CurrencyE.INR;
                    }
                    if (Startup.IsForBRM)
                    {
                        tblPurchaseEnquiryTO.SaudaTypeId = (Int32)Constants.SaudaTypeE.TONNAGE_QTY;
                        tblPurchaseEnquiryTO.CurrencyId = (Int32)Constants.CurrencyE.INR;
                    }

                    tblPurchaseEnquiryTO.RateForC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, true, conn, tran);
                    tblPurchaseEnquiryTO.RateForNC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, false, conn, tran);

                    tblPurchaseEnquiryTO.RefRateofV48Var = _iTblConfigParamsBL.GetCurrentValueOfV8RefVar(Constants.CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE);
                    tblPurchaseEnquiryTO.RefRateC = _iTblConfigParamsBL.GetCurrentValueOfV8RefVar(Constants.CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE_C);
                    result = InsertTblPurchaseEnquiry(tblPurchaseEnquiryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record Could not be saved.";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.Result = 0;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    #endregion

                    #region 3. Save Purchase Enquiry details
                    if (tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count > 0)
                    {
                        for (int qd = 0; qd < tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count; qd++)
                        {
                            TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO = new TblPurchaseEnquiryDetailsTO();
                            TblPurchaseEnquiryDetailsTO enquiryTO = tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[qd];


                            tblPurchaseEnquiryDetailsTO.PurchaseEnquiryId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                            tblPurchaseEnquiryDetailsTO.ActualRate = enquiryTO.ActualRate;
                            tblPurchaseEnquiryDetailsTO.ProdItemId = enquiryTO.ProdItemId;
                            tblPurchaseEnquiryDetailsTO.Qty = enquiryTO.Qty;
                            tblPurchaseEnquiryDetailsTO.Rate = enquiryTO.Rate;
                            tblPurchaseEnquiryDetailsTO.ProductAomunt = enquiryTO.ProductAomunt;
                            tblPurchaseEnquiryDetailsTO.ProductRecovery = enquiryTO.ProductRecovery;
                            tblPurchaseEnquiryDetailsTO.SchedulePurchaseId = enquiryTO.SchedulePurchaseId;
                            tblPurchaseEnquiryDetailsTO.PendingQty = enquiryTO.PendingQty;
                            tblPurchaseEnquiryDetailsTO.LoadingLayerId = enquiryTO.LoadingLayerId;
                            tblPurchaseEnquiryDetailsTO.DemandedRate = enquiryTO.DemandedRate;
                            tblPurchaseEnquiryDetailsTO.TotalCost = enquiryTO.TotalCost;
                            tblPurchaseEnquiryDetailsTO.TotalProduct = enquiryTO.TotalProduct;
                            tblPurchaseEnquiryDetailsTO.MetalCost = enquiryTO.MetalCost;
                            tblPurchaseEnquiryDetailsTO.GradePadta = enquiryTO.GradePadta;
                            tblPurchaseEnquiryDetailsTO.Recovery = enquiryTO.Recovery;
                            tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList = enquiryTO.GradeExpressionDtlsTOList;

                            //Priyanka [11-01-2019]
                            tblPurchaseEnquiryDetailsTO.ItemBookingRate = enquiryTO.ItemBookingRate;

                            result = _iTblPurchaseEnquiryDetailsBL.InsertTblPurchaseEnquiryDetails(tblPurchaseEnquiryDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While InsertTblPurchaseEnquiryDetails");
                                return resultMessage;
                            }
                            else
                            {
                                //Save grade expreesion details
                                if (tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList != null && tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList.Count > 0)
                                {
                                    tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList = tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList.Where(w => w.ExpressionDtlsId > 0).ToList();

                                    for (int d = 0; d < tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList.Count; d++)
                                    {
                                        tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList[d].PurchaseEnquiryDtlsId = tblPurchaseEnquiryDetailsTO.IdPurchaseEnquiryDetails;
                                        result = _iTblGradeExpressionDtlsBL.InsertTblGradeExpressionDtls(tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList[d], conn, tran);
                                        if (result != 1)
                                        {
                                            tran.Rollback();
                                            resultMessage.DefaultBehaviour("Error While InsertTblGradeExpressionDtls");
                                            return resultMessage;
                                        }

                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region Vehicle Type Insertion

                    resultMessage = SavePurchaseVehicleTypeDesc(tblPurchaseEnquiryTO, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error in SavePurchaseVehicleTypeDesc(tblPurchaseEnquiryTO,conn,tran);";
                        return resultMessage;
                    }

                    #endregion


                    #region 5. Save Booking Parities 

                    DateTime sysDate = _iCommonDAO.ServerDateTime;

                    #endregion

                    #region 6.Save enquiry schedule 
                    if (tblPurchaseEnquiryTO.BookingScheduleTOList != null && tblPurchaseEnquiryTO.BookingScheduleTOList.Count > 0)
                    {
                        for (int k = 0; k < tblPurchaseEnquiryTO.BookingScheduleTOList.Count; k++)
                        {
                            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = tblPurchaseEnquiryTO.BookingScheduleTOList[k];

                            tblPurchaseScheduleSummaryTO.CreatedOn = _iCommonDAO.ServerDateTime;
                            tblPurchaseScheduleSummaryTO.UpdatedOn = _iCommonDAO.ServerDateTime;
                            tblPurchaseScheduleSummaryTO.PurchaseEnquiryId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                            result = _iTblPurchaseScheduleSummaryBL.InsertTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO, conn, tran);
                            if (result == 1)
                            {
                                if (tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                                {
                                    for (int i = 0; i < tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count; i++)
                                    {
                                        TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO = tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList[i];
                                        tblPurchaseVehicleDetailsTO.SchedulePurchaseId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                                        result = _iTblPurchaseVehicleDetailsBL.InsertTblPurchaseVehicleDetails(tblPurchaseVehicleDetailsTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            tran.Rollback();
                                            resultMessage.Text = "Error While Inserting The Vehicle Schedule Details";
                                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.Result = 0;
                                            return resultMessage;
                                        }
                                        else
                                        {
                                            if (tblPurchaseVehicleDetailsTO.GradeExpressionDtlsTOList != null && tblPurchaseVehicleDetailsTO.GradeExpressionDtlsTOList.Count > 0)
                                            {
                                                tblPurchaseVehicleDetailsTO.GradeExpressionDtlsTOList = tblPurchaseVehicleDetailsTO.GradeExpressionDtlsTOList.Where(w => w.ExpressionDtlsId > 0).ToList();

                                                for (int d = 0; d < tblPurchaseVehicleDetailsTO.GradeExpressionDtlsTOList.Count; d++)
                                                {
                                                    tblPurchaseVehicleDetailsTO.GradeExpressionDtlsTOList[d].PurchaseScheduleDtlsId = tblPurchaseVehicleDetailsTO.IdVehiclePurchase;
                                                    result = _iTblGradeExpressionDtlsBL.InsertTblGradeExpressionDtls(tblPurchaseVehicleDetailsTO.GradeExpressionDtlsTOList[d], conn, tran);
                                                    if (result != 1)
                                                    {
                                                        resultMessage.Text = "Error While Inserting The Grade Expression Details";
                                                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                                        resultMessage.MessageType = ResultMessageE.Error;
                                                        resultMessage.Result = 0;
                                                        return resultMessage;
                                                    }

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error While Inserting The Vehicle Schedule Details";
                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }

                    }
                    else
                    {

                    }
                    #endregion

                    #region 9. Update Spot Entry Vehicle Ddetails

                    if (tblPurchaseVehicleSpotEntryTO != null && tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry > 0)
                    {
                        if (tblPurchaseEnquiryTO.BookingScheduleTOList != null && tblPurchaseEnquiryTO.BookingScheduleTOList.Count > 0)
                        {
                            var res = tblPurchaseEnquiryTO.BookingScheduleTOList.Where(a => a.SpotEntryVehicleId == tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry).ToList();
                            if (res != null && res.Count > 0)
                            {
                                tblPurchaseVehicleSpotEntryTO.PurchaseScheduleSummaryId = res[0].IdPurchaseScheduleSummary;
                            }
                        }

                        //Prajakta[2019-03-29] Added to check if sauda is already there for spot vehicle
                        TblPurchaseVehicleSpotEntryTO spotVehicleTO = _iTblPurchaseVehicleSpotEntryBL.SelectTblPurchaseVehicleSpotEntryTO(tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry, conn, tran);
                        if (spotVehicleTO != null)
                        {
                            if (spotVehicleTO.PurchaseEnquiryId > 0)
                            {
                                tran.Rollback();
                                resultMessage.Result = 0;
                                resultMessage.Text = "Sauda # " + spotVehicleTO.BookingTO.EnqDisplayNo + " is already created for the Vehicle No. - " + spotVehicleTO.VehicleNo;
                                resultMessage.DisplayMessage = "Sauda # " + spotVehicleTO.BookingTO.EnqDisplayNo + "is already created for the Vehicle No. - " + spotVehicleTO.VehicleNo;
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }


                        tblPurchaseVehicleSpotEntryTO.PurchaseEnquiryId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                        result = _iTblPurchaseVehicleSpotEntryBL.UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While Updating Spot Entry Vehicle Details in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }

                    }
                    #endregion

                    #region Update Pending Booking Qty
                    result = UpdatePendingBookingQty(tblPurchaseEnquiryTO, false, null, conn, tran);
                    if (result <= 0)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Pending Booking Qty";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                    #endregion

                    #region 7. Notifications & SMS

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    if (tblPurchaseEnquiryTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL))
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_DIRECTOR;
                        tblAlertInstanceTO.AlertAction = "ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_DIRECTOR";
                        // tblAlertInstanceTO.AlertComment = "Vehicle Is Reported";
                        tblAlertInstanceTO.AlertComment = "Scrap enquiry #" + tblPurchaseEnquiryTO.EnqDisplayNo + " is awaiting for director approval.";
                        //tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        tblAlertInstanceTO.EffectiveFromDate = tblPurchaseEnquiryTO.CreatedOn;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceDisplayId = "ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_DIRECTOR";
                        tblAlertInstanceTO.SourceEntityId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                        tblAlertInstanceTO.RaisedBy = tblPurchaseEnquiryTO.CreatedBy;
                        tblAlertInstanceTO.RaisedOn = tblPurchaseEnquiryTO.CreatedOn;
                        tblAlertInstanceTO.IsAutoReset = 1;

                        //Sanjay [21 sept 2018] Below code is commented and common notification API is called

                        notify.SendNotificationToUsers(tblAlertInstanceTO);

                    }

                    #endregion

                    #region 8. Update booking Status As OPEN

                    TblPurchaseBookingActionsTO existinBookingActionsTO = _iTblPurchaseBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                    if (existinBookingActionsTO == null || existinBookingActionsTO.BookingStatus == "CLOSE")
                    {
                        TblPurchaseBookingActionsTO bookingActionTO = new TblPurchaseBookingActionsTO();
                        bookingActionTO.BookingStatus = "OPEN";
                        bookingActionTO.IsAuto = 1;
                        bookingActionTO.StatusBy = tblPurchaseEnquiryTO.CreatedBy;
                        bookingActionTO.StatusDate = tblPurchaseEnquiryTO.CreatedOn;

                        result = _iTblPurchaseBookingActionsBL.InsertTblBookingActions(bookingActionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("InsertTblBookingActions");
                            return resultMessage;
                        }
                    }
                    #endregion

                    #region 9. Add Shipment Details Against Enquiry.
                    if (tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList != null && tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList.Count > 0)
                    {
                        for (int ship = 0; ship < tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList.Count; ship++)
                        {
                            TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO = tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsTOList[ship];
                            tblpurchaseEnqShipmemtDtlsTO.CreatedBy = tblPurchaseEnquiryTO.CreatedBy;
                            tblpurchaseEnqShipmemtDtlsTO.IsActive = 1;
                            tblpurchaseEnqShipmemtDtlsTO.CreatedOn = tblPurchaseEnquiryTO.CreatedOn;
                            tblpurchaseEnqShipmemtDtlsTO.PurchaseEnquiryId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                            result = _iTblpurchaseEnqShipmemtDtlsBL.InsertTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("InsertTblpurchaseEnqShipmemtDtls failed");
                                return resultMessage;
                            }
                            if (tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList != null && tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList.Count > 0)
                            {
                                for (int i = 0; i < tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList.Count; i++)
                                {
                                    TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO = new TblpurchaseEnqShipmemtDtlsExtTO();
                                    tblpurchaseEnqShipmemtDtlsExtTO = tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList[i];
                                    tblpurchaseEnqShipmemtDtlsExtTO.CreatedBy = tblPurchaseEnquiryTO.CreatedBy;
                                    tblpurchaseEnqShipmemtDtlsExtTO.CreatedOn = tblPurchaseEnquiryTO.CreatedOn;
                                    tblpurchaseEnqShipmemtDtlsExtTO.IsActive = 1;
                                    tblpurchaseEnqShipmemtDtlsExtTO.ShipmentDtlsId = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;
                                    result = _iITblpurchaseEnqShipmemtDtlsExtBL.InsertTblpurchaseEnqShipmemtDtlsExt(tblpurchaseEnqShipmemtDtlsExtTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour("InsertTblpurchaseEnqShipmemtDtls failed");
                                        return resultMessage;
                                    }
                                }
                            }
                        }
                    }
                    #endregion



                    tran.Commit();
                }
                //Add new Schedule For Spot vehicle
                if (tblPurchaseEnquiryTO.BookingScheduleTOList != null && tblPurchaseEnquiryTO.BookingScheduleTOList.Count > 0 && tblPurchaseVehicleSpotEntryTO != null)
                {
                    List<TblPurchaseScheduleSummaryTO> res = tblPurchaseEnquiryTO.BookingScheduleTOList.Where(a => a.SpotEntryVehicleId == tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry).ToList();
                    if (res != null && res.Count == 1)
                    {
                        ResultMessage resultmsg = _iTblPurchaseScheduleSummaryBL.AddReportedScheduleForSpotVehicle(res[0], serverDateTime);
                        if (resultmsg.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Text = "Success, Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully. Error while reporting the spot vehicle. Vehicle No:" + res[0].VehicleNo;
                            resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully. Error while reporting the spot vehicle. Vehicle No:" + res[0].VehicleNo;
                            enquiryDtlsDict.Add("purchaseEnquiryTO", tblPurchaseEnquiryTO);
                            enquiryDtlsDict.Add("spotEntryTO", tblPurchaseVehicleSpotEntryTO);
                            resultMessage.Tag = enquiryDtlsDict;
                            resultMessage.Result = 1;
                            return resultMessage;

                        }
                        else
                        {
                            // resultMessage.MessageType = ResultMessageE.Information;
                            // resultMessage.Text = "Success, Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                            // resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                            // enquiryDtlsDict.Add("purchaseEnquiryTO", tblPurchaseEnquiryTO);
                            // enquiryDtlsDict.Add("spotEntryTO", tblPurchaseVehicleSpotEntryTO);
                            // resultMessage.Tag = enquiryDtlsDict;
                            // resultMessage.Result = 1;
                            // return resultMessage;
                            return DisplayEnquiryResultMsg(tblPurchaseEnquiryTO, enquiryDtlsDict, tblPurchaseVehicleSpotEntryTO);
                        }
                    }
                    else
                    {
                        // resultMessage.MessageType = ResultMessageE.Information;
                        // resultMessage.Text = "Success, Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                        // resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                        // enquiryDtlsDict.Add("purchaseEnquiryTO", tblPurchaseEnquiryTO);
                        // enquiryDtlsDict.Add("spotEntryTO", tblPurchaseVehicleSpotEntryTO);
                        // resultMessage.Tag = enquiryDtlsDict;
                        // resultMessage.Result = 1;
                        // return resultMessage;
                        return DisplayEnquiryResultMsg(tblPurchaseEnquiryTO, enquiryDtlsDict, tblPurchaseVehicleSpotEntryTO);
                    }
                }
                else
                {
                    // resultMessage.MessageType = ResultMessageE.Information;
                    // resultMessage.Text = "Success, Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                    // resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                    // enquiryDtlsDict.Add("purchaseEnquiryTO", tblPurchaseEnquiryTO);
                    // enquiryDtlsDict.Add("spotEntryTO", tblPurchaseVehicleSpotEntryTO);
                    // resultMessage.Tag = enquiryDtlsDict;
                    // resultMessage.Result = 1;
                    // return resultMessage;
                    return DisplayEnquiryResultMsg(tblPurchaseEnquiryTO, enquiryDtlsDict, tblPurchaseVehicleSpotEntryTO);
                }


            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewPurchaseEnquiry";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage SavePurchaseVehicleTypeDesc(TblPurchaseEnquiryTO tblPurchaseEnquiryTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;

            try
            {
                List<TblPurchaseEnqVehDescTO> existingVehDescTOList = _iTblPurchaseEnqVehDescBL.SelectAllTblPurchaseEnqVehDesc(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);
                if(existingVehDescTOList != null && existingVehDescTOList.Count > 0)
                {
                    for (int k = 0; k < existingVehDescTOList.Count; k++)
                    {
                        result = _iTblPurchaseEnqVehDescBL.DeletePurchVehDesc(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);
                        if(result == -1)
                        {
                            resultMessage.DefaultBehaviour();
                            return resultMessage;
                        }
                    }
                }

                if (tblPurchaseEnquiryTO.TblPurchaseEnqVehDescTOList != null && tblPurchaseEnquiryTO.TblPurchaseEnqVehDescTOList.Count > 0)
                {
                    for (int i = 0; i < tblPurchaseEnquiryTO.TblPurchaseEnqVehDescTOList.Count; i++)
                    {
                        tblPurchaseEnquiryTO.TblPurchaseEnqVehDescTOList[i].PurchaseEnqId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                        result = _iTblPurchaseEnqVehDescBL.InsertTblPurchaseEnqVehDesc(tblPurchaseEnquiryTO.TblPurchaseEnqVehDescTOList[i], conn, tran);
                        if(result != 1)
                        {
                            resultMessage.DefaultBehaviour();
                            return resultMessage;
                        }

                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "Error in SavePurchaseVehicleTypeDesc(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)");
                return resultMessage;
            }
        }
        public ResultMessage DisplayEnquiryResultMsg(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, Dictionary<string, object> enquiryDtlsDict, TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.Information;
            // if (tblPurchaseEnquiryTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL))
            // {
            //     resultMessage.Text = "Success, Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully. Send For Directors Approval.";
            //     resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully. Send For Directors Approval.";
            // }
            // else
            // {
            resultMessage.Text = "Success, Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
            resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
            //}

            enquiryDtlsDict.Add("purchaseEnquiryTO", tblPurchaseEnquiryTO);
            enquiryDtlsDict.Add("spotEntryTO", tblPurchaseVehicleSpotEntryTO);
            resultMessage.Tag = enquiryDtlsDict;
            resultMessage.Result = 1;
            return resultMessage;
        }

        public Boolean CheckIfGradeWithinTargetQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTOList, List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyUpdateTOList, List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyInsertTOList, SqlConnection conn, SqlTransaction tran)
        {
            Boolean result = false;
            if (tblPurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryDetailsTOList.Count > 0 && tblPurchaseEnquiryTO != null)
            {
                List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTOList = _iTblGradeWiseTargetQtyBL.SelectGradeWiseTargetQtyDtls(tblPurchaseEnquiryTO.RateBandDeclarationPurchaseId, tblPurchaseEnquiryTO.UserId, conn, tran);
                if (tblGradeWiseTargetQtyTOList != null && tblGradeWiseTargetQtyTOList.Count > 0)
                {

                    for (int k = 0; k < tblPurchaseEnquiryDetailsTOList.Count; k++)
                    {

                        Double pendingTargetQty = 0;

                        List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTempTOList = tblGradeWiseTargetQtyTOList.Where(s => s.ProdItemId == tblPurchaseEnquiryDetailsTOList[k].ProdItemId).ToList();
                        if (tblGradeWiseTargetQtyTempTOList != null && tblGradeWiseTargetQtyTempTOList.Count == 1)
                        {
                            if (tblGradeWiseTargetQtyTempTOList[0].BookingTargetQty == 0)
                            {
                                result = true;
                                continue;
                            }
                            else
                            {
                                pendingTargetQty = tblGradeWiseTargetQtyTempTOList[0].PendingBookingQty;
                                if (pendingTargetQty >= tblPurchaseEnquiryDetailsTOList[k].Qty)
                                {
                                    result = true;
                                    continue;
                                }
                                else
                                {
                                    result = false;
                                    break;
                                }
                            }

                        }
                        else
                        {
                            result = true;
                            continue;
                        }
                    }

                    //if (result)
                    {
                        for (int m = 0; m < tblPurchaseEnquiryDetailsTOList.Count; m++)
                        {
                            List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTempTOList = tblGradeWiseTargetQtyTOList.Where(s => s.ProdItemId == tblPurchaseEnquiryDetailsTOList[m].ProdItemId).ToList();
                            if (tblGradeWiseTargetQtyTempTOList != null && tblGradeWiseTargetQtyTempTOList.Count == 1)
                            {
                                tblGradeWiseTargetQtyTempTOList[0].PendingBookingQty = tblGradeWiseTargetQtyTempTOList[0].PendingBookingQty - tblPurchaseEnquiryDetailsTOList[m].Qty;
                                tblGradeWiseTargetQtyUpdateTOList.Add(tblGradeWiseTargetQtyTempTOList[0]);
                            }
                            else
                            {
                                TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTOLocal = new TblGradeWiseTargetQtyTO();
                                tblGradeWiseTargetQtyTOLocal.PendingBookingQty = 0;
                                tblGradeWiseTargetQtyTOLocal.PendingBookingQty = tblGradeWiseTargetQtyTOLocal.PendingBookingQty - tblPurchaseEnquiryDetailsTOList[m].Qty;
                                tblGradeWiseTargetQtyTOLocal.PendingUnloadingQty = 0;
                                tblGradeWiseTargetQtyTOLocal.BookingTargetQty = 0;
                                tblGradeWiseTargetQtyTOLocal.UnloadingTargetQty = 0;
                                tblGradeWiseTargetQtyTOLocal.ProdItemId = tblPurchaseEnquiryDetailsTOList[m].ProdItemId;
                                tblGradeWiseTargetQtyTOLocal.ProdClassId = tblPurchaseEnquiryTO.ProdClassId;
                                tblGradeWiseTargetQtyTOLocal.PurchaseManagerId = tblPurchaseEnquiryTO.UserId;
                                tblGradeWiseTargetQtyTOLocal.RateBandPurchaseId = tblPurchaseEnquiryTO.RateBandDeclarationPurchaseId;

                                tblGradeWiseTargetQtyInsertTOList.Add(tblGradeWiseTargetQtyTOLocal);

                            }

                        }

                    }

                }
                else
                {
                    result = true;
                }

            }
            else
            {
                result = true;
            }

            return result;
        }

        public List<TblPurchaseEnquiryTO> SelectAllPurchaseEnquiryForPM(Int32 userId, Int32 rateBandPurchaseId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = _iTblPurchaseEnquiryDAO.SelectAllPurchaseEnquiryForPM(userId, rateBandPurchaseId, prodClassId, conn, tran);
            if (tblPurchaseEnquiryTOList != null && tblPurchaseEnquiryTOList.Count > 0)
            {
                for (int i = 0; i < tblPurchaseEnquiryTOList.Count; i++)
                {
                    List<TblPurchaseEnquiryDetailsTO> tblEnquiryDetailsTOList = new List<TblPurchaseEnquiryDetailsTO>();
                    tblPurchaseEnquiryTOList[i].PurchaseEnquiryDetailsTOList = _iTblPurchaseEnquiryDetailsDAO.SelectAllTblEnquiryDetailsList(tblPurchaseEnquiryTOList[i].IdPurchaseEnquiry, tblPurchaseEnquiryTOList[i].StateId);
                }
            }

            return tblPurchaseEnquiryTOList;

        }


        public int InsertTblPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.InsertTblPurchaseEnquiry(tblPurchaseEnquiryTO, conn, tran);
        }


        #endregion

        #region Updation
        public ResultMessage SaveVehicleDetailsBookingForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1.  Delete previous exisiting records against enquiry
                //List<TblPurchaseEnquiryScheduleTO> tblPurchaseEnquiryScheduleTOList = BL.TblPurchaseEnquiryScheduleBL.SelectAllTblPurchaseEnquiryScheduleList(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);
                List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummary(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);

                for (int l = 0; l < tblPurchaseScheduleSummaryTOList.Count; l++)
                {

                    if (tblPurchaseScheduleSummaryTOList[l].StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_NEW))
                    {
                        //Delete Vehicle
                        result = _iTblPurchaseVehicleDetailsBL.DeleteTblPurchaseVehicleDetails(tblPurchaseScheduleSummaryTOList[l].IdPurchaseScheduleSummary, conn, tran); //// tblPurchaseVehicleDetailsList[i].IdVehiclePurchase,

                        if (result >= 0)
                        {
                            //Delete Schedule
                            //result = TblPurchaseEnquiryScheduleBL.DeleteTblPurchaseEnquirySchedule(tblPurchaseEnquiryScheduleTOList[l].IdSchedulePurchase, conn, tran);
                            result = _iTblPurchaseScheduleSummaryBL.DeleteTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTOList[l].IdPurchaseScheduleSummary, conn, tran);
                            if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error in DeleteTblPurchaseEnquirySchedule method";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                return resultMessage;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error in DeleteTblPurchaseVehicleDetails method";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = -1;
                            return resultMessage;
                        }
                    }
                }
                #endregion

                #region 2.Save project schedule with vehicle details
                if (tblPurchaseEnquiryTO.PurchaseEnquiryScheduleTOLst != null && tblPurchaseEnquiryTO.PurchaseEnquiryScheduleTOLst.Count > 0)
                {
                    //int ScheduleId = 0;
                    int TempScheduleId = 0;
                    double qtytotal = 0;
                    double newPadta = 0;
                    double newBaseMetalCost = 0;
                    double newCalculatedMetalCost = 0;
                    string vno = null;
                    tblPurchaseEnquiryTO.PurchaseEnquiryScheduleTOLst = tblPurchaseEnquiryTO.PurchaseEnquiryScheduleTOLst.OrderBy(c => c.TempScheduleId).ToList();
                    for (int i = 0; i < tblPurchaseEnquiryTO.PurchaseEnquiryScheduleTOLst.Count; i++)
                    {
                        TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryScheduleTO = tblPurchaseEnquiryTO.PurchaseEnquiryScheduleTOLst[i];

                        TblPurchaseScheduleSummaryTO tempPurchaseScheduleSummaryTO = tblPurchaseScheduleSummaryTOList.FirstOrDefault(x => x.IdPurchaseScheduleSummary == tblPurchaseEnquiryScheduleTO.IdSchedulePurchase);
                        if (tempPurchaseScheduleSummaryTO != null && tempPurchaseScheduleSummaryTO.StatusId != Convert.ToInt32(Constants.TranStatusE.BOOKING_NEW))
                        {
                            continue;
                        }

                        qtytotal = 0;
                        newPadta = 0;
                        newBaseMetalCost = 0;
                        newCalculatedMetalCost = 0;
                        //condition for only schedule the qty and not assigned vehicle details
                        //if (tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList == null)
                        if (tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList != null)
                        {
                            for (int j = 0; j < tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList.Count; j++)
                            {
                                qtytotal = qtytotal + tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList[j].ScheduleQty;
                                vno = tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList[j].VehicleNo;
                                newPadta = tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList[j].Padta;
                                newBaseMetalCost = tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList[j].BaseMetalCost;
                                newCalculatedMetalCost = tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList[j].CalculatedMetalCost;
                            }
                        }

                        tblPurchaseEnquiryScheduleTO.PurchaseEnquiryId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                        tblPurchaseEnquiryScheduleTO.Qty = qtytotal;//tblPurchaseEnquiryScheduleTO.ScheduleQty;
                        tblPurchaseEnquiryScheduleTO.SupplierId = tblPurchaseEnquiryTO.SupplierId;
                        tblPurchaseEnquiryScheduleTO.BaseMetalCost = newBaseMetalCost;
                        tblPurchaseEnquiryScheduleTO.CalculatedMetalCost = newCalculatedMetalCost;
                        tblPurchaseEnquiryScheduleTO.Padta = newPadta;
                        tblPurchaseEnquiryScheduleTO.VehicleNo = vno;//tblPurchaseEnquiryTO.VehicleNo;

                        if (tblPurchaseEnquiryScheduleTO.CreatedBy == 0)
                        {
                            tblPurchaseEnquiryScheduleTO.CreatedBy = tblPurchaseEnquiryTO.CreatedBy;
                            tblPurchaseEnquiryScheduleTO.CreatedOn = tblPurchaseEnquiryTO.CreatedOn;
                        }
                        if (tblPurchaseEnquiryScheduleTO.Qty != 0)
                        {
                            result = _iTblPurchaseEnquiryScheduleBL.InsertTblPurchaseEnquirySchedule(tblPurchaseEnquiryScheduleTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblPurchaseEnquirySchedule in Function SaveVehicleDetailsBooking";
                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                resultMessage.Result = 0;
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                            for (int l = 0; l < tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList.Count; l++)
                            {
                                TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO = tblPurchaseEnquiryScheduleTO.BookingVehicleDetailsTOList[l];
                                tblPurchaseVehicleDetailsTO.SchedulePurchaseId = tblPurchaseEnquiryScheduleTO.IdSchedulePurchase;
                                tblPurchaseVehicleDetailsTO.Qty = tblPurchaseVehicleDetailsTO.ScheduleQty;
                                tblPurchaseVehicleDetailsTO.Rate = tblPurchaseVehicleDetailsTO.Rate;
                                if (tblPurchaseVehicleDetailsTO.CreatedBy != 0)
                                {
                                    tblPurchaseVehicleDetailsTO.UpdatedBy = tblPurchaseEnquiryTO.UpdatedBy;
                                    tblPurchaseVehicleDetailsTO.UpdatedOn = tblPurchaseEnquiryTO.UpdatedOn;
                                }
                                else
                                {
                                    tblPurchaseVehicleDetailsTO.CreatedBy = tblPurchaseEnquiryTO.CreatedBy;
                                    tblPurchaseVehicleDetailsTO.CreatedOn = tblPurchaseEnquiryTO.CreatedOn;
                                    tblPurchaseVehicleDetailsTO.UpdatedBy = tblPurchaseEnquiryTO.UpdatedBy;
                                    tblPurchaseVehicleDetailsTO.UpdatedOn = tblPurchaseEnquiryTO.UpdatedOn;
                                }
                                if (tblPurchaseVehicleDetailsTO.SchedulePurchaseId != 0 && (tblPurchaseVehicleDetailsTO.Qty != 0))
                                {
                                    result = _iTblPurchaseVehicleDetailsBL.InsertTblPurchaseVehicleDetails(tblPurchaseVehicleDetailsTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblPurchaseVehicleDetails in Function SaveVehicleDetailsBooking";
                                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                        resultMessage.Result = 0;
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }
                                }
                            }
                        }
                    }

                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Purchase Enquiry Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblPurchaseEnquiryTO;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method UpdateBookingConfirmations";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public int SavePurchaseScheduleDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {
            // ResultMessage resultMessage = new ResultMessage();
            // resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                if (tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                {
                    tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList = tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.OrderBy(c => c.TempScheduleId).ToList();

                    // delete existing records before insert
                    var resultdelete = _iTblPurchaseVehicleDetailsDAO.DeleteTblPurchaseVehicleDetails2(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, conn, tran);

                    for (int i = 0; i < tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count; i++)
                    {
                        TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO = tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList[i];

                        if (tblPurchaseVehicleDetailsTO.SchedulePurchaseId != 0 && (tblPurchaseVehicleDetailsTO.Qty != 0))
                        {
                            result = _iTblPurchaseVehicleDetailsDAO.InsertTblPurchaseVehicleDetails(tblPurchaseVehicleDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();

                            }
                        }
                    }
                }
                return result;

            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public ResultMessage UpdateBookingForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            DateTime currentDate = _iCommonDAO.ServerDateTime;
            Dictionary<string, object> enquiryDtlsDict = new Dictionary<string, object>();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 2. Update Booking Information

                TblPurchaseEnquiryTO existingTblBookingsTO = SelectTblBookingsForPurchaseTO(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);

                existingTblBookingsTO.BookingQty = tblPurchaseEnquiryTO.BookingQty;
                existingTblBookingsTO.PendingBookingQty = existingTblBookingsTO.BookingQty;
                existingTblBookingsTO.OptionalPendingQty = existingTblBookingsTO.PendingBookingQty;
                existingTblBookingsTO.BookingRate = tblPurchaseEnquiryTO.BookingRate;
                existingTblBookingsTO.UpdatedOn = tblPurchaseEnquiryTO.UpdatedOn;
                existingTblBookingsTO.UpdatedBy = tblPurchaseEnquiryTO.UpdatedBy;
                existingTblBookingsTO.IsConfirmed = tblPurchaseEnquiryTO.IsConfirmed;
                existingTblBookingsTO.ProdClassId = tblPurchaseEnquiryTO.ProdClassId;
                existingTblBookingsTO.Comments = tblPurchaseEnquiryTO.Comments;
                existingTblBookingsTO.IsConvertToSauda = tblPurchaseEnquiryTO.IsConvertToSauda;
                existingTblBookingsTO.BaseMetalCost = tblPurchaseEnquiryTO.BaseMetalCost;
                existingTblBookingsTO.CalculatedMetalCost = tblPurchaseEnquiryTO.CalculatedMetalCost;
                existingTblBookingsTO.Padta = tblPurchaseEnquiryTO.Padta;
                existingTblBookingsTO.DemandedRate = tblPurchaseEnquiryTO.DemandedRate;
                existingTblBookingsTO.SaudaCreatedOn = tblPurchaseEnquiryTO.SaudaCreatedOn;
                existingTblBookingsTO.COrNCId = tblPurchaseEnquiryTO.COrNCId;
                existingTblBookingsTO.SupplierId = tblPurchaseEnquiryTO.SupplierId;
                existingTblBookingsTO.Freight = tblPurchaseEnquiryTO.Freight;
                existingTblBookingsTO.IsFixed = tblPurchaseEnquiryTO.IsFixed;
                existingTblBookingsTO.TransportAmtPerMT = tblPurchaseEnquiryTO.TransportAmtPerMT;

                //Priyanka [07-01-2019]
                existingTblBookingsTO.DeliveryDays = tblPurchaseEnquiryTO.DeliveryDays;
                existingTblBookingsTO.NoOfVehicleSched = tblPurchaseEnquiryTO.NoOfVehicleSched;
                existingTblBookingsTO.Remark = tblPurchaseEnquiryTO.Remark;
                existingTblBookingsTO.WtRateApprovalDiff = tblPurchaseEnquiryTO.WtRateApprovalDiff;
                existingTblBookingsTO.WtActualRate = tblPurchaseEnquiryTO.WtActualRate;
                existingTblBookingsTO.VehicleTypeDesc = tblPurchaseEnquiryTO.VehicleTypeDesc;
                existingTblBookingsTO.SaudaTypeId = tblPurchaseEnquiryTO.SaudaTypeId;
                // existingTblBookingsTO.SupplierId = tblPurchaseEnquiryTO.SupplierId;


                #region 4. Check If Booking Item details falls within taregt qty. Update Grade Wise Target Qty

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_SHOW_GRADE_WISE_TRAGET_DETAILS, conn, tran);
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 1 && tblPurchaseEnquiryTO.IsUpdateGradeDtls == 1
                    )
                    {
                        List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyUpdateTOList = new List<TblGradeWiseTargetQtyTO>();
                        List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyInsertTOList = new List<TblGradeWiseTargetQtyTO>();
                        Boolean isUpdateGradeDtls = CheckIfGradeWithinTargetQty(existingTblBookingsTO, tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList, tblGradeWiseTargetQtyUpdateTOList, tblGradeWiseTargetQtyInsertTOList, conn, tran);
                        if (!isUpdateGradeDtls)
                        {
                            //Go for approval
                            tblPurchaseEnquiryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL);
                            tblPurchaseEnquiryTO.AuthReasons += " | TRAGET QTY";
                            tblPurchaseEnquiryTO.IsConvertToSauda = 0;
                        }

                        if (tblGradeWiseTargetQtyUpdateTOList != null && tblGradeWiseTargetQtyUpdateTOList.Count > 0)
                        {
                            for (int p = 0; p < tblGradeWiseTargetQtyUpdateTOList.Count; p++)
                            {
                                result = _iTblGradeWiseTargetQtyBL.UpdateTblGradeWiseTargetQty(tblGradeWiseTargetQtyUpdateTOList[p], conn, tran);
                                if (result <= 0)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Error While Updating Grade Wise Balance Qty Details in Function SaveNewBooking";
                                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                        }

                        if (tblGradeWiseTargetQtyInsertTOList != null && tblGradeWiseTargetQtyInsertTOList.Count > 0)
                        {
                            for (int g = 0; g < tblGradeWiseTargetQtyInsertTOList.Count; g++)
                            {
                                result = _iTblGradeWiseTargetQtyBL.InsertTblGradeWiseTargetQty(tblGradeWiseTargetQtyInsertTOList[g], conn, tran);
                                if (result <= 0)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Error While Inserting Grade Wise Balance Qty Details in Function SaveNewBooking";
                                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                        }

                    }
                }

                #endregion



                #region  Check If padta calculations are in configuration limits
                int Result = 0;
                TblConfigParamsTO tblConfigParamsTOPadta = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_PADTA_CONFIG_LIMIT_FOR_ENQUIRY_AND_SAUDA_APPROVAL, conn, tran);
                if (tblConfigParamsTOPadta != null && tblPurchaseEnquiryTO.IsConvertToSauda == 1)
                {
                    if (tblConfigParamsTOPadta.ConfigParamVal.ToString() == "0")
                    {
                        Result = 1;
                    }
                    else
                    {
                        int minLimit;
                        int maxLimit;
                        string[] arr = tblConfigParamsTOPadta.ConfigParamVal.Split(',');
                        minLimit = Convert.ToInt32(arr[0]);
                        maxLimit = Convert.ToInt32(arr[1]);

                        double padta = 0;

                        padta = tblPurchaseEnquiryTO.Padta;

                        if (tblPurchaseEnquiryTO.COrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
                        {
                            padta = tblPurchaseEnquiryTO.Padta;
                        }
                        if (minLimit <= padta && padta <= maxLimit)
                        {
                            Result = 1;
                        }
                        else
                        {
                            tblPurchaseEnquiryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL);
                            tblPurchaseEnquiryTO.AuthReasons += " |" + Constants.AuthReasonIdsE.PADTA_OUT_OF_BAND.ToString();
                            tblPurchaseEnquiryTO.IsConvertToSauda = 0;
                        }
                    }
                }

                #endregion

                existingTblBookingsTO.CurrencyId = tblPurchaseEnquiryTO.CurrencyId;
                existingTblBookingsTO.ContractTypeId = tblPurchaseEnquiryTO.ContractTypeId;
                existingTblBookingsTO.ContractComment = tblPurchaseEnquiryTO.ContractComment;
                existingTblBookingsTO.StatusId = tblPurchaseEnquiryTO.StatusId;
                existingTblBookingsTO.AuthReasons = tblPurchaseEnquiryTO.AuthReasons;
                existingTblBookingsTO.IsConvertToSauda = tblPurchaseEnquiryTO.IsConvertToSauda;

                existingTblBookingsTO.ContractNumber = tblPurchaseEnquiryTO.ContractNumber;
                existingTblBookingsTO.ContractDate = tblPurchaseEnquiryTO.ContractDate;
                existingTblBookingsTO.ImpuritiesTolerance = tblPurchaseEnquiryTO.ImpuritiesTolerance;
                existingTblBookingsTO.WeighmentTolerance = tblPurchaseEnquiryTO.WeighmentTolerance;
                existingTblBookingsTO.IndentureName = tblPurchaseEnquiryTO.IndentureName;
                existingTblBookingsTO.CountryOfOrigin = tblPurchaseEnquiryTO.CountryOfOrigin;
                existingTblBookingsTO.FinalPlaceOfDelivery = tblPurchaseEnquiryTO.FinalPlaceOfDelivery;
                existingTblBookingsTO.PortOfDischarge = tblPurchaseEnquiryTO.PortOfDischarge;
                existingTblBookingsTO.PortOfLoading = tblPurchaseEnquiryTO.PortOfLoading;
                existingTblBookingsTO.AverageLoading = tblPurchaseEnquiryTO.AverageLoading;


                if (existingTblBookingsTO.CurrencyId == null || existingTblBookingsTO.CurrencyId <= 0)
                {
                    existingTblBookingsTO.CurrencyId = (Int32)Constants.CurrencyE.INR;
                }
                if (Startup.IsForBRM)
                {
                    existingTblBookingsTO.SaudaTypeId = (Int32)Constants.SaudaTypeE.TONNAGE_QTY;
                    existingTblBookingsTO.CurrencyId = (Int32)Constants.CurrencyE.INR;
                }

                if (existingTblBookingsTO.IsConvertToSauda == 1)
                    existingTblBookingsTO.SaudaCreatedOn = currentDate;

                existingTblBookingsTO.RateForC = existingTblBookingsTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(existingTblBookingsTO.StateId, true, conn, tran);
                existingTblBookingsTO.RateForNC = existingTblBookingsTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(existingTblBookingsTO.StateId, false, conn, tran);


                existingTblBookingsTO.RefRateofV48Var = _iTblConfigParamsBL.GetCurrentValueOfV8RefVar(Constants.CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE);
                existingTblBookingsTO.RefRateC = _iTblConfigParamsBL.GetCurrentValueOfV8RefVar(Constants.CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE_C);

                result = UpdateTblBookingsForPurchase(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateBookingForPurchase";
                    resultMessage.Tag = tblPurchaseEnquiryTO;
                    return resultMessage;
                }

                //Priyanka [03-01-2019] : Added to insert into tblPurchaseEnquiryHistory
                existingTblBookingsTO.CreatedOn = currentDate;
                existingTblBookingsTO.Comments = tblPurchaseEnquiryTO.Comments;
                result = InsertTblBookingHistory(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingHistory";
                    resultMessage.Tag = tblPurchaseEnquiryTO;
                    return resultMessage;
                }
                #endregion

                //Save enquiry details
                resultMessage = SavePurchaseEnquiryDtls(tblPurchaseEnquiryTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    throw new Exception("Erorr In SavePurchaseEnquiryDtls");
                }

                #region 6.Save enquiry schedule 
                if (tblPurchaseEnquiryTO.BookingScheduleTOList != null && tblPurchaseEnquiryTO.BookingScheduleTOList.Count > 0)
                {
                    for (int k = 0; k < tblPurchaseEnquiryTO.BookingScheduleTOList.Count; k++)
                    {
                        TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = tblPurchaseEnquiryTO.BookingScheduleTOList[k];

                        TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTempTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTO(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, true, conn, tran);
                        if (tblPurchaseScheduleSummaryTempTO == null)
                        {
                            tblPurchaseScheduleSummaryTO.CreatedOn = currentDate;
                            tblPurchaseScheduleSummaryTO.UpdatedOn = currentDate;
                            result = _iTblPurchaseScheduleSummaryBL.InsertTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO, conn, tran);
                            if (result == 1)
                            {
                                if (tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                                {
                                    for (int i = 0; i < tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count; i++)
                                    {
                                        TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO = tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList[i];
                                        tblPurchaseVehicleDetailsTO.SchedulePurchaseId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                                        result = _iTblPurchaseVehicleDetailsBL.InsertTblPurchaseVehicleDetails(tblPurchaseVehicleDetailsTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            tran.Rollback();
                                            resultMessage.Text = "Error While Inserting Vehicle Schedule Item Details";
                                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            return resultMessage;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error While Inserting Vehicle Schedule Details";
                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }
                        else
                        {
                            //Update Purchase Schedule Details
                            result = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO, conn, tran);
                            if (result >= 1)
                            {
                                //First Delete previous Item Details
                                Boolean isGetGradeExpDtls = true;
                                List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOTempList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, isGetGradeExpDtls, conn, tran);
                                if (tblPurchaseVehicleDetailsTOTempList != null && tblPurchaseVehicleDetailsTOTempList.Count > 0)
                                {
                                    for (int b = 0; b < tblPurchaseVehicleDetailsTOTempList.Count; b++)
                                    {
                                        result = _iTblPurchaseVehicleDetailsBL.DeleteTblPurchaseVehicleDetails(tblPurchaseVehicleDetailsTOTempList[b].IdVehiclePurchase, conn, tran);
                                        if (result <= 0)
                                        {
                                            tran.Rollback();
                                            resultMessage.Text = "Error While Updating Vehicle Schedule Details";
                                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            return resultMessage;
                                        }
                                    }
                                }

                                if (result >= 1)
                                {
                                    if (tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                                    {
                                        for (int a = 0; a < tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count; a++)
                                        {
                                            tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList[a].SchedulePurchaseId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                                            result = _iTblPurchaseVehicleDetailsBL.InsertTblPurchaseVehicleDetails(tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList[a], conn, tran);
                                            if (result <= 0)
                                            {
                                                tran.Rollback();
                                                resultMessage.Text = "Error While Updating Vehicle Schedule Details";
                                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                return resultMessage;
                                            }

                                        }
                                    }
                                }

                            }

                        }
                    }

                }
                else
                {

                }
                #endregion


                #region Vehicle Type Insertion

                resultMessage = SavePurchaseVehicleTypeDesc(tblPurchaseEnquiryTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Error in SavePurchaseVehicleTypeDesc(tblPurchaseEnquiryTO,conn,tran);";
                    return resultMessage;
                }

                #endregion

                #region Notifications & SMSs

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();

                if (tblPurchaseEnquiryTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL))
                {
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_DIRECTOR;
                    tblAlertInstanceTO.AlertAction = "Scrap Enquiry Approval";
                    tblAlertInstanceTO.AlertComment = "Scrap enquiry #" + tblPurchaseEnquiryTO.EnqDisplayNo + " is awaiting for director approval.";
                    tblAlertInstanceTO.EffectiveFromDate = tblPurchaseEnquiryTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_DIRECTOR";
                    tblAlertInstanceTO.SourceEntityId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                    tblAlertInstanceTO.RaisedBy = tblPurchaseEnquiryTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblPurchaseEnquiryTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    //Sanjay [21 sept 2018] Below code is commented and common notification API is called

                    notify.SendNotificationToUsers(tblAlertInstanceTO);
                }

                //ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //if (rMessage.MessageType != ResultMessageE.Information)
                //{
                //    tran.Rollback();
                //    resultMessage.DefaultBehaviour();
                //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //    resultMessage.Text = "Error While Generating Notification";

                //    return resultMessage;
                //}
                #endregion            
                
                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                if (tblPurchaseEnquiryTO.IsConvertToSauda == 1)
                {
                    // if (tblPurchaseEnquiryTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL))
                    // {
                    //     resultMessage.Text = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully. Send For Directors Approval.";
                    //     resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully. Send For Directors Approval.";
                    // }
                    // else
                    // {
                    resultMessage.Text = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                    resultMessage.DisplayMessage = "Enquiry # - " + tblPurchaseEnquiryTO.EnqDisplayNo + " is generated Successfully.";
                    //}

                }
                else
                {
                    resultMessage.Text = "Purchase Enquiry Updated Sucessfully";
                    resultMessage.DisplayMessage = "Purchase Enquiry Updated Sucessfully";
                }

                resultMessage.Result = 1;
                enquiryDtlsDict.Add("purchaseEnquiryTO", tblPurchaseEnquiryTO);
                resultMessage.Tag = enquiryDtlsDict;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method UpdateBookingConfirmations";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage AddUpdateShipmentDetails(List<TblpurchaseEnqShipmemtDtlsTO> tblpurchaseEnqShipmemtDtlsTOList,int loginUserId)
        {
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Add / Update Shipment Details Against Enquiry.
                if (tblpurchaseEnqShipmemtDtlsTOList != null && tblpurchaseEnqShipmemtDtlsTOList.Count>0)
                {
                    if (tblpurchaseEnqShipmemtDtlsTOList[0].PurchaseEnquiryId > 0)
                    {
                        List<TblpurchaseEnqShipmemtDtlsTO> listTemp = GetShipmentDetailsByPurchaseEnquiryId(tblpurchaseEnqShipmemtDtlsTOList[0].PurchaseEnquiryId);
                        if (listTemp != null && listTemp.Count > 0)
                        {
                            for (int shipment = 0; shipment < listTemp.Count; shipment++)
                            {
                                TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO = listTemp[shipment];
                                if (tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls > 0)
                                {
                                    tblpurchaseEnqShipmemtDtlsTO.IsActive = 0;

                                    result = _iTblpurchaseEnqShipmemtDtlsBL.UpdateTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO, conn, tran);
                                    if (result <= 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour("UpdateTblpurchaseEnqShipmemtDtls failed");
                                        return resultMessage;
                                    }
                                }

                            }
                        }
                    }
                        for (int shipment = 0; shipment < tblpurchaseEnqShipmemtDtlsTOList.Count; shipment++) {
                        TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO = tblpurchaseEnqShipmemtDtlsTOList[shipment];
                        tblpurchaseEnqShipmemtDtlsTO.CreatedBy = loginUserId;
                        tblpurchaseEnqShipmemtDtlsTO.UpdatedBy = loginUserId;
                        tblpurchaseEnqShipmemtDtlsTO.IsActive = 1;
                        tblpurchaseEnqShipmemtDtlsTO.CreatedOn = _iCommonDAO.ServerDateTime;
                        tblpurchaseEnqShipmemtDtlsTO.UpdatedOn = _iCommonDAO.ServerDateTime;
                        if (tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls > 0)
                        {
                            result = _iTblpurchaseEnqShipmemtDtlsBL.UpdateTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO, conn, tran);
                            if (result <= 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("UpdateTblpurchaseEnqShipmemtDtls failed");
                                return resultMessage;
                            }
                            #region Update existing Shipment Ext details
                            List<TblpurchaseEnqShipmemtDtlsExtTO> TblpurchaseEnqShipmemtDtlsExtTOList = _iITblpurchaseEnqShipmemtDtlsExtBL.SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls);
                            if (TblpurchaseEnqShipmemtDtlsExtTOList != null && TblpurchaseEnqShipmemtDtlsExtTOList.Count > 0)
                            {
                                for (int ship = 0; ship < TblpurchaseEnqShipmemtDtlsExtTOList.Count; ship++)
                                {
                                    TblpurchaseEnqShipmemtDtlsExtTOList[ship].IsActive = 0;
                                    TblpurchaseEnqShipmemtDtlsExtTOList[ship].UpdatedOn = _iCommonDAO.ServerDateTime;
                                    TblpurchaseEnqShipmemtDtlsExtTOList[ship].UpdatedBy = loginUserId;
                                    result = _iITblpurchaseEnqShipmemtDtlsExtBL.UpdateTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTOList[ship], conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour("InsertTblpurchaseEnqShipmemtDtls failed");
                                        return resultMessage;
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            result = _iTblpurchaseEnqShipmemtDtlsBL.InsertTblpurchaseEnqShipmemtDtls(tblpurchaseEnqShipmemtDtlsTO, conn, tran);
                            if (result <= 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("InsertTblpurchaseEnqShipmemtDtls failed");
                                return resultMessage;
                            }
                        }

                        if (tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList != null && tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList.Count > 0)
                        {
                            for (int i = 0; i < tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList.Count; i++)
                            {
                                TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO = new TblpurchaseEnqShipmemtDtlsExtTO();
                                tblpurchaseEnqShipmemtDtlsExtTO = tblpurchaseEnqShipmemtDtlsTO.TblpurchaseEnqShipmemtDtlsExtTOList[i];
                                tblpurchaseEnqShipmemtDtlsExtTO.CreatedBy = loginUserId;
                                tblpurchaseEnqShipmemtDtlsExtTO.CreatedOn = _iCommonDAO.ServerDateTime;
                                tblpurchaseEnqShipmemtDtlsExtTO.IsActive = 1;
                                tblpurchaseEnqShipmemtDtlsExtTO.ShipmentDtlsId = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;
                                result = _iITblpurchaseEnqShipmemtDtlsExtBL.InsertTblpurchaseEnqShipmemtDtlsExt(tblpurchaseEnqShipmemtDtlsExtTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("InsertTblpurchaseEnqShipmemtDtls failed");
                                    return resultMessage;
                                }
                            }
                        }
                    }
                }

                tran.Commit();
                resultMessage.Result = 1;
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text ="Record Saved Successfully";
                return resultMessage;
                #endregion
            }
            catch(Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method AddUpdateShipmentDetails";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        //Prajakta[2019-01-22] Delete previous and add new enquiry details 
        public ResultMessage SavePurchaseEnquiryDtls(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            int result = 0;

            try
            {

                List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTOList = _iTblPurchaseEnquiryDetailsBL.SelectAllTblEnquiryDetailsListByEnquiryId(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);

                if (tblPurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryDetailsTOList.Count > 0)
                {
                    for (int l = 0; l < tblPurchaseEnquiryDetailsTOList.Count; l++)
                    {

                        if (tblPurchaseEnquiryDetailsTOList[l].GradeExpressionDtlsTOList != null && tblPurchaseEnquiryDetailsTOList[l].GradeExpressionDtlsTOList.Count > 0)
                        {
                            for (int d = 0; d < tblPurchaseEnquiryDetailsTOList[l].GradeExpressionDtlsTOList.Count; d++)
                            {
                                result = _iTblGradeExpressionDtlsBL.DeleteTblGradeExpressionDtls(tblPurchaseEnquiryDetailsTOList[l].GradeExpressionDtlsTOList[d].IdGradeExpressionDtls, conn, tran);
                                if (result <= 0)
                                {
                                    throw new Exception("Error In DeleteTblGradeExpressionDtls");
                                }

                            }
                        }

                        result = _iTblPurchaseEnquiryDetailsBL.DeleteTblPurchaseEnquiryDetails(tblPurchaseEnquiryDetailsTOList[l].IdPurchaseEnquiryDetails, conn, tran);
                        if (result == -1)
                        {
                            throw new Exception("Error In DeleteTblPurchaseEnquiryDetails");
                        }

                    }
                }


                #endregion

                #region 3.1. Save Materialwise Qty and Rate
                if (tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count > 0)
                {
                    for (int j = 0; j < tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count; j++)
                    {
                        TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO = tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[j];
                        tblPurchaseEnquiryDetailsTO.PurchaseEnquiryId = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                        // tblPurchaseEnquiryDetailsTO.Rate = tblPurchaseEnquiryDetailsTO.ProdItemId; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                        //tblPurchaseEnquiryDetailsTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                        result = _iTblPurchaseEnquiryDetailsBL.InsertTblPurchaseEnquiryDetails(tblPurchaseEnquiryDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            throw new Exception("Error In InsertTblPurchaseEnquiryDetails");
                        }
                        else
                        {
                            //Save grade expreesion details
                            if (tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList != null && tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList.Count > 0)
                            {
                                tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList = tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList.Where(w => w.ExpressionDtlsId > 0).ToList();

                                for (int d = 0; d < tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList.Count; d++)
                                {
                                    tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList[d].PurchaseEnquiryDtlsId = tblPurchaseEnquiryDetailsTO.IdPurchaseEnquiryDetails;
                                    result = _iTblGradeExpressionDtlsBL.InsertTblGradeExpressionDtls(tblPurchaseEnquiryDetailsTO.GradeExpressionDtlsTOList[d], conn, tran);
                                    if (result != 1)
                                    {
                                        throw new Exception("Error In InsertTblGradeExpressionDtls");
                                    }

                                }
                            }
                        }
                    }
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (System.Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SavePurchaseEnquiryDtls(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection, SqlConnection conn, SqlTransaction tran)");
                return resultMessage;
            }

        }

        public ResultMessage UpdateBookingForSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            int result = 0;
            try
            {
                result = _iTblPurchaseEnquiryDAO.UpdateTblBookingsForConverToSauda(tblPurchaseEnquiryTO);

                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateBookingForSauda";
                    resultMessage.Tag = tblPurchaseEnquiryTO;
                    return resultMessage;
                }

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Purchase Enquiry Converted To Sauda Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblPurchaseEnquiryTO;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method UpdateBookingConfirmations";
                return resultMessage;
            }
            finally
            {

            }
        }

        public int UpdateTblBookingsForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.UpdateTblBookingsForPurchase(tblPurchaseEnquiryTO, conn, tran);
        }

        public ResultMessage CloseOpenQtySauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Double totalScheduleQty = 0;
                // //Get All Schedule for enquiry
                // List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryTOList = TblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummary(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);
                // if (TblPurchaseScheduleSummaryTOList != null && TblPurchaseScheduleSummaryTOList.Count > 0)
                // {
                //     totalScheduleQty = TblPurchaseScheduleSummaryTOList.Select(a => a.Qty).Sum();
                // }

                tblPurchaseEnquiryTO.BookingQty = Math.Abs(tblPurchaseEnquiryTO.PendingBookingQty);
                //tblPurchaseEnquiryTO.PendingBookingQty = 0;

                tblPurchaseEnquiryTO.RateForC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, true, conn, tran);
                tblPurchaseEnquiryTO.RateForNC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, false, conn, tran);

                result = UpdateTblBookingsForPurchase(tblPurchaseEnquiryTO, conn, tran);

                Int32 isAuto = 0;
                Int32 statusId = (Int32)Constants.TranStatusE.COMPLETED;
                resultMessage = _iTblPurchaseScheduleSummaryBL.SetStatusCompleteAfterCorrection(tblPurchaseEnquiryTO.IdPurchaseEnquiry, tblPurchaseEnquiryTO.UpdatedBy, isAuto, conn, tran, statusId);

                if (result <= 0)
                {
                    throw new Exception("Error while updating tblPurchaseEnquiryTO ");
                }

                if (result >= 1)
                {
                    if (resultMessage.Result <= 0)
                    {
                        tran.Rollback();
                        return resultMessage;
                    }
                    else
                    {
                        tran.Commit();
                        resultMessage.DefaultSuccessBehaviour("Sauda - # " + tblPurchaseEnquiryTO.EnqDisplayNo + " Completed successfully.");
                        return resultMessage;
                    }

                }
                else
                {
                    tran.Rollback();
                    throw new Exception("Error While Record Save : CloseOpenQtySauda");
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "CloseOpenQtySauda");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public Int32 UpdatePendingBookingQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, Boolean isCheckForExistingQty, TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            Double totalScheduleQty = 0;
            Int32 result = 0;
            //Get All Schedule for enquiry

            //TblPurchaseEnquiryTO tblPurchaseEnquiryTO = SelectTblBookingsForPurchaseTO(purchaseEnquiryId, conn, tran);
            if (tblPurchaseEnquiryTO != null)
            {
                //Prajakta[2019-04-05] Commented and added to update booking qty against new schedule
                // List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryTOList = TblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummary(tblPurchaseEnquiryTO.IdPurchaseEnquiry, conn, tran);
                // List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryTOList = TblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(tblPurchaseEnquiryTO.IdPurchaseEnquiry, 0, conn, tran);
                // if (TblPurchaseScheduleSummaryTOList != null && TblPurchaseScheduleSummaryTOList.Count > 0)
                // {
                //     totalScheduleQty = TblPurchaseScheduleSummaryTOList.Select(a => a.Qty).Sum();
                // }

                //Prajakta[2019-04-12] Added
                if (scheduleTO != null)
                {
                    totalScheduleQty = scheduleTO.Qty;

                    if (scheduleTO.StatusId == (Int32)Constants.TranStatusE.New)
                    {
                        if (isCheckForExistingQty)
                        {
                            TblPurchaseScheduleSummaryTO existingScheduleTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTO(scheduleTO.IdPurchaseScheduleSummary, false, conn, tran);
                            if (existingScheduleTO != null)
                            {
                                if (existingScheduleTO.StatusId == (Int32)Constants.TranStatusE.New)
                                {
                                    tblPurchaseEnquiryTO.PendingBookingQty = tblPurchaseEnquiryTO.PendingBookingQty + existingScheduleTO.Qty;
                                    tblPurchaseEnquiryTO.OptionalPendingQty = tblPurchaseEnquiryTO.PendingBookingQty;
                                }

                            }
                        }
                        tblPurchaseEnquiryTO.PendingBookingQty = tblPurchaseEnquiryTO.PendingBookingQty - totalScheduleQty;
                        tblPurchaseEnquiryTO.OptionalPendingQty = tblPurchaseEnquiryTO.PendingBookingQty;
                    }

                    if (scheduleTO.StatusId == (Int32)Constants.TranStatusE.DELETE_VEHICLE)
                    {
                        tblPurchaseEnquiryTO.PendingBookingQty = tblPurchaseEnquiryTO.PendingBookingQty + totalScheduleQty;
                        tblPurchaseEnquiryTO.OptionalPendingQty = tblPurchaseEnquiryTO.PendingBookingQty;
                    }


                    //tblPurchaseEnquiryTO.PendingBookingQty = tblPurchaseEnquiryTO.BookingQty - totalScheduleQty;
                    tblPurchaseEnquiryTO.RateForC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, true, conn, tran);
                    tblPurchaseEnquiryTO.RateForNC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, false, conn, tran);

                    result = UpdateTblBookingsForPurchase(tblPurchaseEnquiryTO, conn, tran);
                    if (result <= 0)
                    {
                        return -1;
                    }
                }
                else
                {
                    return 1;
                }


            }
            else
            {
                return -1;
            }

            return result;

        }

        //Priyanka [03-01-2019]

        public int UpdateTblBookings(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.UpdateTblBookings(tblBookingsTO, conn, tran);
        }
        public int InsertTblBookingHistory(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.ExecuteInsertionCommandForHistory(tblBookingsTO, conn, tran);
        }

        public ResultMessage UpdateBookingConfirmations(TblPurchaseEnquiryTO tblBookingsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            DateTime currentDate = _iCommonDAO.ServerDateTime;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Add Record in tblBookingBeyondQuota For History

                TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new TblPurchaseBookingBeyondQuotaTO();
                tblBookingBeyondQuotaTO = tblBookingsTO.GetBookingBeyondQuotaTO();

                tblBookingsTO.UpdatedOn = _iCommonDAO.ServerDateTime;
                tblBookingBeyondQuotaTO.CreatedBy = tblBookingsTO.UpdatedBy;
                tblBookingBeyondQuotaTO.CreatedOn = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusDate = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusRemark = tblBookingsTO.StatusRemark;

                #endregion

                #region 2. Update Booking Information

                TblPurchaseEnquiryTO existingTblBookingsTO = SelectTblBookingsTO(tblBookingsTO.IdPurchaseEnquiry, conn, tran);

                existingTblBookingsTO.StatusId = tblBookingsTO.StatusId;
                existingTblBookingsTO.StatusDate = tblBookingsTO.StatusDate;
                existingTblBookingsTO.UpdatedOn = tblBookingsTO.UpdatedOn;
                existingTblBookingsTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                existingTblBookingsTO.Comments = tblBookingsTO.Comments;
                //existingTblBookingsTO.StatusRemark = tblBookingsTO.StatusRemark;
                existingTblBookingsTO.BookingRate = tblBookingsTO.BookingRate;
                existingTblBookingsTO.BookingQty = tblBookingsTO.BookingQty;
                existingTblBookingsTO.AuthReasons = tblBookingsTO.AuthReasons;
                existingTblBookingsTO.IsConvertToSauda = tblBookingsTO.IsConvertToSauda;
                existingTblBookingsTO.IdPurchaseEnquiry = tblBookingsTO.IdPurchaseEnquiry;
                existingTblBookingsTO.IsConvertToSauda = tblBookingsTO.IsConvertToSauda;
                existingTblBookingsTO.PendingBookingQty = tblBookingsTO.PendingBookingQty;
                existingTblBookingsTO.OptionalPendingQty = tblBookingsTO.OptionalPendingQty;
                existingTblBookingsTO.WtRateApprovalDiff = tblBookingsTO.WtRateApprovalDiff;
                existingTblBookingsTO.WtActualRate = tblBookingsTO.WtActualRate;

                if (tblBookingsTO.IsConvertToSauda == 1)
                {
                    existingTblBookingsTO.SaudaCreatedOn = _iCommonDAO.ServerDateTime;
                }
                if (tblBookingsTO.Bookingpmrate > 0)
                    existingTblBookingsTO.Bookingpmrate = tblBookingsTO.Bookingpmrate;

                if (existingTblBookingsTO.CurrencyId == null || existingTblBookingsTO.CurrencyId <= 0)
                {
                    existingTblBookingsTO.CurrencyId = (Int32)Constants.CurrencyE.INR;
                }
                if (Startup.IsForBRM)
                {
                    existingTblBookingsTO.SaudaTypeId = (Int32)Constants.SaudaTypeE.TONNAGE_QTY;
                    existingTblBookingsTO.CurrencyId = (Int32)Constants.CurrencyE.INR;
                }

                if (existingTblBookingsTO.IsConvertToSauda == 1)
                    existingTblBookingsTO.SaudaCreatedOn = currentDate;

                result = UpdateTblBookings(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblBookings";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                //Priyanka [03-01-2019] : Added to create history of enquiry.
                existingTblBookingsTO.Comments = tblBookingsTO.Comments;
                existingTblBookingsTO.CreatedOn = currentDate;
                result = InsertTblBookingHistory(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingHistory";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }
                #endregion



                if (tblBookingsTO.StatusId == (Int32)Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTOR)
                {

                    List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTOList = _iTblPurchaseEnquiryDetailsBL.SelectAllTblEnquiryDetailsListByEnquiryId(tblBookingsTO.IdPurchaseEnquiry, conn, tran);

                    List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyUpdateTOList = new List<TblGradeWiseTargetQtyTO>();
                    if (tblPurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryDetailsTOList.Count > 0)
                    {
                        List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTOList = _iTblGradeWiseTargetQtyBL.SelectGradeWiseTargetQtyDtls(tblBookingsTO.RateBandDeclarationPurchaseId, tblBookingsTO.UserId, conn, tran);
                        if (tblGradeWiseTargetQtyTOList != null && tblGradeWiseTargetQtyTOList.Count > 0)
                        {
                            for (int p = 0; p < tblPurchaseEnquiryDetailsTOList.Count; p++)
                            {
                                List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTempTOList = tblGradeWiseTargetQtyTOList.Where(s => s.ProdItemId == tblPurchaseEnquiryDetailsTOList[p].ProdItemId).ToList();
                                if (tblGradeWiseTargetQtyTempTOList != null && tblGradeWiseTargetQtyTempTOList.Count == 1)
                                {
                                    tblGradeWiseTargetQtyTempTOList[0].PendingBookingQty = tblGradeWiseTargetQtyTempTOList[0].PendingBookingQty + tblPurchaseEnquiryDetailsTOList[p].Qty;
                                    tblGradeWiseTargetQtyUpdateTOList.Add(tblGradeWiseTargetQtyTempTOList[0]);
                                }
                            }
                        }
                    }

                    if (tblGradeWiseTargetQtyUpdateTOList != null && tblGradeWiseTargetQtyUpdateTOList.Count > 0)
                    {
                        for (int p = 0; p < tblGradeWiseTargetQtyUpdateTOList.Count; p++)
                        {
                            result = _iTblGradeWiseTargetQtyBL.UpdateTblGradeWiseTargetQty(tblGradeWiseTargetQtyUpdateTOList[p], conn, tran);
                            if (result <= 0)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error While Updating Grade Wise Balance Qty Details in Function UpdateBookingConfirmations";
                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }
                    }

                }
                else
                {
                    //Prajakta[2019-01-22] Added
                    //Save enquiry details
                    if (tblBookingsTO.StatusId != (int)Constants.TranStatusE.BOOKING_DELETE)
                    {
                        resultMessage = SavePurchaseEnquiryDtls(tblBookingsTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            throw new Exception("Erorr In SavePurchaseEnquiryDtls");
                        }
                    }
                }



                // #region Update enquiry item details
                // if (tblBookingsTO.PurchaseEnquiryDetailsTOList != null && tblBookingsTO.PurchaseEnquiryDetailsTOList.Count > 0)
                // {
                //     existingTblBookingsTO.PurchaseEnquiryDetailsTOList = tblBookingsTO.PurchaseEnquiryDetailsTOList;
                //     for (int q = 0; q < existingTblBookingsTO.PurchaseEnquiryDetailsTOList.Count; q++)
                //     {
                //         result = BL.TblPurchaseEnquiryDetailsBL.UpdateTblPurchaseEnquiryDetails(existingTblBookingsTO.PurchaseEnquiryDetailsTOList[q], conn, tran);
                //         if (result <= 0)
                //         {
                //             throw new Exception("Error In UpdateTblPurchaseEnquiryDetails()");
                //         }
                //     }
                // }

                #endregion

                #region Notifications & SMSs

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                if (existingTblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_ACCEPTED_BY_DIRECTOR))
                {

                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    tblAlertUsersTO.UserId = existingTblBookingsTO.CnFOrgId;
                    tblAlertUsersTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                    tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_APPROVED_BY_DIRECTOR;
                    tblAlertUsersTOList.Add(tblAlertUsersTO);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_APPROVED_BY_DIRECTOR;
                    tblAlertInstanceTO.AlertAction = "SCRAP_ENQUIRY_APPROVED_BY_DIRECTOR";
                    tblAlertInstanceTO.AlertComment = "Your enquiry #" + existingTblBookingsTO.EnqDisplayNo + " is approved and converted to sauda.";
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = existingTblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "SCRAP_ENQUIRY_APPROVED_BY_DIRECTOR";
                    tblAlertInstanceTO.SourceEntityId = existingTblBookingsTO.IdPurchaseEnquiry;
                    tblAlertInstanceTO.RaisedBy = tblBookingsTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResetEnquiryApprovalAlert(existingTblBookingsTO, tblAlertInstanceTO);

                    //Sanjay [21 sept 2018] Below code is commented and common notification API is called

                    notify.SendNotificationToUsers(tblAlertInstanceTO);
                }
                else if (existingTblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.PENDING_FOR_PURCHASE_MANAGER_APPROVAL))
                {

                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    tblAlertUsersTO.UserId = existingTblBookingsTO.CnFOrgId;
                    tblAlertUsersTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                    tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_PM;
                    tblAlertUsersTOList.Add(tblAlertUsersTO);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_PM;
                    tblAlertInstanceTO.AlertAction = "ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_PM";
                    tblAlertInstanceTO.AlertComment = "Your scrap enquiry #" + existingTblBookingsTO.EnqDisplayNo + " is awaiting for acceptance.";
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = existingTblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_PM";
                    tblAlertInstanceTO.SourceEntityId = existingTblBookingsTO.IdPurchaseEnquiry;
                    tblAlertInstanceTO.RaisedBy = existingTblBookingsTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResetEnquiryApprovalAlert(existingTblBookingsTO, tblAlertInstanceTO);

                    //Sanjay [21 sept 2018] Below code is commented and common notification API is called

                    notify.SendNotificationToUsers(tblAlertInstanceTO);
                }
                else if (existingTblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTOR))
                {

                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    tblAlertUsersTO.UserId = existingTblBookingsTO.CnFOrgId;
                    tblAlertUsersTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                    tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_REJECTED_BY_DIRECTOR;

                    tblAlertUsersTOList.Add(tblAlertUsersTO);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_REJECTED_BY_DIRECTOR;
                    tblAlertInstanceTO.AlertAction = "ENQUIRY_REJECTED_BY_DIRECTOR";
                    tblAlertInstanceTO.AlertComment = "Your enquiry #" + existingTblBookingsTO.EnqDisplayNo + " is rejected. Booking Rate: " + existingTblBookingsTO.BookingRate + "(Rs/MT) Booking Qty: " + existingTblBookingsTO.BookingQty + "(MT)";
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = existingTblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "ENQUIRY_REJECTED_BY_DIRECTOR";
                    tblAlertInstanceTO.SourceEntityId = existingTblBookingsTO.IdPurchaseEnquiry;
                    tblAlertInstanceTO.RaisedBy = existingTblBookingsTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResetEnquiryApprovalAlert(existingTblBookingsTO, tblAlertInstanceTO);

                    //Sanjay [21 sept 2018] Below code is commented and common notification API is called

                    notify.SendNotificationToUsers(tblAlertInstanceTO);
                }
                else if (existingTblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_DELETE))
                {
                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    tblAlertUsersTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                    tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_DELETED;
                    if (existingTblBookingsTO.UserId != existingTblBookingsTO.UpdatedBy)
                    {
                        tblAlertUsersTO.UserId = existingTblBookingsTO.UserId;
                    }

                    tblAlertUsersTOList.Add(tblAlertUsersTO);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_DELETED;
                    tblAlertInstanceTO.AlertAction = "BOOKING_DELETED";
                    tblAlertInstanceTO.AlertComment = "Booking #" + existingTblBookingsTO.EnqDisplayNo + " is deleted.";
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = existingTblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "BOOKING_DELETED";
                    tblAlertInstanceTO.SourceEntityId = existingTblBookingsTO.IdPurchaseEnquiry;
                    tblAlertInstanceTO.RaisedBy = existingTblBookingsTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResetEnquiryApprovalAlert(existingTblBookingsTO, tblAlertInstanceTO);

                    //Sanjay [21 sept 2018] Below code is commented and common notification API is called

                    notify.SendNotificationToUsers(tblAlertInstanceTO);
                }




                //ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //if (rMessage.MessageType != ResultMessageE.Information)
                //{
                //    tran.Rollback();
                //    resultMessage.DefaultBehaviour();
                //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //    resultMessage.Text = "Error While Generating Notification";

                //    return resultMessage;
                //}


                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = resultMessage.DisplayMessage = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblBookingsTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Text = "Exception Error in Method UpdateBookingConfirmations";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        private void ResetEnquiryApprovalAlert(TblPurchaseEnquiryTO existingTblBookingsTO, TblAlertInstanceTO tblAlertInstanceTO)
        {
            //Reset Approval Pending Alert
            AlertsToReset alertsToReset = new AlertsToReset();
            ResetAlertInstanceTO resetAlertInstanceTO = new ResetAlertInstanceTO();
            resetAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.ENQUIRY_PENDING_FOR_ACCEPTANCE_BY_DIRECTOR;
            resetAlertInstanceTO.SourceEntityTxnId = existingTblBookingsTO.IdPurchaseEnquiry;
            alertsToReset.ResetAlertInstanceTOList.Add(resetAlertInstanceTO);
            tblAlertInstanceTO.AlertsToReset = alertsToReset;
        }

        public ResultMessage CloseSaudaManually(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                Int32 isAuto = 0;
                Int32 statusId = (Int32)Constants.TranStatusE.COMPLETED;
                if(tblPurchaseEnquiryTO.BookingQty == tblPurchaseEnquiryTO.PendingBookingQty)
                {
                    statusId = (Int32)Constants.TranStatusE.BOOKING_CANCELED;
                }
                resultMessage = _iTblPurchaseScheduleSummaryBL.SetStatusCompleteAfterCorrection(tblPurchaseEnquiryTO.IdPurchaseEnquiry, tblPurchaseEnquiryTO.UpdatedBy, isAuto, conn, tran, statusId,tblPurchaseEnquiryTO.SaudaCloseRemark);
                if (resultMessage.Result == 0)
                {
                    //resultMessage.DefaultBehaviour();
                    //resultMessage.DisplayMessage = "Failed to close sauda";
                    //resultMessage.MessageType = ResultMessageE.Information;
                    return resultMessage;
                }
                else if (resultMessage.Result == 1)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Sauda - " + tblPurchaseEnquiryTO.EnqDisplayNo + " closed successfully.";
                }
                else if (resultMessage.Result == 2)
                {
                    //went for approval
                    tblPurchaseEnquiryTO.Comments = tblPurchaseEnquiryTO.SaudaCloseRemark;
                    tblPurchaseEnquiryTO.StatusId = (Int32)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL;
                    int result = _iTblPurchaseEnquiryDAO.UpdateTblBookingsForPurchase(tblPurchaseEnquiryTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Failed to close sauda.";
                        return resultMessage;
                    }
                    tran.Commit();

                    resultMessage = _iTblPurchaseScheduleSummaryBL.SendNotificationsForSaudaCloseApproval(tblPurchaseEnquiryTO, true, 0, tblPurchaseEnquiryTO.UpdatedBy);
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Sauda - " + tblPurchaseEnquiryTO.EnqDisplayNo + " send for director's approval.";

                }
                return resultMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "CloseSaudaManually(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage ApproveRejectCloseSauda(TblPurchaseEnquiryTO enquiryTO, Int32 isApproveOrReject, Int32 loginUserId)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (enquiryTO == null)
                {
                    throw new Exception("enquiryTO == null");
                }

                enquiryTO.UpdatedOn = serverDate;
                enquiryTO.UpdatedBy = loginUserId;
                resultMessage = ApproveRejectCloseSaudaDtls(enquiryTO, isApproveOrReject, loginUserId, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                tran.Commit();
                //Notifications

                ResultMessage resultMsg = _iTblPurchaseScheduleSummaryBL.SendNotificationsForSaudaCloseApproval(enquiryTO, false, isApproveOrReject, loginUserId);
                if (resultMsg.MessageType != ResultMessageE.Information)
                {
                    return resultMsg;
                }

                return resultMessage;


            }
            catch (System.Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in ApproveRejectCloseSauda(Int32 purchaseEnquiryId,Int32 isApproveOrReject)");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }




        }

        public ResultMessage ApproveRejectCloseSaudaDtls(TblPurchaseEnquiryTO enquiryTO, Int32 isApproveOrReject, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            Int32 isAuto = 0;

            try
            {
                if (enquiryTO == null)
                {
                    throw new Exception("enquiryTO == null");
                }

                Int32 statusId = 0;

                if (isApproveOrReject == 1)
                {
                    statusId = (Int32)Constants.TranStatusE.COMPLETED;
                }
                else
                {
                    statusId = (Int32)Constants.TranStatusE.BOOKING_APPROVED;
                }

                enquiryTO.StatusId = statusId;
                enquiryTO.UpdatedOn = _iCommonDAO.ServerDateTime;
                if (isApproveOrReject == 1)
                {
                    resultMessage = _iTblPurchaseScheduleSummaryBL.CompleteSaudaStatusAndAddConsumptionEntry(enquiryTO, enquiryTO.PendingBookingQty, isAuto, loginUserId, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        return resultMessage;
                    }
                }
                else
                {
                    result = UpdateTblBookingsForPurchase(enquiryTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while updating complete status against enqNo - " + enquiryTO.EnqDisplayNo;
                        return resultMessage;
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                if (isApproveOrReject == 1)
                    resultMessage.DisplayMessage = "Sauda : " + enquiryTO.EnqDisplayNo + " closed successfully.";
                else if (isApproveOrReject == 0)
                    resultMessage.DisplayMessage = "Sauda : " + enquiryTO.EnqDisplayNo + " closure request rejected.";

                return resultMessage;
            }
            catch (System.Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "Error in ApproveRejectCloseSauda(TblPurchaseEnquiryTO enquiryTO,Int32 isApproveOrReject,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;
            }
        }

        public List<TblPurchaseEnquiryTO> SelectAllTodaysBookingsWithOpeningBalance(int cnfOrgId, int dealerOrgId, DateTime serverDate)
        {
            return _iTblPurchaseEnquiryDAO.SelectAllTodaysBookingsWithOpeningBalance(cnfOrgId, dealerOrgId, serverDate);
        }
        public List<TblPurchaseEnquiryTO> SelectAllPendingBookingsList(int cnfOrgId, int dealerOrgId, DateTime date, string v1, bool v2)
        {
            return _iTblPurchaseEnquiryDAO.SelectAllPendingBookingsList(cnfOrgId, dealerOrgId, date, v1, v2);
        }


       public List<TblpurchaseEnqShipmemtDtlsTO> GetShipmentDetailsByPurchaseEnquiryId(int purchaseEnquiryId)
        {
            #region Get Shipment Details If Available
            List<TblpurchaseEnqShipmemtDtlsTO> TblpurchaseEnqShipmemtDtlsTOList = _iTblpurchaseEnqShipmemtDtlsBL.SelectTblpurchaseEnqShipmemtDtlsTOByEnqId(purchaseEnquiryId);

            if (TblpurchaseEnqShipmemtDtlsTOList != null && TblpurchaseEnqShipmemtDtlsTOList.Count > 0)
            {
                for (int ship = 0; ship < TblpurchaseEnqShipmemtDtlsTOList.Count; ship++)
                {
                    TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList = _iITblpurchaseEnqShipmemtDtlsExtBL.SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(TblpurchaseEnqShipmemtDtlsTOList[ship].IdShipmentDtls);
                }
            }
            return TblpurchaseEnqShipmemtDtlsTOList;
            #endregion

        }

        //Deepali Added [21-06-2021] for task no 1152
        public List<TblpurchaseEnqShipmemtDtlsExtTO> GetShipmentDetailsByPurchaseEnquiryIdForReport(int purchaseEnquiryId)
        {
            #region Get Shipment Details If Available
            List<TblpurchaseEnqShipmemtDtlsExtTO> TblpurchaseEnqShipmemtDtlsExtTOList = new List<TblpurchaseEnqShipmemtDtlsExtTO>();
            List<TblpurchaseEnqShipmemtDtlsTO> TblpurchaseEnqShipmemtDtlsTOList = _iTblpurchaseEnqShipmemtDtlsBL.SelectTblpurchaseEnqShipmemtDtlsTOByEnqId(purchaseEnquiryId);
            int srNo = 0;
            if (TblpurchaseEnqShipmemtDtlsTOList != null && TblpurchaseEnqShipmemtDtlsTOList.Count > 0)
            {
                for (int ship = 0; ship < TblpurchaseEnqShipmemtDtlsTOList.Count; ship++)
                {
                    TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO = new TblpurchaseEnqShipmemtDtlsExtTO();
                    TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList = _iITblpurchaseEnqShipmemtDtlsExtBL.SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(TblpurchaseEnqShipmemtDtlsTOList[ship].IdShipmentDtls);
                    if (TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList != null && TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList.Count > 0)
                    {
                        for (int i = 0; i < TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList.Count; i++)
                        {
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].ShippingLine = TblpurchaseEnqShipmemtDtlsTOList[ship].ShippingLine;
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].BillNo = TblpurchaseEnqShipmemtDtlsTOList[ship].BillNo;
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].BeNo = TblpurchaseEnqShipmemtDtlsTOList[ship].BeNo;
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].BeDateStr = TblpurchaseEnqShipmemtDtlsTOList[ship].BeDateStr;
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].BillDateStr = TblpurchaseEnqShipmemtDtlsTOList[ship].BillDateStr;
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].SupplierName = TblpurchaseEnqShipmemtDtlsTOList[ship].SupplierName;
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].IndentureName = TblpurchaseEnqShipmemtDtlsTOList[ship].IndentureName;
                            srNo = srNo + 1;
                            TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i].SrNo = srNo;
                            TblpurchaseEnqShipmemtDtlsExtTOList.Add(TblpurchaseEnqShipmemtDtlsTOList[ship].TblpurchaseEnqShipmemtDtlsExtTOList[i]);
                        }
                    }
                    else
                    {
                        tblpurchaseEnqShipmemtDtlsExtTO.ShippingLine = TblpurchaseEnqShipmemtDtlsTOList[ship].ShippingLine;
                        tblpurchaseEnqShipmemtDtlsExtTO.BillNo = TblpurchaseEnqShipmemtDtlsTOList[ship].BillNo;
                        tblpurchaseEnqShipmemtDtlsExtTO.BeNo = TblpurchaseEnqShipmemtDtlsTOList[ship].BeNo;
                        tblpurchaseEnqShipmemtDtlsExtTO.BeDateStr = TblpurchaseEnqShipmemtDtlsTOList[ship].BeDateStr;
                        tblpurchaseEnqShipmemtDtlsExtTO.BillDateStr = TblpurchaseEnqShipmemtDtlsTOList[ship].BillDateStr;
                        tblpurchaseEnqShipmemtDtlsExtTO.SupplierName = TblpurchaseEnqShipmemtDtlsTOList[ship].SupplierName;
                        tblpurchaseEnqShipmemtDtlsExtTO.IndentureName = TblpurchaseEnqShipmemtDtlsTOList[ship].IndentureName;
                        srNo = srNo + 1;
                        tblpurchaseEnqShipmemtDtlsExtTO.SrNo = srNo;
                        TblpurchaseEnqShipmemtDtlsExtTOList.Add(tblpurchaseEnqShipmemtDtlsExtTO);

                    }
                }
            }
            return TblpurchaseEnqShipmemtDtlsExtTOList;
            #endregion

        }
        public ResultMessage PrintShipmentReport(int purchaseEnquiryId)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<TblpurchaseEnqShipmemtDtlsExtTO> TblReportsTOList = GetShipmentDetailsByPurchaseEnquiryIdForReport(purchaseEnquiryId);

            if (TblReportsTOList != null && TblReportsTOList.Count > 0)
            {
                DataSet printDataSet = new DataSet();
                DataTable headerDT = new DataTable();
                if (TblReportsTOList != null && TblReportsTOList.Count > 0)
                {
                    headerDT = _iCommonDAO.ToDataTable(TblReportsTOList);
                }
                headerDT.TableName = "headerDT";

                printDataSet.Tables.Add(headerDT);
                String ReportTemplateName = "ShipmentDetailsRpt";
               
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(ReportTemplateName);
                String fileName = "Doc-" + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    String filePath = String.Empty;
                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {
                        filePath = resultMessage.Tag.ToString();
                    }
                    //driveName + path;
                    int returnPath = 0;
                    if (returnPath != 1)
                    {
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;

                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                            string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }

                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                    }

                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
            }
            else
            {
                resultMessage.DefaultBehaviour();
                return resultMessage;

            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


    }
}
