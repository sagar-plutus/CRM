using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using System.Linq;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseBookingOpngBalBL : ITblPurchaseBookingOpngBalBL
    {
        private readonly ITblPurchaseBookingOpngBalDAO _iTblPurchaseBookingOpngBalDAO;
        private readonly ITblPurchaseEnquiryQtyConsumptionBL _iTblPurchaseEnquiryQtyConsumptionBL;
        private readonly IConnectionString _iConnectionString;

        private readonly ITblPurchaseEnquiryBL _iTblPurchaseEnquiryBL;
        private readonly Icommondao _iCommonDAO;
        public TblPurchaseBookingOpngBalBL(ITblPurchaseEnquiryBL iTblPurchaseEnquiryBL, ITblPurchaseBookingOpngBalDAO iTblPurchaseBookingOpngBalDAO, ITblPurchaseEnquiryQtyConsumptionBL iTblPurchaseEnquiryQtyConsumptionBL, Icommondao iCommonDAO, IConnectionString iConnectionString)
        {
            _iTblPurchaseBookingOpngBalDAO = iTblPurchaseBookingOpngBalDAO;
            _iTblPurchaseEnquiryQtyConsumptionBL = iTblPurchaseEnquiryQtyConsumptionBL;
            _iConnectionString = iConnectionString;
            _iTblPurchaseEnquiryBL = iTblPurchaseEnquiryBL;
            _iCommonDAO = iCommonDAO;
        }
        #region Selection
        public List<TblPurchaseBookingOpngBalTO> SelectAllTblPurchaseBookingOpngBal()
        {
            return _iTblPurchaseBookingOpngBalDAO.SelectAllTblPurchaseBookingOpngBal();
        }

        public List<TblPurchaseBookingOpngBalTO> SelectAllTblPurchaseBookingOpngBalList()
        {
            return _iTblPurchaseBookingOpngBalDAO.SelectAllTblPurchaseBookingOpngBal();
        }

        public TblPurchaseBookingOpngBalTO SelectTblPurchaseBookingOpngBalTO(Int32 idPurchaseBookingOpngBal)
        {
            return _iTblPurchaseBookingOpngBalDAO.SelectTblPurchaseBookingOpngBal(idPurchaseBookingOpngBal);
        }

        #endregion

        #region Insertion


        public int calculateOpeningClosingBal()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            DateTime serverDateTime = _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Dictionary<Int32, Double> bookingDCT = _iTblPurchaseBookingOpngBalDAO.SelectBookingsPendingQtyDCT(conn, tran);
                if (bookingDCT != null && bookingDCT.Count > 0)
                {
                    int result = UpdateTblPurchaseBookingOpngBal(conn, tran);
                    if (result >= 0)
                    {
                        DateTime serverDate = serverDateTime;
                        foreach (var bookingId in bookingDCT.Keys)
                        {
                            TblPurchaseBookingOpngBalTO tblBookingOpngBalTO = new TblPurchaseBookingOpngBalTO();
                            tblBookingOpngBalTO.EnquiryId = bookingId;
                            tblBookingOpngBalTO.OpeningBalQty = bookingDCT[bookingId];
                            tblBookingOpngBalTO.BalAsOnDate = serverDate;
                            tblBookingOpngBalTO.IsActive = 1;

                            result = InsertTblPurchaseBookingOpngBal(tblBookingOpngBalTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While InsertTblBookingOpngBal For BookingID : " + bookingId;
                                resultMessage.Result = 0;
                                return 0;
                            }
                        }
                    }
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Opening Balance Calculated Successfully";
                resultMessage.Result = 1;
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method CalculateBookingOpeningBalance";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int UpdateTblPurchaseBookingOpngBal(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseBookingOpngBalDAO.UpdateTblPurchaseBookingOpngBal(conn, tran);
        }

        public int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO)
        {
            return _iTblPurchaseBookingOpngBalDAO.InsertTblPurchaseBookingOpngBal(tblPurchaseBookingOpngBalTO);
        }

        public int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseBookingOpngBalDAO.InsertTblPurchaseBookingOpngBal(tblPurchaseBookingOpngBalTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO)
        {
            return _iTblPurchaseBookingOpngBalDAO.UpdateTblPurchaseBookingOpngBal(tblPurchaseBookingOpngBalTO);
        }


        public int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseBookingOpngBalDAO.UpdateTblPurchaseBookingOpngBal(tblPurchaseBookingOpngBalTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseBookingOpngBal(Int32 idPurchaseBookingOpngBal)
        {
            return _iTblPurchaseBookingOpngBalDAO.DeleteTblPurchaseBookingOpngBal(idPurchaseBookingOpngBal);
        }

        public int DeleteTblPurchaseBookingOpngBal(Int32 idPurchaseBookingOpngBal, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseBookingOpngBalDAO.DeleteTblPurchaseBookingOpngBal(idPurchaseBookingOpngBal, conn, tran);
        }

        #endregion

        public List<SaudaReportTo> GetAllPendingSaudaForReport(int cnfOrgId, int dealerOrgId, int materialTypeId, int statusId)
        {

            try
            {
                List<SaudaReportTo> list = new List<SaudaReportTo>();
                DateTime serverDate = _iCommonDAO.ServerDateTime;
                DateTime serverDateForDaysElapsed = serverDate;

                List<TblPurchaseEnquiryTO> openingBalBookingList = _iTblPurchaseEnquiryBL.SelectAllTodaysBookingsWithOpeningBalance(cnfOrgId, dealerOrgId, serverDate);
                List<TblPurchaseEnquiryTO> todaysList = _iTblPurchaseEnquiryBL.SelectAllPendingBookingsList(cnfOrgId, dealerOrgId, serverDate.Date, "=", false);
                if (todaysList != null && todaysList.Count > 0)
                {

                }

                List<TblPurchaseBookingOpngBalTO> openingBalQtyList = _iTblPurchaseBookingOpngBalDAO.SelectAllTblPurchaseBookingOpngBal();
                List<TblPurchaseEnquiryQtyConsumptionTO> bookingConsuList = _iTblPurchaseEnquiryQtyConsumptionBL.SelectAllTblPurchaseEnquiryQtyConsumption(serverDate);
               
               // Get last proccessed datetime 
                TblPurchaseBookingOpngBalTO tblOpngTO = _iTblPurchaseBookingOpngBalDAO.SelectTblPurchaseBookingOpngBalLatest();

                if(tblOpngTO != null){
                    serverDate = tblOpngTO.BalAsOnDate;
                }

                Dictionary<int, Double> todayRejectedUnLoadingQtyDCT = _iTblPurchaseBookingOpngBalDAO.SelectBookingWiseUnloadingQtyDCT(serverDate, true);
                Dictionary<int, Double> todaysUnLoadingQtyDCT = _iTblPurchaseBookingOpngBalDAO.SelectBookingWiseUnloadingQtyDCT(serverDate, false);

                List<TblPurchaseEnquiryTO> finalList = new List<TblPurchaseEnquiryTO>();
                if (openingBalBookingList != null)
                    finalList.AddRange(openingBalBookingList);
                if (todaysList != null)
                    finalList.AddRange(todaysList);

                if (finalList != null && finalList.Count > 0)
                {

                    if (dealerOrgId > 0)
                    {
                        finalList = finalList.Where(w => w.SupplierId == dealerOrgId).ToList();
                    }
                    if (statusId > 0)
                    {
                        finalList = finalList.Where(w => w.StatusId == statusId).ToList();
                    }
                    if (materialTypeId > 0)
                    {
                        finalList = finalList.Where(w => w.ProdClassId == materialTypeId).ToList();
                    }

                    List<Int32> enquiryIdList = new List<int>();

                    var list1 = finalList.GroupBy(a => a.IdPurchaseEnquiry).ToList().Select(a => a.Key).ToList();
                    enquiryIdList.AddRange(list1);

                    if (todayRejectedUnLoadingQtyDCT != null && todayRejectedUnLoadingQtyDCT.Count > 0)
                    {
                        var list2 = todayRejectedUnLoadingQtyDCT.ToList().Select(a => a.Key).ToList();
                        enquiryIdList.AddRange(list2);
                    }

                    if (todaysUnLoadingQtyDCT != null && todaysUnLoadingQtyDCT.Count > 0)
                    {
                        var list3 = todaysUnLoadingQtyDCT.ToList().Select(a => a.Key).ToList();
                        enquiryIdList.AddRange(list3);
                    }

                    var distinctBookings = enquiryIdList.Distinct().ToList();
                    foreach (var enquiryId in distinctBookings)
                    {

                        SaudaReportTo saudaReportTO = new SaudaReportTo();

                        var bookingTO = finalList.Where(a => a.IdPurchaseEnquiry == enquiryId).FirstOrDefault();
                        if (bookingTO == null)
                            bookingTO = _iTblPurchaseEnquiryBL.SelectTblPurchaseEnquiry(enquiryId);

                        if (bookingTO == null)
                        {
                            continue;
                        }

                        saudaReportTO.EnquiryId = enquiryId;
                        saudaReportTO.EnquiryNo = bookingTO.EnqDisplayNo;
                        if (saudaReportTO.EnquiryId == 244 || saudaReportTO.EnquiryId == 150)
                        {
                            saudaReportTO.EnquiryNo = bookingTO.EnqDisplayNo;

                        }
                        saudaReportTO.PurchaseManager = bookingTO.PurchaseManagerName;
                        saudaReportTO.CnfOrgId = bookingTO.CnFOrgId;
                        saudaReportTO.PartyName = bookingTO.SupplierName;
                        saudaReportTO.StatusName = bookingTO.StatusName;
                        saudaReportTO.MaterialType = bookingTO.ProdClassDesc;
                        saudaReportTO.DealerOrgId = bookingTO.SupplierId;
                        saudaReportTO.OrgSaudaQty = bookingTO.BookingQty;

                        saudaReportTO.DeliveryDays = bookingTO.DeliveryDays;

                        //if(bookingTO.EnqDisplayNo == "2020/1006")
                        //{

                        //}

                        //Prajakta[2020-04-08] Commented and added on current server date
                        //saudaReportTO.DaysElapsed = (int)serverDate.Subtract(bookingTO.CreatedOn).TotalDays;
                        saudaReportTO.DaysElapsed = (int)serverDateForDaysElapsed.Subtract(bookingTO.CreatedOn).TotalDays;
                        Int32 cnfId = saudaReportTO.CnfOrgId;
                        Int32 dealerId = saudaReportTO.DealerOrgId;
                        Int32 statusIdBooking = bookingTO.StatusId;
                        Int32 ProdClassId = bookingTO.ProdClassId;

                        // if (userDealerList != null && userDealerList.Count > 0)
                        // {
                        //     // If User has area alloacated then check for allocated Cnf and Area
                        //     var isAllowedTO = userDealerList.Where(u => u.DealerOrgId == dealerId && u.CnfOrgId == cnfOrgId).FirstOrDefault();
                        //     if (isAllowedTO == null)
                        //         continue;
                        // }

                        if (cnfOrgId > 0)
                        {
                            if (cnfOrgId != cnfId)
                                continue;
                        }

                        if (dealerOrgId > 0)
                        {
                            if (dealerOrgId != dealerId)
                                continue;
                        }
                        if (statusId > 0)
                        {
                            if (statusId != statusIdBooking)
                                continue;
                        }
                        if (materialTypeId > 0)
                        {
                            if (materialTypeId != ProdClassId)
                                continue;
                        }

                        var openingQtyMT = openingBalQtyList.Where(b => b.EnquiryId == enquiryId).Sum(x => x.OpeningBalQty);

                        saudaReportTO.OpeningSaudaQty = openingQtyMT;
                        saudaReportTO.Rate = bookingTO.BookingRate;
                        //saudaReportTO.SaudaDate = bookingTO.CreatedOn.ToShortDateString();
                        saudaReportTO.SaudaDate = bookingTO.SaudaCreatedOn;

                        var todaysDelQty = bookingConsuList.Where(d => d.PurchaseEnqId == enquiryId).Sum(s => s.ConsumptionQty);
                        saudaReportTO.ClosingSaudaQty = todaysDelQty;

                        Double todaysLoadingQty = 0;
                        if (todaysUnLoadingQtyDCT != null && todaysUnLoadingQtyDCT.ContainsKey(enquiryId))
                        {
                            todaysLoadingQty = todaysUnLoadingQtyDCT[enquiryId];
                        }
                        Double todaysDelLoadingQty = 0;
                        if (todayRejectedUnLoadingQtyDCT != null && todayRejectedUnLoadingQtyDCT.ContainsKey(enquiryId))
                        {
                            todaysDelLoadingQty = todayRejectedUnLoadingQtyDCT[enquiryId];
                        }

                        Double todaysFinalLoadingQty = todaysLoadingQty - todaysDelLoadingQty;
                        saudaReportTO.TodaysUnloadingQty = todaysFinalLoadingQty;

                        Double todaysBookQtyMT = 0;
                        todaysBookQtyMT = bookingTO.BookingQty;
                        if (bookingTO.CreatedOn.Day == serverDate.Day && bookingTO.CreatedOn.Month == serverDate.Month && bookingTO.CreatedOn.Year == serverDate.Year)
                        {
                            todaysBookQtyMT = bookingTO.BookingQty;
                            // saudaReportTO.TodaysUnloadingQty = todaysBookQtyMT - bookingTO.PendingBookingQty;
                            todaysLoadingQty = todaysBookQtyMT - bookingTO.PendingBookingQty;
                        }

                        Double closingBal = 0;

                        if (openingQtyMT == 0)
                            closingBal = todaysBookQtyMT - (todaysLoadingQty - todaysDelLoadingQty + todaysDelQty);
                        else
                            closingBal = openingQtyMT - (todaysLoadingQty - todaysDelLoadingQty + todaysDelQty);

                        saudaReportTO.ClosingSaudaQty = closingBal;

                        list.Add(saudaReportTO);
                    }
                }

                // list = list.OrderBy(a => a.PurchaseManager).ThenByDescending(b => b.NoOfDayElapsed).ToList();
                list = list.OrderBy(a => a.DaysElapsed).ToList();


                SaudaReportTo saudaReportTOTotal = new SaudaReportTo();
                if (list != null && list.Count > 0)
                {
                    saudaReportTOTotal.TodaysUnloadingQty = 0;
                    saudaReportTOTotal.OpeningSaudaQty = 0;
                    saudaReportTOTotal.ClosingSaudaQty = 0;
                    saudaReportTOTotal.OrgSaudaQty = 0;
                    saudaReportTOTotal.PurchaseManager = "Total";
                    foreach (var saudaReportTO in list)
                    {

                        DateTime dt = Convert.ToDateTime(saudaReportTO.SaudaDate);
                        //saudaReportTO.SaudaEndDate = (dt.AddDays(saudaReportTO.DeliveryDays)).ToShortDateString();
                        saudaReportTO.SaudaEndDate = (dt.AddDays(saudaReportTO.DeliveryDays));
                        if (saudaReportTO.OrgSaudaQty > 0)
                        {
                            saudaReportTOTotal.OrgSaudaQty += saudaReportTO.OrgSaudaQty;
                        }
                        double totalQty = saudaReportTO.TodaysUnloadingQty;
                        if (totalQty != null)
                        {
                            saudaReportTOTotal.TodayConfirmedUnloadQty = totalQty;
                        }
                        double totalScheduledQty = saudaReportTO.TodaysUnloadingQty;
                        if (totalScheduledQty != null)
                        {
                            // saudaReportTOTotal.TodaysUnloadingQty = totalScheduledQty;
                            saudaReportTOTotal.TodaysUnloadingQty += totalScheduledQty;
                        }
                        double closingQty = saudaReportTO.ClosingSaudaQty;
                        if (closingQty != null)
                        {
                            // saudaReportTO.ClosingSaudaQty = closingQty;
                            saudaReportTOTotal.ClosingSaudaQty += closingQty;
                        }
                        // double openingQty = _ireportDAO.GetOpeningQty(saudaReportTO.EnquiryId);
                        double openingQty = saudaReportTO.OpeningSaudaQty;

                        if (openingQty != null && openingQty > 0)
                        {
                            // saudaReportTO.OpeningSaudaQty = openingQty;
                            saudaReportTOTotal.OpeningSaudaQty += openingQty;
                        }

                    }

                    list.Add(saudaReportTOTotal);

                }

                //Prajakta [2020-29-07] dont show closing bal in Minus(-);
                if (list != null && list.Count > 0)
                {
                    var tempList = list.Where(w => w.ClosingSaudaQty < 0).ToList();
                    if (tempList != null && tempList.Count > 0)
                    {
                        tempList.ForEach(f => f.ClosingSaudaQty = 0);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
