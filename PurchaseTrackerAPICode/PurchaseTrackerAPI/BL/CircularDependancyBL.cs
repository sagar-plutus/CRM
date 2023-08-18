using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using PurchaseTrackerAPI.StaticStuff;

using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;

namespace PurchaseTrackerAPI.BL
{
    public class CircularDependancyBL : ICircularDependancyBL
    {
        private readonly ITblPurchaseScheduleSummaryDAO _iTblPurchaseScheduleSummaryDAO;
        private readonly ITblPurchaseInvoiceDAO _iTblPurchaseInvoiceDAO;
        //private readonly ITblPurchaseInvoiceBL _iTblPurchaseInvoiceBL;
        private readonly ITblPurchaseUnloadingDtlDAO _iTblPurchaseUnloadingDtlDAO;
        private readonly ITblPurchaseVehicleDetailsBL _iTblPurchaseVehicleDetailsBL;
        private readonly ITblPurchaseEnquiryDAO _iTblPurchaseEnquiryDAO;
       // private readonly Icommondao _iCommonDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly ITblRateBandDeclarationPurchaseDAO _iTblRateBandDeclarationPurchaseDAO;
        //private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        //private readonly Icommondao _iCommonDAO;
        private readonly Icommondao _icommondao;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblPurchaseInvoiceAddrDAO _iTblPurchaseInvoiceAddrDAO;
        private readonly ITblPurchaseInvoiceItemDetailsBL _iTblPurchaseInvoiceItemDetailsBL;
        private readonly ITblPurchaseInvoiceItemTaxDetailsBL _iTblPurchaseInvoiceItemTaxDetailsBL;
        private readonly ITblPurchaseInvoiceInterfacingDtlBL _iTblPurchaseInvoiceInterfacingDtlBL;
        private readonly ITblPurchaseInvoiceDocumentsBL _iTblPurchaseInvoiceDocumentsBL;
        private readonly IRunReport _iRunReport;

        private readonly ITblpurchaseEnqShipmemtDtlsBL _iTblpurchaseEnqShipmemtDtlsBL;
        private readonly ITblpurchaseEnqShipmemtDtlsExtBL _iITblpurchaseEnqShipmemtDtlsExtBL;
        public CircularDependancyBL(ITblPurchaseScheduleSummaryDAO iTblPurchaseScheduleSummaryDAO, ITblPurchaseEnquiryDAO iTblPurchaseEnquiryDAO, ITblPurchaseInvoiceDAO iTblPurchaseInvoiceDAO, ITblPurchaseUnloadingDtlDAO iTblPurchaseUnloadingDtlDAO,
        ITblPurchaseVehicleDetailsBL iTblPurchaseVehicleDetailsBL,
        ITblRateBandDeclarationPurchaseDAO iTblRateBandDeclarationPurchaseDAO,
        ITblConfigParamsBL iTblConfigParamsBL,
        Icommondao iCommonDAO, IDimReportTemplateBL iDimReportTemplateBL, IRunReport iRunReport,
        ITblPurchaseInvoiceAddrDAO iTblPurchaseInvoiceAddrDAO,
            ITblPurchaseInvoiceItemDetailsBL iTblPurchaseInvoiceItemDetailsBL,
            ITblPurchaseInvoiceItemTaxDetailsBL iTblPurchaseInvoiceItemTaxDetailsBL,
            ITblPurchaseInvoiceInterfacingDtlBL iTblPurchaseInvoiceInterfacingDtlBL,
            ITblPurchaseInvoiceDocumentsBL iTblPurchaseInvoiceDocumentsBL,
            Icommondao icommondao, IConnectionString iConnectionString, ITblpurchaseEnqShipmemtDtlsBL iTblpurchaseEnqShipmemtDtlsBL,
            ITblpurchaseEnqShipmemtDtlsExtBL iITblpurchaseEnqShipmemtDtlsExtBL
            )
        {
            _iTblpurchaseEnqShipmemtDtlsBL = iTblpurchaseEnqShipmemtDtlsBL;
            _iITblpurchaseEnqShipmemtDtlsExtBL = iITblpurchaseEnqShipmemtDtlsExtBL;
            _iTblPurchaseScheduleSummaryDAO = iTblPurchaseScheduleSummaryDAO;
            _iTblPurchaseUnloadingDtlDAO = iTblPurchaseUnloadingDtlDAO;
            _iTblPurchaseInvoiceDAO = iTblPurchaseInvoiceDAO;
            _iTblPurchaseEnquiryDAO = iTblPurchaseEnquiryDAO;
            _iTblPurchaseVehicleDetailsBL = iTblPurchaseVehicleDetailsBL;
            _iTblRateBandDeclarationPurchaseDAO = iTblRateBandDeclarationPurchaseDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
           // _iCommonDAO = iCommonDAO;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iRunReport = iRunReport;
           // _iTblPurchaseInvoiceBL = iTblPurchaseInvoiceBL;
            _icommondao = icommondao;
            _iConnectionString = iConnectionString;
            _iTblPurchaseInvoiceAddrDAO = iTblPurchaseInvoiceAddrDAO;
            _iTblPurchaseInvoiceItemDetailsBL = iTblPurchaseInvoiceItemDetailsBL;
            _iTblPurchaseInvoiceItemTaxDetailsBL = iTblPurchaseInvoiceItemTaxDetailsBL;
            _iTblPurchaseInvoiceInterfacingDtlBL = iTblPurchaseInvoiceInterfacingDtlBL;
            _iTblPurchaseInvoiceDocumentsBL = iTblPurchaseInvoiceDocumentsBL;

        }
        public List<TblPurchaseScheduleSummaryTO> GetPurchaseScheduleSummaryTOByVehicleNo(String vehicleNo, Int32 actualRootScheduleId)
        {
            return _iTblPurchaseScheduleSummaryDAO.SelectPurchaseScheduleSummaryTOByVehicleNo(vehicleNo, actualRootScheduleId);
        }
        public TblPurchaseEnquiryTO SelectTblBookingsTO(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblBookings(idBooking, conn, tran);
        }

        public List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId)
        {
            return _iTblPurchaseInvoiceDAO.SelectAllTblPurchaseInvoiceListAgainstSchedule(rootPurchaseSchId);
        }
        public List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.SelectAllTblPurchaseInvoiceListAgainstSchedule(rootPurchaseSchId, conn, tran);
        }
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId, Int32 isGradingBeforeUnld = 0)
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(purchaseWeighingStageId, isGradingBeforeUnld);

        }

        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(purchaseWeighingStageId, conn, tran);

        }
        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootScheduleID(Int32 RootScheduleId, Boolean isActive)
        {
            return _iTblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummaryTOByRootID(RootScheduleId, isActive);
        }

        //------
        public TblPurchaseScheduleSummaryTO GetVehicleDetailsByScheduleIds(Int32 IdPurchaseScheduleSummary, Int32 statusId, Int32 vehiclePhaseId, Int32 rootScheduleId)
        {

            //TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryReturnTO = new TblPurchaseScheduleSummaryTO();

            Boolean isGetGradeExpDtls = false;
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = SelectTblPurchaseScheduleSummaryDtlsTO(IdPurchaseScheduleSummary, rootScheduleId);
            if (tblPurchaseScheduleSummaryTO != null)
            {
                if (vehiclePhaseId > 0)
                {
                    if (tblPurchaseScheduleSummaryTO.StatusId == Convert.ToInt32(statusId) && tblPurchaseScheduleSummaryTO.VehiclePhaseId == Convert.ToInt32(vehiclePhaseId))
                    {
                        List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                        tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, isGetGradeExpDtls);
                        if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                        {
                            tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                        }
                        return tblPurchaseScheduleSummaryTO;
                    }
                    else if (tblPurchaseScheduleSummaryTO.ParentPurchaseScheduleSummaryId > 0)
                    {
                        return GetVehicleDetailsByScheduleIds(tblPurchaseScheduleSummaryTO.ParentPurchaseScheduleSummaryId, statusId, vehiclePhaseId, 0);
                    }
                    else
                        return null;
                }
                else
                {
                    if (tblPurchaseScheduleSummaryTO.StatusId == Convert.ToInt32(statusId))
                    {
                        List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                        tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, isGetGradeExpDtls);
                        if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                        {
                            tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                        }
                        return tblPurchaseScheduleSummaryTO;
                    }
                    else if (tblPurchaseScheduleSummaryTO.ParentPurchaseScheduleSummaryId > 0)
                    {
                        return GetVehicleDetailsByScheduleIds(tblPurchaseScheduleSummaryTO.ParentPurchaseScheduleSummaryId, statusId, vehiclePhaseId, 0);
                    }
                    else
                        return null;
                }

            }
            return null;
        }

        public TblPurchaseScheduleSummaryTO SelectTblPurchaseScheduleSummaryDtlsTO(Int32 isPurchaseScheduleSummary, Int32 rootScheduleId)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryDAO.SelectTblPurchaseScheduleSummaryDetailsList(isPurchaseScheduleSummary, rootScheduleId);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count == 1)
            {
                return tblPurchaseScheduleSummaryTOList[0];
            }
            else
                return null;

        }
        public int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleSummaryDAO.UpdateTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO, conn, tran);
        }

        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 parentScheduleId)
        {
            return _iTblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummaryTO(parentScheduleId);
        }

        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 idPurchaseScheduleSummary, Boolean isActive, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummaryTO(idPurchaseScheduleSummary, isActive, conn, tran);
        }
        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByRootScheduleID(Int32 RootScheduleId, Boolean isActive, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummaryTOByRootID(RootScheduleId, isActive, conn, tran);
        }

        public TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByParentScheduleId(Int32 parentScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummaryTOByParentScheduleId(parentScheduleId, conn, tran);
        }

        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblPurchaseEnquiryTO(purchaseEnquiryId);

        }

        public TblPurchaseEnquiryTO SelectTblBookingsForPurchaseTO(Int32 idPurchaseEnquiry, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblBookingsForPurchase(idPurchaseEnquiry, conn, tran);

        }
        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseEnquiryDAO.SelectTblPurchaseEnquiryTO(purchaseEnquiryId, conn, tran);

        }

        public List<TblPurchaseEnquiryTO> SelectSaudaListBySaudaIds(String saudaIds)
        {
            return _iTblPurchaseEnquiryDAO.SelectSaudaListBySaudaIds(saudaIds);
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
                            TblPurchaseScheduleSummaryTO existingScheduleTO = SelectAllEnquiryScheduleSummaryTO(scheduleTO.IdPurchaseScheduleSummary, false, conn, tran);
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

                    if (scheduleTO == null)
                    {
                        //tblPurchaseEnquiryTO.PendingBookingQty = tblPurchaseEnquiryTO.BookingQty - totalScheduleQty;
                        tblPurchaseEnquiryTO.RateForC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, true, conn, tran);
                        tblPurchaseEnquiryTO.RateForNC = tblPurchaseEnquiryTO.BookingRate + _iTblPurchaseEnquiryDAO.SelectParityForCAndNC(tblPurchaseEnquiryTO.StateId, false, conn, tran);
                    }

                    tblPurchaseEnquiryTO.IsEnqTransfered = 0;
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


        public int UpdateTblBookingsForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.UpdateTblBookingsForPurchase(tblPurchaseEnquiryTO, conn, tran);
        }
        public int UpdateIsGradingWhileUnloadingFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleSummaryDAO.UpdateIsGradingWhileUnloadingFlag(tblPurchaseScheduleSummaryTO, conn, tran);
        }
        public int InsertTblPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.InsertTblPurchaseEnquiry(tblPurchaseEnquiryTO, conn, tran);
        }

        public Int32 SelectMaxEnquiryNo(Int32 finYear, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.SelectMaxEnquiryNo(finYear, conn, tran);
        }

        public List<TblRateBandDeclarationPurchaseTO> SelectAllTblRateBandDeclarationPurchase(Int32 globalRatePurchaseId)
        {
            List<TblRateBandDeclarationPurchaseTO> tblRateBandDeclarationPurchaseTOList = _iTblRateBandDeclarationPurchaseDAO.SelectAllTblRateBandDeclarationPurchase(globalRatePurchaseId);
            return tblRateBandDeclarationPurchaseTOList;
        }


        //public List<TblRateBandDeclarationPurchaseTO> SelectLatestRateBandDeclarationPurchaseTOList(Int32 cnfId, DateTime date)
        //{
        //    return _iTblRateBandDeclarationPurchaseDAO.SelectLatestRateBandDeclarationPurchaseTOList(cnfId, date);
        //}

        //public List<TblRateBandDeclarationPurchaseTO> GetRateDeclartionDtlsWhileBooking(Int32 cnfId)
        //{
        //    DateTime date = new DateTime();
        //    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_SCRAP_DAILY_RATE_DECLARATION_FOR_ENQUIRY);
        //    if (tblConfigParamsTO != null && tblConfigParamsTO.ConfigParamVal.ToString() == "1")
        //    {
        //        date = _iCommonDAO.ServerDateTime;
        //        return _iTblRateBandDeclarationPurchaseDAO.SelectLatestRateBandDeclarationPurchaseTOList(cnfId, date);
        //    }
        //    else
        //    {
        //        return _iTblRateBandDeclarationPurchaseDAO.SelectLatestRateBandDeclarationPurchaseTOList(cnfId, date);
        //    }

        //}
        public List<TblRateBandDeclarationPurchaseTO> GetRateDeclartionDtlsWhileBooking(Int32 cnfId)
        {
            DateTime date = new DateTime();
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_SCRAP_DAILY_RATE_DECLARATION_FOR_ENQUIRY);
            if (tblConfigParamsTO != null && tblConfigParamsTO.ConfigParamVal.ToString() == "1")
            {
                date = _icommondao.ServerDateTime;
                return _iTblRateBandDeclarationPurchaseDAO.SelectLatestRateBandDeclarationPurchaseTOList(cnfId, date);
            }
            else
            {
                return _iTblRateBandDeclarationPurchaseDAO.SelectLatestRateBandDeclarationPurchaseTOList(cnfId, date);
            }

        }

        public List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOReturnList = new List<TblPurchaseEnquiryTO>();
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = _iTblPurchaseEnquiryDAO.GetSupplierWiseSaudaDetails(supplierId, statusId, conn, tran);
            if (tblPurchaseEnquiryTOList != null && tblPurchaseEnquiryTOList.Count > 0)
            {
                if (tblPurchaseEnquiryTOList != null && tblPurchaseEnquiryTOList.Count > 0)
                {
                    for (int i = 0; i < tblPurchaseEnquiryTOList.Count; i++)
                    {
                        TblPurchaseEnquiryTO tblPurchaseEnquiryTO = new TblPurchaseEnquiryTO();
                        if (tblPurchaseEnquiryTOList[i].BookingQty == 0 || tblPurchaseEnquiryTOList[i].PendingBookingQty > 0)
                        {
                            tblPurchaseEnquiryTO = tblPurchaseEnquiryTOList[i];
                            tblPurchaseEnquiryTO.TblpurchaseEnqShipmemtDtlsExtTOList = GetShipmentDetailsByPurchaseEnquiryIdForReport(tblPurchaseEnquiryTO.IdPurchaseEnquiry);
                            tblPurchaseEnquiryTOReturnList.Add(tblPurchaseEnquiryTO);
                        }

                    }
                }
            }
            return tblPurchaseEnquiryTOReturnList;

        }
        public int UpdateMaterialTypeOfSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.UpdateMaterialTypeOfSauda(tblPurchaseEnquiryTO, conn, tran);

        }

        public ResultMessage PrintNCVehicalReport(List<TallyReportTO> tallyReportList)
        {
            ResultMessage resultMessage = new ResultMessage();
            DataSet printReportDS = new DataSet();
            DataTable printReportDT = new DataTable();
            printReportDT.Columns.Add("Date");
            printReportDT.Columns.Add("Truckno");
            printReportDT.Columns.Add("SupplierName");
            printReportDT.Columns.Add("PM");
            printReportDT.Columns.Add("Location");
            printReportDT.Columns.Add("Grade");
            printReportDT.Columns.Add("GradeQty", typeof(double));
            printReportDT.Columns.Add("GradeRate", typeof(double));
            printReportDT.Columns.Add("Total", typeof(double));
            printReportDT.Columns.Add("BillType");
            printReportDT.Columns.Add("MaterialType");
            printReportDT.Columns.Add("ContainerNo");

            for (int i = 0; i < tallyReportList.Count; i++)
            {
                TallyReportTO tallyReportTO = tallyReportList[i];
                printReportDT.Rows.Add();
                int rowNo = printReportDT.Rows.Count - 1;
                printReportDT.Rows[rowNo]["Date"] = tallyReportTO.Date;
                printReportDT.Rows[rowNo]["Truckno"] = tallyReportTO.TruckNo;
                printReportDT.Rows[rowNo]["SupplierName"] = tallyReportTO.SupplierName;
                printReportDT.Rows[rowNo]["PM"] = tallyReportTO.PM;
                printReportDT.Rows[rowNo]["Location"] = tallyReportTO.Location;
                printReportDT.Rows[rowNo]["Grade"] = tallyReportTO.Grade;
                printReportDT.Rows[rowNo]["GradeQty"] = tallyReportTO.GradeQty;
                printReportDT.Rows[rowNo]["GradeRate"] = tallyReportTO.GradeRate;
                printReportDT.Rows[rowNo]["Total"] = tallyReportTO.Total;
                printReportDT.Rows[rowNo]["BillType"] = tallyReportTO.BillType;
                printReportDT.Rows[rowNo]["MaterialType"] = tallyReportTO.MaterialType;
                printReportDT.Rows[rowNo]["ContainerNo"] = tallyReportTO.ContainerNo;
            }
            printReportDT.TableName = "printReportDT";
            printReportDS.Tables.Add(printReportDT);

            string templateName = "PrintNCVehicalReport";
            // String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
            String templateFilePath = @"C:\Deliver Templates\PrintNCVehicalReport.template.xls";
             String fileTempName = "PrintNCVehicalReport-" + DateTime.Now.Ticks;
            String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileTempName + ".xls";
            Boolean IsProduction = true;

            resultMessage = _iRunReport.GenrateMktgInvoiceReport(printReportDS, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);

            if (resultMessage.MessageType == ResultMessageE.Information)
            {
                String filePath = String.Empty;
                if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                {
                    filePath = resultMessage.Tag.ToString();
                }
                String fileName1 = Path.GetFileName(saveLocation);
                Byte[] bytes = File.ReadAllBytes(filePath);
                if (bytes != null && bytes.Length > 0)
                {
                    resultMessage.Tag = bytes;
                    string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                    string directoryName;
                    directoryName = Path.GetDirectoryName(saveLocation);
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + fileTempName + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + fileTempName + "*");

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
                    resultMessage.DefaultSuccessBehaviour();
                }
            }
            return resultMessage;
        }
        public ResultMessage CreatePOWithGRRAPI(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                var values = new JObject();
                values.Add("tblPurchaseInvoiceTO", JsonConvert.SerializeObject(tblPurchaseInvoiceTO));

                ApiData data = new ApiData();
                data.tblPurchaseInvoiceTO = tblPurchaseInvoiceTO;

                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ApiData));
                ser.WriteObject(ms, data);
                byte[] json = ms.ToArray();
                ms.Close();

                String createPOwithGrrUrl = "Commercial/CreatePurchaseInvoicePOWithGRR";

                String url = Startup.StockAPIUrl + createPOwithGrrUrl;
                object result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", _iConnectionString.GetConnectionString(Constants.STOCK_API_URL));
                request.Method = "Post";
                request.ContentType = "application/json";

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(json, 0, json.Length);
                    requestStream.Close();
                }


                //WebResponse objResponse = request.GetResponseAsync().Result;
                request.Timeout = 120000;
                WebResponse objResponse = GetWebResponseAsync(request).Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }

                resultMessage = JsonConvert.DeserializeObject<ResultMessage>(result.ToString());
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in CreatePOWithGRRAPI(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)");
                return resultMessage;
            }
        }

        public async Task<WebResponse> GetWebResponseAsync(WebRequest webRequest)
        {
            return await webRequest.GetResponseAsync();
        }
        public class ApiData
        {
            public TblPurchaseInvoiceTO tblPurchaseInvoiceTO = new TblPurchaseInvoiceTO();
        }

        public TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTOWithDetails(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                TblPurchaseInvoiceTO tblPurchaseInvoiceTO = _iTblPurchaseInvoiceDAO.SelectTblPurchaseInvoice(purchaseInvoiceId, conn, tran);
                if (tblPurchaseInvoiceTO != null)
                {
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(purchaseInvoiceId, conn, tran);
                    List<TblPurchaseInvoiceItemDetailsTO> itemList = _iTblPurchaseInvoiceItemDetailsBL.SelectAllTblPurchaseInvoiceItemDetailsList(purchaseInvoiceId, conn, tran);
                    if (itemList != null && itemList.Count > 0)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            itemList[i].TblPurchaseInvoiceItemTaxDetailsTOList = _iTblPurchaseInvoiceItemTaxDetailsBL.SelectAllTblPurchaseInvoiceItemTaxDetailsList(itemList[i].IdPurchaseInvoiceItem, conn, tran);

                        }
                        tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList = itemList;
                    }

                    tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls = _iTblPurchaseInvoiceInterfacingDtlBL.SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(purchaseInvoiceId, conn, tran);

                }

                return tblPurchaseInvoiceTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        public int UpdateEnquiryPendingBookingQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDAO.UpdateEnquiryPendingBookingQty(tblPurchaseEnquiryTO, conn, tran);
        }
        public ResultMessage CreatePurchaseInvoicePOWithGRR(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblPurchaseInvoiceTO> tblPurchaseInvoiceTOList = SelectAllTblPurchaseInvoiceListAgainstSchedule(rootScheduleId, conn, tran);
                if (tblPurchaseInvoiceTOList == null || tblPurchaseInvoiceTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Invoice details not found against purchase schedule Id = " + rootScheduleId;
                    return resultMessage;
                }

                TblPurchaseInvoiceTO tblPurchaseInvoiceTO = tblPurchaseInvoiceTOList[0];

                tblPurchaseInvoiceTO = SelectTblPurchaseInvoiceTOWithDetails(tblPurchaseInvoiceTO.IdInvoicePurchase, conn, tran);
                if (tblPurchaseInvoiceTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Invoice details not found against purchase schedule Id = " + rootScheduleId;
                    return resultMessage;
                }

                tblPurchaseInvoiceTO.UpdatedOn = _icommondao.ServerDateTime;
                resultMessage = CreatePOWithGRRAPI(tblPurchaseInvoiceTO);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }

                if (resultMessage.Tag != null)
                {
                    TblPurchaseInvoiceTO purchaseInvoiceTO = JsonConvert.DeserializeObject<TblPurchaseInvoiceTO>(resultMessage.Tag.ToString());
                    if (purchaseInvoiceTO != null)
                    {
                        Int32 result = _iTblPurchaseInvoiceDAO.UpdatePOAndGrrNoForInvoice(purchaseInvoiceTO, conn, tran);
                        if (result == -1)
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
                resultMessage.DefaultExceptionBehaviour(ex, "Error in CreatePurchaseInvoicePOWithGRR(Int32 rootScheduleId,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;
            }
        }

        //Deepali Added [24-06-2021] for task no 1151
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

    }
}
