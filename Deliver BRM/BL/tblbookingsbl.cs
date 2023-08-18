using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using Microsoft.Extensions.Logging;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblBookingsBL
    {
        #region Selection

        private static readonly object bookingNoLock = new object();

        public static List<TblBookingsTO> SelectAllTblBookingsList()
        {
            return TblBookingsDAO.SelectAllTblBookings();

        }

        public static List<TblBookingsTO> SelectAllBookingsListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingsDAO.SelectAllBookingsListFromLoadingSlipId(loadingSlipId, conn, tran);
        }
        /// <summary>
        /// Priyanka [28-03-2019]
        /// </summary>
        /// <param name="bookingDisplayNo"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static List<TblBookingsTO> SelectAllBookingsListFromBookingDisplayNo(Int32 BookingRefId, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingsDAO.SelectAllBookingsListFromBookingDisplayNo(BookingRefId, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-02-23] Wil  return list of all booking out of quota and rate band
        /// and booking status = New
        /// </summary>
        /// <returns></returns>
        public static List<TblBookingsTO> SelectAllBookingsListForApproval()
        {
            return TblBookingsDAO.SelectAllBookingsListForApproval();

        }

        /// <summary>
        /// Sanjay [2017-02-27] Will return list of all bookings of given cnf which are beyond quota and Rate Band
        /// and Approved By Directors. This will be for Acceptance By Cnf
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        public static List<TblBookingsTO> SelectAllBookingsListForAcceptance(Int32 cnfId, List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblBookingsDAO.SelectAllBookingsListForAcceptance(cnfId, tblUserRoleTO);

        }

        public static List<TblBookingsTO> SelectAllLatestBookingOfDealer(Int32 dealerId, Int32 lastNRecords)
        {
            List<TblBookingsTO> pendingList = TblBookingsDAO.SelectAllLatestBookingOfDealer(dealerId, lastNRecords, true);
            if (pendingList != null && pendingList.Count < lastNRecords)
            {
                lastNRecords = lastNRecords - pendingList.Count;
                List<TblBookingsTO> list = TblBookingsDAO.SelectAllLatestBookingOfDealer(dealerId, lastNRecords, false);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var isExist = pendingList.Where(e => e.IdBooking == list[i].IdBooking).FirstOrDefault();
                        if (isExist == null)
                            pendingList.Add(list[i]);

                    }
                }
            }

            return pendingList;
        }

        /// <summary>
        /// Sanjay [2017-02-23] This will return list of booking to show for loading
        /// Only Apporved and having Pending Qty > 0 bookings will be returned.
        /// </summary>
        /// <param name="cnfId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public static List<TblBookingsTO> SelectAllBookingList(Int32 cnfId, Int32 dealerId, List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblUserRoleTO userRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                userRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblBookingsDAO.SelectAllBookingList(cnfId, dealerId, userRoleTO);

        }

        public static List<TblBookingsTO> SelectBookingList(Int32 cnfId, Int32 dealerId, string statusId, DateTime fromDate, DateTime toDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 BookingId,Int32 bookingTypeId,Int32 skipDateFilter)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblBookingsDAO.SelectBookingList(cnfId, dealerId, statusId, fromDate, toDate, tblUserRoleTO, BookingId, bookingTypeId, skipDateFilter);

        }

        /// <summary>
        /// Priyanka [21-03-2018]
        /// </summary>
        /// <param name="idBooking"></param>
        /// <returns></returns>
        public static List<TblBookingSummaryTO> SelectBookingSummaryList(Int32 typeId, Int32 masterId, DateTime fromDate, DateTime toDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            List<TblBookingSummaryTO> tblBookingSummaryTOList = TblBookingsDAO.SelectBookingSummaryList(typeId, masterId, fromDate, toDate, tblUserRoleTO, cnfId);
            if (tblBookingSummaryTOList != null && tblBookingSummaryTOList.Count > 0)
            {
                string IdBookingStr = string.Join(",", tblBookingSummaryTOList.Select(n => n.IdBooking.ToString()).ToArray());
                List<TblBookingQtyConsumptionTO> bookingConsuList = DAL.TblBookingQtyConsumptionDAO.SelectAllTblBookingQtyConsumption(IdBookingStr);
                if(bookingConsuList != null && bookingConsuList.Count > 0)
                {
                    foreach (var element in tblBookingSummaryTOList)
                    {
                        element.BookingDelQty = bookingConsuList.Where(d => d.BookingId == element.IdBooking).Sum(s => s.ConsumptionQty);
                    }

                    //    for (int i = 0; i < tblBookingSummaryTOList.Count; i++)
                    //{
                    //    tblBookingSummaryTOList[i].BookingDelQty = bookingConsuList.Where(d => d.BookingId == tblBookingSummaryTOList[i].IdBooking).Sum(s => s.ConsumptionQty);
                    //}
                }
            }
            return tblBookingSummaryTOList;
        }







        public static TblBookingsTO SelectTblBookingsTO(Int32 idBooking)
        {
            return TblBookingsDAO.SelectTblBookings(idBooking);

        }

        /// <summary>
        /// Sanjay [2017-03-03] To Get the Details of Given Booking with child details
        /// </summary>
        /// <param name="idBooking"></param>
        /// <returns></returns>
        public static TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking)
        {
            try
            {
                TblBookingsTO tblBookingsTO = TblBookingsDAO.SelectTblBookings(idBooking);
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.IS_SHOW_BOOKING_PAGE_SIZE_AND_ADDRESS_DTLS);
                if (tblConfigParamsTO != null && tblConfigParamsTO.ConfigParamVal == "1")
                {
                    tblBookingsTO.DeliveryAddressLst = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrList(idBooking);
                    tblBookingsTO.OrderDetailsLst = BL.TblBookingExtBL.SelectAllTblBookingExtList(idBooking);
                }
                else
                {
                    //Prajakta[2020-04-24] Added to get schedule list
                    List<TblBookingScheduleTO> tblBookingScheduleTOList = TblBookingScheduleDAO.SelectAllTblBookingScheduleList(idBooking);

                    if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                    {

                        tblBookingScheduleTOList = tblBookingScheduleTOList.Where(a => a.Qty > 0).ToList();

                        if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                        {
                            for (int i = 0; i < tblBookingScheduleTOList.Count; i++)
                            {

                                TblBookingScheduleTO tblBookingScheduleTO = tblBookingScheduleTOList[i];
                                List<TblBookingExtTO> tblBookingExtTOLst = TblBookingExtDAO.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);

                                tblBookingScheduleTO.OrderDetailsLst = tblBookingExtTOLst;
                                List<TblBookingDelAddrTO> tblBookingDelAddrTOLst = TblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);
                                if(tblBookingDelAddrTOLst!=null && tblBookingDelAddrTOLst.Count>0)
                                {
                                    for (int j = 0; j < tblBookingDelAddrTOLst.Count; j++)
                                    {
                                        tblBookingDelAddrTOLst[j].LoadingLayerId = tblBookingScheduleTO.LoadingLayerId;
                                    }
                                }

                                tblBookingScheduleTO.DeliveryAddressLst = tblBookingDelAddrTOLst;
                            }
                        }

                    }
                    tblBookingsTO.BookingScheduleTOLst = tblBookingScheduleTOList;
                }

               
                return tblBookingsTO;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                TblBookingsTO tblBookingsTO = TblBookingsDAO.SelectTblBookings(idBooking, conn, tran);
                tblBookingsTO.DeliveryAddressLst = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrList(idBooking, conn, tran);
                tblBookingsTO.OrderDetailsLst = BL.TblBookingExtBL.SelectAllTblBookingExtList(idBooking, conn, tran);
                return tblBookingsTO;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static SalesTrackerAPI.DashboardModels.BookingInfo SelectBookingDashboardInfo(List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId,Int32 dealerId,  DateTime date)
        {
            try
            {
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                // Boolean isPriorityOther = true;
                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
                 
                }
                return TblBookingsDAO.SelectBookingDashboardInfo(tblUserRoleTO, orgId, dealerId, date);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static TblBookingsTO SelectTblBookingsTO(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingsDAO.SelectTblBookings(idBooking, conn, tran);

        }

        public static List<PendingBookingRptTO> SelectAllPendingBookingsForReport(Int32 cnfId, Int32 dealerOrgId, List<TblUserRoleTO> tblUserRoleTOList,Int32 StateId,Int32 DistId)
        {
            //BL.TblBookingOpngBalBL.CalculateBookingOpeningBalance();
            try
            {
               TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);

                }

                int isConfEn = 0;
                int userId = 0;
                if (tblUserRoleTO != null)
                {
                    isConfEn = tblUserRoleTO.EnableAreaAlloc;
                    userId = tblUserRoleTO.UserId;
                }

                List<UserAreaCnfDealerDtlTO> userDealerList = null;
                if (userId > 0 && isConfEn == 1)
                    userDealerList = BL.TblUserAreaAllocationBL.SelectAllUserAreaCnfDealerList(userId);

                List<PendingBookingRptTO> list = new List<PendingBookingRptTO>();
                DateTime serverDate = Constants.ServerDateTime;

                List<TblBookingsTO> openingBalBookingList = DAL.TblBookingsDAO.SelectAllTodaysBookingsWithOpeningBalance(cnfId, dealerOrgId, serverDate, StateId, DistId);
                List<TblBookingsTO> todaysList = DAL.TblBookingsDAO.SelectAllPendingBookingsList(cnfId, dealerOrgId, serverDate.Date, "=", false, StateId,DistId);
                //Saket [2019-05-24] Added to ignore tentative newly spltted records.
                if (todaysList != null && todaysList.Count > 0)
                {
                    //todaysList = todaysList.Where(w => w.IsConfirmed != 0 && w.BookingRefId == 0).ToList();
                    todaysList = todaysList.Where(w => w.BookingRefId == 0 || w.IsConfirmed == 1).ToList();
                    //if(todaysList != null && todaysList.Count > 0)
                    //    todaysList = todaysList.Where(w => w.BookingTypeId != (int)Constants.BookingTypeE.Bulk).ToList();
                }
                //if (openingBalBookingList != null && openingBalBookingList.Count > 0)//Reshma Added show only normnal booking in report
                //    openingBalBookingList = openingBalBookingList.Where(w => w.BookingTypeId != (int)Constants.BookingTypeE.Bulk).ToList();

                List<TblBookingOpngBalTO> openingBalQtyList = DAL.TblBookingOpngBalDAO.SelectAllTblBookingOpngBal(serverDate);
                List<TblBookingQtyConsumptionTO> bookingConsuList = DAL.TblBookingQtyConsumptionDAO.SelectAllTblBookingQtyConsumption(serverDate);
                Dictionary<int, Double> todayDeletedLoadingQtyDCT = DAL.TblLoadingSlipDtlDAO.SelectBookingWiseLoadingQtyDCT(serverDate, true);
                Dictionary<int, Double> todaysLoadingQtyDCT = DAL.TblLoadingSlipDtlDAO.SelectBookingWiseLoadingQtyDCT(serverDate, false);

                List<TblBookingsTO> finalList = new List<TblBookingsTO>();
                if (openingBalBookingList != null)
                    finalList.AddRange(openingBalBookingList);
                if (todaysList != null)
                    finalList.AddRange(todaysList);

                if (finalList != null && finalList.Count > 0)
                {

                    if (dealerOrgId > 0)
                    {
                        finalList = finalList.Where(w => w.DealerOrgId == dealerOrgId).ToList();
                    }

                    List<Int32> bookingIdList = new List<int>();

                    var list1 = finalList.GroupBy(a => a.IdBooking).ToList().Select(a => a.Key).ToList();
                    bookingIdList.AddRange(list1);

                    if (todayDeletedLoadingQtyDCT != null && todayDeletedLoadingQtyDCT.Count > 0)
                    {
                        var list2 = todayDeletedLoadingQtyDCT.ToList().Select(a => a.Key).ToList();
                        bookingIdList.AddRange(list2);
                    }

                    if (todaysLoadingQtyDCT != null && todaysLoadingQtyDCT.Count > 0)
                    {
                        var list3 = todaysLoadingQtyDCT.ToList().Select(a => a.Key).ToList();
                        bookingIdList.AddRange(list3);
                    }

                    var distinctBookings = bookingIdList.Distinct().ToList();
                    foreach (var bookingId in distinctBookings)
                    {

                        PendingBookingRptTO pendingBookingRptTO = new PendingBookingRptTO();

                        var bookingTO = finalList.Where(a => a.IdBooking == bookingId).FirstOrDefault();
                        if (bookingTO == null)
                            bookingTO = SelectTblBookingsTO(bookingId);
                  
                        if (bookingTO == null)
                        {
                            continue;
                        }
                        pendingBookingRptTO.BookingId = bookingId;
                        pendingBookingRptTO.BookingDisplayNo = bookingTO.BookingDisplayNo;
                        pendingBookingRptTO.CnfName = bookingTO.CnfName;
                        pendingBookingRptTO.CnfOrgId = bookingTO.CnFOrgId;
                        pendingBookingRptTO.DealerName = bookingTO.DealerName;
                        pendingBookingRptTO.DealerVillageName = bookingTO.DealerVillageName;
                        pendingBookingRptTO.DealerTalukaName = bookingTO.DealerTalukaName;
                        pendingBookingRptTO.DealerDistrictName = bookingTO.DealerDistrictName;
                        pendingBookingRptTO.DealerStateName = bookingTO.DealerStateName;
                        pendingBookingRptTO.DealerOrgId = bookingTO.DealerOrgId;
                        pendingBookingRptTO.BookingTypeId = bookingTO.BookingTypeId;
                        pendingBookingRptTO.NoOfDayElapsed = (int)serverDate.Subtract(bookingTO.CreatedOn).TotalDays;
                        Int32 cnfOrgId = pendingBookingRptTO.CnfOrgId;
                        Int32 dealerId = pendingBookingRptTO.DealerOrgId;

                        if (userDealerList != null && userDealerList.Count > 0)
                        {
                            // If User has area alloacated then check for allocated Cnf and Area
                            var isAllowedTO = userDealerList.Where(u => u.DealerOrgId == dealerId && u.CnfOrgId == cnfOrgId).FirstOrDefault();
                            if (isAllowedTO == null)
                                continue;
                        }

                        if (cnfId > 0)
                        {
                            if (cnfOrgId != cnfId)
                                continue;
                        }

                        if (dealerOrgId > 0)
                        {
                            if (dealerOrgId != dealerId)
                                continue;
                        }

                        var openingQtyMT = openingBalQtyList.Where(b => b.BookingId == bookingId).Sum(x => x.OpeningBalQty);

                        pendingBookingRptTO.OpeningBalanceMT = openingQtyMT;
                        pendingBookingRptTO.BookingRate = bookingTO.BookingRate;
                        pendingBookingRptTO.BookingDate = bookingTO.CreatedOn;
                        pendingBookingRptTO.PendingQty = bookingTO.PendingQty;
                        Double todaysBookQtyMT = 0;
                        if (bookingTO.CreatedOn.Day == serverDate.Day && bookingTO.CreatedOn.Month == serverDate.Month && bookingTO.CreatedOn.Year == serverDate.Year)
                        {
                            todaysBookQtyMT = bookingTO.BookingQty;
                            pendingBookingRptTO.TodaysBookingMT = todaysBookQtyMT;
                        }

                        var todaysDelQty = bookingConsuList.Where(d => d.BookingId == bookingId).Sum(s => s.ConsumptionQty);
                        pendingBookingRptTO.TodaysDelBookingQty = todaysDelQty;

                        Double todaysLoadingQty = 0;
                        if (todaysLoadingQtyDCT != null && todaysLoadingQtyDCT.ContainsKey(bookingId))
                        {
                            todaysLoadingQty = todaysLoadingQtyDCT[bookingId];
                        }
                        Double todaysDelLoadingQty = 0;
                        if (todayDeletedLoadingQtyDCT != null && todayDeletedLoadingQtyDCT.ContainsKey(bookingId))
                        {
                            todaysDelLoadingQty = todayDeletedLoadingQtyDCT[bookingId];
                        }

                        Double todaysFinalLoadingQty = todaysLoadingQty - todaysDelLoadingQty;
                        pendingBookingRptTO.TodaysLoadingQtyMT = todaysFinalLoadingQty;
                        
                        Double closingBal = 0;

                        if (openingQtyMT == 0)
                            closingBal = todaysBookQtyMT - (todaysLoadingQty - todaysDelLoadingQty + todaysDelQty);
                        else
                            closingBal = openingQtyMT - (todaysLoadingQty - todaysDelLoadingQty + todaysDelQty);

                        pendingBookingRptTO.ClosingBalance = closingBal;

                        list.Add(pendingBookingRptTO);
                    }
                }

                list = list.OrderBy(a => a.CnfName).ThenByDescending(b => b.NoOfDayElapsed).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Vijaymala [2017-09-11] added to get booking list to generate booking graph
        /// </summary>
        /// <returns></returns>
        public static List<BookingGraphRptTO> SelectBookingListForGraph(Int32 OrganizationId, List<TblUserRoleTO> userRoleTOList, Int32 dealerId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

            if (userRoleTOList != null && userRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(userRoleTOList);
            }
            return TblBookingsDAO.SelectBookingListForGraph(OrganizationId, tblUserRoleTO, dealerId);

        }

        public static List<TblBookingsTO> GetBulkBookingHistoryBookingId(Int32 bookingId)
        {
            List<TblBookingsTO> tblBookingsTOList = new List<TblBookingsTO>();
            tblBookingsTOList = TblBookingsDAO.GetBulkBookingHistoryBookingId(bookingId);
            return tblBookingsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblBookings(TblBookingsTO tblBookingsTO)
        {
            return TblBookingsDAO.InsertTblBookings(tblBookingsTO);
        }

        public static int InsertTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingsDAO.InsertTblBookings(tblBookingsTO, conn, tran);
        }

        public static ResultMessage SaveNewBooking(TblBookingsTO tblBookingsTO, SqlConnection conn=null, SqlTransaction tran=null)
        {
            if (conn == null && tran == null)
            {
                 conn = new SqlConnection(Startup.ConnectionString);
                 tran = null;
            }
            int result = 0;
            DateTime sysDate=Constants.ServerDateTime;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                }

                //Harshala [2020-03-11]
                lock (bookingNoLock)
                {

                    #region entity range 
                    DimFinYearTO curFinYearTO = DimensionBL.GetCurrentFinancialYear(tblBookingsTO.CreatedOn,conn,tran);
                    if (curFinYearTO == null)
                    {
                        resultMessage.Text = "Current Fin Year Object Not Found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                    TblEntityRangeTO entityRangeTO = new TblEntityRangeTO();
                    //if(tblBookingsTO.BookingTypeId ==(int)Constants.BookingTypeE.Normal)
                        entityRangeTO=BL.TblEntityRangeBL.SelectTblEntityRangeTOByEntityName(Constants.REGULAR_BOOKING, curFinYearTO.IdFinYear, conn, tran);
                    //else
                        //entityRangeTO=BL.TblEntityRangeBL.SelectTblEntityRangeTOByEntityName(Constants.BULK_BOOKING, curFinYearTO.IdFinYear, conn, tran);

                    if (entityRangeTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "entity range not found in Function SaveNewBooking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                    entityRangeTO.EntityPrevValue = entityRangeTO.EntityPrevValue + 1;
                    result = BL.TblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : While UpdateTblEntityRange";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                    tblBookingsTO.BookingDisplayNo = entityRangeTO.EntityPrevValue.ToString();
                    if (tblBookingsTO.BookingTypeId == (int)Constants.BookingTypeE.Bulk)
                        tblBookingsTO.BookingDisplayNo = "B" + tblBookingsTO.BookingDisplayNo;
                    #endregion

                    #region 0. Check Bookings are Open Or Closed. If Closed Then Do Not Save the request

                    TblBookingActionsTO bookingStatusTO = BL.TblBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                    if (bookingStatusTO == null || bookingStatusTO.BookingStatus == "CLOSE")
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Sorry..Record Could not be saved. Bookings are closed";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Bookings are closed";
                        return resultMessage;
                    }

                    #endregion

                    #region 0.1 Check for Parity Chart. Assign Active ParityId For Future Reference
                    //Sudhir[23-MARCH-2018] Commented for as per new parity logic no parity need to check or save at the time of Booking.
                    /*
                    TblParitySummaryTO activeParityTO = BL.TblParitySummaryBL.SelectActiveTblParitySummaryTO(tblBookingsTO.DealerOrgId, conn, tran);
                    if (activeParityTO != null)
                    {
                        tblBookingsTO.ParityId = activeParityTO.IdParity;
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                        if (addrTO == null)
                        {
                            resultMessage.Text = "Sorry..Record Could not be saved. Address Is Not Defined For The Dealer And Hence Parity Settings For Not found";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Address Is Not Defined For The Dealer And Hence Parity Settings Not found";
                        }
                        else
                        {
                            resultMessage.Text = "Sorry..Record Could not be saved. Parity Settings Not defined for the state " + addrTO.StateName;
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Parity Settings Not defined for the state " + addrTO.StateName;
                        }
                        return resultMessage;
                    }
                    */
                    #endregion

                    #region 1. Save Booking Request First

                    //Priyanka [10-04-2019]
                    //tblBookingsTO.IsWithinQuotaLimit = 1;

                    TblUserTO tblUserTO = new TblUserTO();
                    tblUserTO = BL.TblUserBL.SelectTblUserTO(tblBookingsTO.CreatedBy, conn, tran);
                    if (tblUserTO != null)
                    {
                        if (tblUserTO.OrgTypeId == Convert.ToInt32(Constants.OrgTypeE.DEALER))
                        {
                            tblBookingsTO.StatusId = Convert.ToInt32(Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL);
                            tblBookingsTO.AuthReasons = "Enquiry From Dealer";
                        }
                        else
                        {
                            //tblBookingsTO.StatusId = Convert.ToInt32(Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL);
                            Boolean isFromUpdateQuotaAndSetStatus = false;
                            resultMessage = UpdateQuotaAndSetStatus(tblBookingsTO, conn, tran, tblBookingsTO.CreatedOn, resultMessage, ref isFromUpdateQuotaAndSetStatus);
                            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                resultMessage.Text = "Error While Update Quota & Set Status";
                                return resultMessage;
                            }
                        }
                    }

                    if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                    {
                        //Prajakta[2020-04-24] added as per disscussion with saket
                        var res = tblBookingsTO.BookingScheduleTOLst.GroupBy(x => x.ScheduleGroupId);
                        if (res != null)
                            tblBookingsTO.NoOfDeliveries = res.Count();
                    }

                    tblBookingsTO.PendingQty = tblBookingsTO.BookingQty;
                    result = InsertTblBookings(tblBookingsTO, conn, tran);
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

                    #region 2. If Booking Beyond Quota Then Maintain History Of Approvals

                    if (tblBookingsTO.IsWithinQuotaLimit == 0)
                    {
                        TblBookingBeyondQuotaTO bookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                        bookingBeyondQuotaTO.BookingId = tblBookingsTO.IdBooking;
                        bookingBeyondQuotaTO.CreatedBy = tblBookingsTO.CreatedBy;
                        bookingBeyondQuotaTO.CreatedOn = tblBookingsTO.CreatedOn;
                        bookingBeyondQuotaTO.DeliveryPeriod = tblBookingsTO.DeliveryDays;
                        bookingBeyondQuotaTO.Quantity = tblBookingsTO.BookingQty;
                        bookingBeyondQuotaTO.Rate = tblBookingsTO.BookingRate;
                        bookingBeyondQuotaTO.CdStructureId = tblBookingsTO.CdStructureId;
                        bookingBeyondQuotaTO.OrcAmt = tblBookingsTO.OrcAmt;
                        bookingBeyondQuotaTO.Remark = tblBookingsTO.AuthReasons;
                        bookingBeyondQuotaTO.Rate = tblBookingsTO.BookingRate;
                        bookingBeyondQuotaTO.StatusDate = tblBookingsTO.CreatedOn;
                        bookingBeyondQuotaTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;

                        //Priyanka [09-04-2019]
                        TblUserTO tblUserTONew = BL.TblUserBL.SelectTblUserTO(tblBookingsTO.CreatedBy, conn, tran);
                        if (tblUserTONew != null)
                        {
                            if (tblUserTONew.OrgTypeId == Convert.ToInt32(Constants.OrgTypeE.DEALER))
                            {
                                bookingBeyondQuotaTO.StatusId = Convert.ToInt32(Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL);
                            }
                        }

                        result = BL.TblBookingBeyondQuotaBL.InsertTblBookingBeyondQuota(bookingBeyondQuotaTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingBeyondQuota in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }

                    #endregion

                    //Prajakta[2020-04-24] Commented and added size details with respect to schedule
                    //#region 3. Save Materialwise Qty and Rate
                    //if (tblBookingsTO.OrderDetailsLst != null && tblBookingsTO.OrderDetailsLst.Count > 0)
                    //{
                    //    for (int i = 0; i < tblBookingsTO.OrderDetailsLst.Count; i++)
                    //    {
                    //        tblBookingsTO.OrderDetailsLst[i].BookingId = tblBookingsTO.IdBooking;
                    //        tblBookingsTO.OrderDetailsLst[i].Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                    //        result = BL.TblBookingExtBL.InsertTblBookingExt(tblBookingsTO.OrderDetailsLst[i], conn, tran);
                    //        if (result != 1)
                    //        {
                    //            tran.Rollback();
                    //            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingExt in Function SaveNewBooking";
                    //            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                    //            resultMessage.Result = 0;
                    //            resultMessage.MessageType = ResultMessageE.Error;
                    //            return resultMessage;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //Sanjay [2017-02-23] These details are not compulsory while entry
                    //    //tran.Rollback();
                    //    //resultMessage.Text = "OrderDetailsLst Not Found  While SaveNewBooking";
                    //    //resultMessage.MessageType = ResultMessageE.Error;
                    //    //return resultMessage;
                    //}
                    //#endregion

                    //Prajakta[2020-04-24] Commented and added address details with respect to schedule
                    //#region 4. Save Order Delivery Addresses
                    //if (tblBookingsTO.DeliveryAddressLst != null && tblBookingsTO.DeliveryAddressLst.Count > 0)
                    //{
                    //    for (int i = 0; i < tblBookingsTO.DeliveryAddressLst.Count; i++)
                    //    {
                    //        if (string.IsNullOrEmpty(tblBookingsTO.DeliveryAddressLst[i].Country))
                    //            tblBookingsTO.DeliveryAddressLst[i].Country = Constants.DefaultCountry;

                    //        tblBookingsTO.DeliveryAddressLst[i].BookingId = tblBookingsTO.IdBooking;
                    //        result = BL.TblBookingDelAddrBL.InsertTblBookingDelAddr(tblBookingsTO.DeliveryAddressLst[i], conn, tran);
                    //        if (result != 1)
                    //        {
                    //            tran.Rollback();
                    //            resultMessage.Text = "Error While Inserting Booking Del Address in Function SaveNewBooking";
                    //            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                    //            resultMessage.MessageType = ResultMessageE.Error;
                    //            return resultMessage;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //Sanjay [2017-03-02] These details are not compulsory while entry
                    //    //tran.Rollback();
                    //    //resultMessage.Text = "Delivery Address Not Found - Function SaveNewBooking";
                    //    //resultMessage.MessageType = ResultMessageE.Error;
                    //    //return resultMessage;
                    //}
                    //#endregion


                    #region 3.Save project schedule 
                    if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                    {
                        for (int i = 0; i < tblBookingsTO.BookingScheduleTOLst.Count; i++)
                        {
                            TblBookingScheduleTO tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[i];
                            tblBookingScheduleTO.BookingId = tblBookingsTO.IdBooking;
                            tblBookingScheduleTO.CreatedBy = tblBookingsTO.CreatedBy;
                            tblBookingScheduleTO.CreatedOn = tblBookingsTO.CreatedOn;

                            result = TblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Record Could not be saved.";
                                resultMessage.DisplayMessage = "Record Could not be saved.";
                                resultMessage.Result = 0;
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }



                            #region 3.1. Save Materialwise Qty and Rate
                            if (tblBookingScheduleTO.OrderDetailsLst != null && tblBookingScheduleTO.OrderDetailsLst.Count > 0)
                            {
                                for (int j = 0; j < tblBookingScheduleTO.OrderDetailsLst.Count; j++)
                                {
                                    TblBookingExtTO tblBookingExtTO = tblBookingScheduleTO.OrderDetailsLst[j];
                                    tblBookingExtTO.BookingId = tblBookingsTO.IdBooking;
                                    tblBookingExtTO.Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                                    tblBookingExtTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                    //tblBookingExtTO.BrandId = dimBrandTO.IdBrand;

                                    result = TblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.Text = "Record Could not be saved.";
                                        resultMessage.DisplayMessage = "Record Could not be saved.";
                                        resultMessage.Result = 0;
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }
                                }
                            }

                            #endregion
                            #region 3.2. Save Order Delivery Addresses

                            if (tblBookingScheduleTO.DeliveryAddressLst != null && tblBookingScheduleTO.DeliveryAddressLst.Count > 0)
                            {
                                for (int k = 0; k < tblBookingScheduleTO.DeliveryAddressLst.Count; k++)
                                {
                                    TblBookingDelAddrTO tblBookingDelAddrTO = tblBookingScheduleTO.DeliveryAddressLst[k];
                                    if (string.IsNullOrEmpty(tblBookingDelAddrTO.Country))
                                        tblBookingDelAddrTO.Country = Constants.DefaultCountry;

                                    tblBookingDelAddrTO.BookingId = tblBookingsTO.IdBooking;
                                    tblBookingDelAddrTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                    result = TblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.Text = "Record Could not be saved.";
                                        resultMessage.DisplayMessage = "Record Could not be saved.";
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }
                                }
                            }
                            else
                            {
                                //Sanjay [2017-03-02] These details are not compulsory while entry
                                //tran.Rollback();
                                //resultMessage.Text = "Delivery Address Not Found - Function SaveNewBooking";
                                //resultMessage.MessageType = ResultMessageE.Error;
                                //return resultMessage;
                            }
                            #endregion
                        }

                    }
                    #endregion

                    //#region 5. Update Quota for Balance Qty

                    //existingQuotaTO.BalanceQty = existingQuotaTO.BalanceQty - tblBookingsTO.BookingQty;
                    //existingQuotaTO.UpdatedBy = tblBookingsTO.CreatedBy;
                    //existingQuotaTO.UpdatedOn = Constants.ServerDateTime;

                    //result = TblQuotaDeclarationBL.UpdateBalanceQuotaQty(existingQuotaTO, conn, tran);
                    //if (result != 1)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.Text = "Error While Updating Quota UpdateTblQuotaDeclaration in Function SaveNewBooking";
                    //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    return resultMessage;
                    //}

                    //#endregion

                    #region 6. Manage Quota Consumption History

                    TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO = new TblQuotaConsumHistoryTO();
                    tblQuotaConsumHistoryTO.AvailableQuota = tblBookingsTO.QuotaQtyBforBooking;
                    tblQuotaConsumHistoryTO.BalanceQuota = tblBookingsTO.QuotaQtyAftBooking;
                    tblQuotaConsumHistoryTO.BookingId = tblBookingsTO.IdBooking;
                    tblQuotaConsumHistoryTO.CreatedBy = tblBookingsTO.CreatedBy;
                    tblQuotaConsumHistoryTO.CreatedOn = tblBookingsTO.CreatedOn;
                    tblQuotaConsumHistoryTO.QuotaDeclarationId = tblBookingsTO.QuotaDeclarationId;
                    tblQuotaConsumHistoryTO.QuotaQty = -tblBookingsTO.BookingQty;
                    tblQuotaConsumHistoryTO.Remark = "New Booking for Dealer :" + tblBookingsTO.DealerName;
                    tblQuotaConsumHistoryTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;

                    result = BL.TblQuotaConsumHistoryBL.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Quota InsertTblQuotaConsumHistory in Function SaveNewBooking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    #endregion

                    #region Notifications & SMS

                    //Saket [2018-05-04] get total rate instead booking rate
                    Double totalBookingRate = GetBookingTotalRate(tblBookingsTO, conn, tran);

                    // if booking withing quota then send notification to dealer confirming order detail
                    // else send notification for approval of booking

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    ResultMessage rMessage = new ResultMessage();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblBookingsTO.CnFOrgId, conn, tran);
                    if (cnfUserList != null && cnfUserList.Count > 0)
                    {
                        for (int a = 0; a < cnfUserList.Count; a++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                            tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }

                    //Priyanka [18-04-2019] : added
                    if (tblUserTO.OrgTypeId == Convert.ToInt32(Constants.OrgTypeE.DEALER))
                    {

                    }
                    else
                    {
                        if (tblBookingsTO.IsWithinQuotaLimit == 1)
                        {
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_CONFIRMED;
                            tblAlertInstanceTO.AlertAction = "BOOKING_CONFIRMED";

                            //Saket [2018-05-04] Added total rate instead of booking rate
                            //tblAlertInstanceTO.AlertComment = "Your Booking #" + tblBookingsTO.IdBooking + " is confirmed. Rate : " + tblBookingsTO.BookingRate + " AND Qty : " + tblBookingsTO.BookingQty;
                            // tblAlertInstanceTO.AlertComment = "Your Booking #" + tblBookingsTO.IdBooking + " is confirmed. Rate : " + totalBookingRate + " AND Qty : " + tblBookingsTO.BookingQty;
                            //Added By kiran for change idbooking to booking display number
                            tblAlertInstanceTO.AlertComment = "Your Booking #" + tblBookingsTO.BookingDisplayNo + " is confirmed. Rate : " + totalBookingRate + " AND Qty : " + tblBookingsTO.BookingQty;

                            tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                            // SMS to Dealer
                            Dictionary<Int32, String> orgMobileNoDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                            if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                            {
                                tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                                smsTO.SourceTxnDesc = "New Booking";
                                string confirmMsg = string.Empty;
                                if (tblBookingsTO.IsConfirmed == 1)
                                    confirmMsg = "Confirmed";
                                else
                                    confirmMsg = "Not Confirmed";

                                //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                                //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                                //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs / MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking;

                                //Saket [2018-05-04] Added total rate instead of booking rate
                                //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                                //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + totalBookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                                //added by kiran for idbooking to bokking display number
                                smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + totalBookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + "";

                                tblAlertInstanceTO.SmsTOList.Add(smsTO);
                            }

                        }
                        else
                        {
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                            tblAlertInstanceTO.AlertAction = "BOOKING_APPROVAL_REQUIRED";
                            //tblAlertInstanceTO.AlertComment = "Approval Required For Booking #" + tblBookingsTO.IdBooking;
                            //Added by kiran for id booking to booking display number
                            tblAlertInstanceTO.AlertComment = "Approval Required For Booking #" + tblBookingsTO.BookingDisplayNo;
                        }


                        tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.CreatedOn;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceDisplayId = "New Booking";
                        tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                        tblAlertInstanceTO.RaisedBy = tblBookingsTO.CreatedBy;
                        tblAlertInstanceTO.RaisedOn = tblBookingsTO.CreatedOn;
                        tblAlertInstanceTO.IsAutoReset = 1;
                        rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        if (rMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Text = "Error While Generating Notification";

                            return resultMessage;
                        }
                    }
                    //Priyana [18-04-2019] Added to send alert to respective cnf 
                    if (tblBookingsTO.StatusId == (Int32)Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_PENDING_FOR_CNF_APPROVAL;
                        tblAlertInstanceTO.AlertAction = "BOOKING_PENDING_FOR_CNF_APPROVAL";
                        //tblAlertInstanceTO.AlertComment = "Cnf Approval Required For Booking #" + tblBookingsTO.IdBooking;
                        //Added by kiran for change idbooking to bookingDisaply number
                        tblAlertInstanceTO.AlertComment = "Cnf Approval Required For Booking #" + tblBookingsTO.BookingDisplayNo;
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.CreatedOn;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceDisplayId = "New Booking";
                        tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                        tblAlertInstanceTO.RaisedBy = tblBookingsTO.CreatedBy;
                        tblAlertInstanceTO.RaisedOn = tblBookingsTO.CreatedOn;
                        tblAlertInstanceTO.IsAutoReset = 1;
                        rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        if (rMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Text = "Error While Generating Notification";

                            return resultMessage;
                        }
                    }



                    #endregion
                    if(tblBookingsTO.IsCreateNewBooking !=1)
                        tran.Commit();
                }
                resultMessage.MessageType = ResultMessageE.Information;
                if (tblBookingsTO.IsWithinQuotaLimit == 1)
                {
                    resultMessage.Text = "Success, Booking # - " + tblBookingsTO.BookingDisplayNo + " is generated.";
                    resultMessage.DisplayMessage = "Success, Booking # - " + tblBookingsTO.BookingDisplayNo + " is generated.";
                }
                else
                {
                    resultMessage.Text = "Success, Booking # - " + tblBookingsTO.BookingDisplayNo + " is generated But Sent For Approval";
                    resultMessage.DisplayMessage = "Success, Booking # - " + tblBookingsTO.BookingDisplayNo + " is generated But Sent For Approval";
                }

                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewBooking";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                if (tblBookingsTO.IsCreateNewBooking != 1)
                    conn.Close();
            }
        }

        private static ResultMessage UpdateQuotaAndSetStatus(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran, DateTime sysDate, ResultMessage resultMessage,ref Boolean  isFromUpdateQuotaAndSetStatus)
        {
            int result = 0;
           
            TblQuotaDeclarationTO existingQuotaTO = TblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(tblBookingsTO.QuotaDeclarationId, conn, tran);

            if (existingQuotaTO == null)
            {
                tran.Rollback();
                resultMessage.Text = "Sorry..Record Could not be saved. Quota Not Found";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                return resultMessage;
            }
            //Aniket [8-3-2019] added to verify booking should placed against current date and current booking rate
            int isAllowBooking = 0;
            List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = TblQuotaDeclarationBL.SelectLatestQuotaDeclarationTOList(tblBookingsTO.CnFOrgId, sysDate);
            for (int i = 0; i < tblQuotaDeclarationTOList.Count; i++)
            {
                if (tblQuotaDeclarationTOList[i].IdQuotaDeclaration == tblBookingsTO.QuotaDeclarationId)
                {
                    isAllowBooking = 1;
                    break;
                }
            }
            if (isAllowBooking == 0)
            {
                tran.Rollback();
                resultMessage.Text = "Sorry..Booking can not continue, Please refresh your page and try again";
                resultMessage.DisplayMessage = "Sorry..Booking can not continue, Please refresh your page and try again";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                return resultMessage;
            }
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MAX_ALLOWED_DEL_PERIOD, conn, tran);
            Int32 maxAllowedDelPeriod = 7;

            if (tblConfigParamsTO != null)
                maxAllowedDelPeriod = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            tblBookingsTO.QuotaQtyBforBooking = Convert.ToInt32(existingQuotaTO.BalanceQty);
            tblBookingsTO.QuotaQtyAftBooking = Convert.ToInt32(tblBookingsTO.QuotaQtyBforBooking - tblBookingsTO.BookingQty);

            TblGlobalRateTO globalRateTO = TblGlobalRateBL.SelectTblGlobalRateTO(existingQuotaTO.GlobalRateId, conn, tran);
            if (globalRateTO == null)
            {
                tran.Rollback();
                resultMessage.Text = "Sorry..Record Could not be saved. Rate Declaration Not Found";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                // return resultMessage;
            }

            Double orcAmtPerTon = 0;
            if (tblBookingsTO.OrcMeasure == "Rs/MT")
            {
                orcAmtPerTon = tblBookingsTO.OrcAmt;
            }
            else orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;

            Double allowedRate = globalRateTO.Rate - existingQuotaTO.RateBand;
            Double bookingRateWithOrcAmt = tblBookingsTO.BookingRate;

            if (orcAmtPerTon > 0)
                bookingRateWithOrcAmt = tblBookingsTO.BookingRate - existingQuotaTO.RateBand - orcAmtPerTon;

            TblOrganizationTO dealerOrgTO = BL.TblOrganizationBL.SelectTblOrganizationTO(tblBookingsTO.DealerOrgId, conn, tran);
            if (dealerOrgTO == null)
            {
                tran.Rollback();
                resultMessage.Text = "Sorry..Record Could not be saved. Dealer Details not found";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MAX_ALLOWED_CD_STRUCTURE, conn, tran);
            Double maxCdStructure = 1.5;

            if (tblConfigParamsTO != null)
                maxCdStructure = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);


            if (tblBookingsTO.QuotaQtyAftBooking < 0 || (tblBookingsTO.BookingRate < allowedRate)
                || (tblBookingsTO.DeliveryDays > maxAllowedDelPeriod)
                // || (bookingRateWithOrcAmt < globalRateTO.Rate) // Sanjay [2017-06-30] Not required as per discussion in meeting 29/6/17. Nitin K Sir and BRM Team. It will directly added into booking Rate for final Rate Calculation
                )
            {
                tblBookingsTO.AuthReasons = "";
                tblBookingsTO.IsWithinQuotaLimit = 0;
                tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;
                isFromUpdateQuotaAndSetStatus = true;
                if (tblBookingsTO.QuotaQtyAftBooking < 0)
                    tblBookingsTO.AuthReasons = "QTY|";
                if (tblBookingsTO.BookingRate < allowedRate)
                    tblBookingsTO.AuthReasons += "RATE|";
                if (tblBookingsTO.DeliveryDays > maxAllowedDelPeriod)
                    tblBookingsTO.AuthReasons += "DELIVERY|";
                //if (bookingRateWithOrcAmt < globalRateTO.Rate)  // Sanjay [2017-06-30] Not required as per discussion in meeting 29/6/17. Nitin K Sir and BRM Team.It will directly added into booking Rate for final Rate Calculation
                //    tblBookingsTO.AuthReasons += "ORC|";

                tblBookingsTO.AuthReasons = tblBookingsTO.AuthReasons.TrimEnd('|');
            }
            else
            {
                tblBookingsTO.IsWithinQuotaLimit = 1;
                tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_APPROVED;
            }

            if (tblBookingsTO.CdStructure > maxCdStructure)
            {
                if (tblBookingsTO.CdStructure > dealerOrgTO.CdStructure)
                {
                    tblBookingsTO.IsWithinQuotaLimit = 0;
                    tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;
                    tblBookingsTO.AuthReasons += "CD|";
                }
            }

            #region Update Quota for Balance Qty

            existingQuotaTO.BalanceQty = existingQuotaTO.BalanceQty - tblBookingsTO.BookingQty;
            existingQuotaTO.UpdatedBy = tblBookingsTO.CreatedBy;
            existingQuotaTO.UpdatedOn = Constants.ServerDateTime;

            result = TblQuotaDeclarationBL.UpdateBalanceQuotaQty(existingQuotaTO, conn, tran);
            if (result != 1)
            {
                tran.Rollback();
                resultMessage.Text = "Error While Updating Quota UpdateTblQuotaDeclaration in Function SaveNewBooking";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            #endregion
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region Updation
        public static int UpdateTblBookings(TblBookingsTO tblBookingsTO)
        {
            return TblBookingsDAO.UpdateTblBookings(tblBookingsTO);
        }

        public static int UpdateTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingsDAO.UpdateTblBookings(tblBookingsTO, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-02-17] To Update the Pending Booking qty after the Loading activity is done
        /// </summary>
        /// <param name="tblBookingsTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int UpdateBookingPendingQty(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingsDAO.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
        }

        /// <summary>
        /// Saket [2018-05-04] Added to get the total value against booking.
        /// </summary>
        /// <param name="tblBookingsTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static Double GetBookingTotalRate(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            Double totalRate = tblBookingsTO.BookingRate;

            //Vijaymala added [04-06-2018] to add ORcAmt for notification 
            Double orcAmtPerTon = 0;
            if (tblBookingsTO.OrcAmt != 0)  // Aniket added to accept Orc in negative
            {
                if (tblBookingsTO.OrcMeasure == "Rs/MT")  //Need to change
                    orcAmtPerTon = tblBookingsTO.OrcAmt;
                else
                {
                    if (tblBookingsTO.BookingQty > 0)
                        orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                }
                  
            }

            //Vijaymala[19-06-2018]commented code and get parity details acc to new rule
            // TblParitySummaryTO tblParitySummaryTO = TblParitySummaryBL.SelectTblParitySummaryTO(tblBookingsTO.ParityId, conn, tran);
            //if (tblParitySummaryTO != null)
            //{
            //    totalRate += tblParitySummaryTO.ExpenseAmt + tblParitySummaryTO.FreightAmt + tblParitySummaryTO.OtherAmt + tblParitySummaryTO.BaseValCorAmt + orcAmtPerTon;
            //}
            Int32 stateId = 0;
            TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
            if(addrTO!=null)
            {
                stateId = addrTO.StateId;
            }

            TblParityDetailsTO tblParityDetailsTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(stateId, tblBookingsTO.BookingDatetime);
            if (tblParityDetailsTO != null)
            {
                totalRate += tblParityDetailsTO.ExpenseAmt + tblParityDetailsTO.FreightAmt + tblParityDetailsTO.OtherAmt + tblParityDetailsTO.BaseValCorAmt + orcAmtPerTon;
            }

            return totalRate;
        }


        public static ResultMessage UpdateBookingConfirmations(TblBookingsTO tblBookingsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Delete All Scheduled Booking If Booting Qty is less than Scheduled Qty
                
                    //Added By Gokul - Delete Already Scheduled bookings if change in booking Qty
                    if (tblBookingsTO.IsResetScheduledQty)
                    {
                        List<TblBookingScheduleTO> tempBookingScheduleTOList = new List<TblBookingScheduleTO>();

                        List<TblBookingScheduleTO> tblBookingScheduleTOList = TblBookingScheduleBL.SelectAllTblBookingScheduleList(tblBookingsTO.IdBooking, conn, tran);
                        if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                        {

                            for (int l = 0; l < tblBookingScheduleTOList.Count; l++)
                            {
                                //Delete Ext
                                result = TblBookingExtDAO.DeleteTblBookingExtBySchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                                if (result == -1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Sorry..Record Could not be saved";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = -1;
                                    return resultMessage;
                                }

                                //Delete Address
                                result = TblBookingDelAddrDAO.DeleteTblBookingDelAddrByScheduleId(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                                if (result == -1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Sorry..Record Could not be saved";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = -1;
                                    return resultMessage;
                                }

                                //Delete Schedule
                                result = TblBookingScheduleDAO.DeleteTblBookingSchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                                if (result == -1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Sorry..Record Could not be saved";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = -1;
                                    return resultMessage;
                                }

                            }
                        }



                    }
                    
                #endregion


                #region 1. Add Record in tblBookingBeyondQuota For History

                TblBookingsTO existingTblBookingsTO = SelectTblBookingsTO(tblBookingsTO.IdBooking, conn, tran);
                //Added by Kiran for avoid multiple accept calls
                if(existingTblBookingsTO.StatusId == tblBookingsTO.StatusId)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Already updated current status.";
                    resultMessage.Tag = existingTblBookingsTO;
                    return resultMessage;
                }

                TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                tblBookingBeyondQuotaTO =tblBookingsTO.GetBookingBeyondQuotaTO();

                tblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                tblBookingBeyondQuotaTO.CreatedBy = tblBookingsTO.UpdatedBy;
                tblBookingBeyondQuotaTO.CreatedOn = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusDate = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusRemark = tblBookingsTO.StatusRemark;
                result = BL.TblBookingBeyondQuotaBL.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
                if(result!=1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingBeyondQuota";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                #endregion

                #region 2. Update Booking Information

                
                Boolean isCnfAcceptDirectly = false;
                if(tblBookingsTO.TranStatusE== Constants.TranStatusE.BOOKING_APPROVED_DIRECTORS
                    && tblBookingsTO.BookingQty==existingTblBookingsTO.BookingQty
                    && tblBookingsTO.BookingRate==existingTblBookingsTO.BookingRate
                    && tblBookingsTO.DeliveryDays==existingTblBookingsTO.DeliveryDays
                    && tblBookingsTO.CdStructureId==existingTblBookingsTO.CdStructureId
                    && tblBookingsTO.OrcAmt==existingTblBookingsTO.OrcAmt
                    )
                {
                    isCnfAcceptDirectly = true;
                }

                Double diffQty = 0;
                if (tblBookingsTO.IsDeleted == 1 || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_CANDF
                    || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTORS
                    || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_MARKETING)
                    diffQty = existingTblBookingsTO.BookingQty - 0;
                else
                    diffQty = existingTblBookingsTO.BookingQty - tblBookingsTO.BookingQty;

                Double pendBookQtyToUpdate = 0;
                Double pendQuotaQtyToUpdate = 0;
                if (diffQty != 0)
                {
                    if (diffQty < 0)
                    {
                        pendBookQtyToUpdate = Math.Abs(diffQty);
                        pendQuotaQtyToUpdate = diffQty;
                    }
                    else
                    {
                        pendBookQtyToUpdate = -diffQty;
                        pendQuotaQtyToUpdate = diffQty;
                    }
                }

                Double pendingBookingQty = existingTblBookingsTO.PendingQty;
                existingTblBookingsTO.PendingQty = existingTblBookingsTO.PendingQty + pendBookQtyToUpdate;

                if (existingTblBookingsTO.PendingQty < 0)
                    existingTblBookingsTO.PendingQty = 0;

                existingTblBookingsTO.BookingQty = tblBookingsTO.BookingQty;
                existingTblBookingsTO.BookingRate = tblBookingsTO.BookingRate;
                existingTblBookingsTO.DeliveryDays = tblBookingsTO.DeliveryDays;
                existingTblBookingsTO.CdStructureId = tblBookingsTO.CdStructureId;
                existingTblBookingsTO.CdStructure = tblBookingsTO.CdStructure;
                existingTblBookingsTO.OrcAmt = tblBookingsTO.OrcAmt;

                existingTblBookingsTO.StatusId = tblBookingsTO.StatusId;
                existingTblBookingsTO.StatusDate = tblBookingsTO.StatusDate;
                existingTblBookingsTO.UpdatedOn = tblBookingsTO.UpdatedOn;
                existingTblBookingsTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                
                result = UpdateTblBookings(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblBookings";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                //Sanjay [2017-06-06] if booking is deleted then maintain history of pending qty
                if (tblBookingsTO.IsDeleted == 1 || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_DELETE)
                {
                    TblBookingQtyConsumptionTO bookingQtyConsumptionTO = new TblBookingQtyConsumptionTO();
                    bookingQtyConsumptionTO.BookingId = existingTblBookingsTO.IdBooking;
                    bookingQtyConsumptionTO.ConsumptionQty = pendingBookingQty;
                    bookingQtyConsumptionTO.CreatedBy = tblBookingsTO.UpdatedBy;
                    bookingQtyConsumptionTO.CreatedOn = tblBookingsTO.UpdatedOn;
                    bookingQtyConsumptionTO.StatusId = (int)tblBookingsTO.TranStatusE;
                    bookingQtyConsumptionTO.Remark = "Booking Deleted";

                    result = BL.TblBookingQtyConsumptionBL.InsertTblBookingQtyConsumption(bookingQtyConsumptionTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertTblBookingQtyConsumption";
                        resultMessage.Tag = bookingQtyConsumptionTO;
                        return resultMessage;
                    }
                }

                #region 5. Update Quota for Balance Qty
                    TblQuotaDeclarationTO existingQuotaTO = TblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(tblBookingsTO.QuotaDeclarationId, conn, tran);
                if (existingQuotaTO == null)
                {
                    tran.Rollback();
                    resultMessage.Text = "Existing Quota Not Found In Function UpdateBookingConfirmations";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                //tblBookingsTO.QuotaQtyBforBooking = Convert.ToInt32(existingQuotaTO.BalanceQty);
                //tblBookingsTO.QuotaQtyAftBooking = Convert.ToInt32(tblBookingsTO.QuotaQtyBforBooking - tblBookingsTO.BookingQty);
                Double availQuota = existingQuotaTO.BalanceQty;
                existingQuotaTO.BalanceQty = existingQuotaTO.BalanceQty + pendQuotaQtyToUpdate;
                existingQuotaTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                existingQuotaTO.UpdatedOn = Constants.ServerDateTime;

                result = TblQuotaDeclarationBL.UpdateTblQuotaDeclaration(existingQuotaTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While Updating Quota UpdateTblQuotaDeclaration in Function SaveNewBooking";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                #endregion

                #region 6. Manage Quota Consumption History

                TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO = new TblQuotaConsumHistoryTO();
                tblQuotaConsumHistoryTO.AvailableQuota = availQuota;
                tblQuotaConsumHistoryTO.BalanceQuota = existingQuotaTO.BalanceQty;
                tblQuotaConsumHistoryTO.BookingId = existingTblBookingsTO.IdBooking;
                tblQuotaConsumHistoryTO.CreatedBy = tblBookingsTO.UpdatedBy;
                tblQuotaConsumHistoryTO.CreatedOn = existingQuotaTO.UpdatedOn;
                tblQuotaConsumHistoryTO.QuotaDeclarationId = existingTblBookingsTO.QuotaDeclarationId;
                tblQuotaConsumHistoryTO.QuotaQty = pendQuotaQtyToUpdate;

                if (tblBookingsTO.IsDeleted == 1)
                    tblQuotaConsumHistoryTO.Remark = "Booking Deleted";
                else if(tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_CANDF)
                    tblQuotaConsumHistoryTO.Remark = "Booking Rejected BY C&F";
                else
                    tblQuotaConsumHistoryTO.Remark = "Existing Booking Updated for Dealer :" + existingTblBookingsTO.DealerName;

                tblQuotaConsumHistoryTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.UPDATE;

                result = BL.TblQuotaConsumHistoryBL.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While Updating Quota InsertTblQuotaConsumHistory in Function SaveNewBooking";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                #endregion

                #endregion

                if(isCnfAcceptDirectly)
                {
                    tblBookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                    tblBookingBeyondQuotaTO = tblBookingsTO.GetBookingBeyondQuotaTO();
                    tblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                    tblBookingBeyondQuotaTO.TranStatusE = Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF;
                    tblBookingBeyondQuotaTO.CreatedBy = tblBookingsTO.UpdatedBy;
                    tblBookingBeyondQuotaTO.CreatedOn = tblBookingsTO.UpdatedOn;
                    tblBookingBeyondQuotaTO.StatusDate = tblBookingsTO.UpdatedOn;
                    result = BL.TblBookingBeyondQuotaBL.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertTblBookingBeyondQuota";
                        resultMessage.Tag = tblBookingBeyondQuotaTO;
                        return resultMessage;
                    }

                    existingTblBookingsTO.TranStatusE = tblBookingBeyondQuotaTO.TranStatusE;
                    existingTblBookingsTO.StatusDate = tblBookingsTO.StatusDate;
                    existingTblBookingsTO.UpdatedOn = tblBookingsTO.UpdatedOn;
                    existingTblBookingsTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                    result = UpdateTblBookings(existingTblBookingsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While UpdateTblBookings";
                        resultMessage.Tag = tblBookingBeyondQuotaTO;
                        return resultMessage;
                    }
                }


                #region Notifications & SMSs

                if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_DIRECTORS ||
                    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTORS ||
                    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_CANDF ||
                    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF)
                {
                    //Saket [2018-05-04] get total rate instead booking rate
                    Double bookingTotalAmt = GetBookingTotalRate(tblBookingsTO, conn, tran);

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblBookingsTO.CnFOrgId, conn, tran);
                    TblUserTO userTO = TblUserBL.SelectTblUserTO(tblBookingsTO.CreatedBy,conn,tran);
                    if(userTO!=null)
                    {
                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                        tblAlertUsersTO.UserId = userTO.IdUser;
                        tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                        tblAlertUsersTOList.Add(tblAlertUsersTO);
                    }

                    if (cnfUserList != null && cnfUserList.Count > 0)
                    {
                        for (int a = 0; a < cnfUserList.Count; a++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                            tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                            if (tblAlertUsersTOList != null && tblAlertUsersTOList.Count > 0)
                            {
                                var isExistTO = tblAlertUsersTOList.Where(x => x.UserId == tblAlertUsersTO.UserId).FirstOrDefault();
                                if (isExistTO == null)
                                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                            }
                            else
                                tblAlertUsersTOList.Add(tblAlertUsersTO);

                        }
                    }
                    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_DIRECTORS && isCnfAcceptDirectly)
                    {

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = tblBookingsTO.TranStatusE.ToString();
                        tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.BookingDisplayNo + " is accepted";

                        // SMS to Dealer
                        Dictionary<Int32, String> orgMobileNoDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                        {
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                            smsTO.SourceTxnDesc = "Booking Approved By Directors";
                            string confirmMsg = string.Empty;
                            if (tblBookingsTO.IsConfirmed == 1)
                                confirmMsg = "Confirmed";
                            else
                                confirmMsg = "Not Confirmed";

                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                            
                            //Saket [2018-05-04] Added total rate instead of booking rate
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + bookingTotalAmt + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + "";

                            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, 0, conn, tran);
                        if(result<0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }
                    }
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_DIRECTORS && !isCnfAcceptDirectly)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVED_BY_DIRECTORS";
                        tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.BookingDisplayNo + " is awaiting for your confirmation";
                        tblAlertInstanceTO.SourceDisplayId = "Approved By Directors With Change";
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }

                    }
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTORS)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = "BOOKING_REJECTED_BY_DIRECTORS";
                        tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.BookingDisplayNo + " is rejected";
                        tblAlertInstanceTO.SourceDisplayId = "BOOKING_REJECTED_BY_DIRECTORS";
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }

                    }
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_ACCEPTED_BY_CNF;
                        tblAlertInstanceTO.AlertAction = "BOOKING_ACCEPTED_BY_CNF";
                        tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.BookingDisplayNo + " is accepted BY CnF";

                        // SMS to Dealer
                        Dictionary<Int32, String> orgMobileNoDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                        {
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                            smsTO.SourceTxnDesc = "BOOKING_ACCEPTED_BY_CNF";
                            string confirmMsg = string.Empty;
                            if (tblBookingsTO.IsConfirmed == 1)
                                confirmMsg = "Confirmed";
                            else
                                confirmMsg = "Not Confirmed";

                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;

                            //Saket [2018-05-04] Added total rate instead of booking rate
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + bookingTotalAmt + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + "";
                            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        }

                        result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }
                    }
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_CANDF)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_CNF;
                        tblAlertInstanceTO.AlertAction = "BOOKING_REJECTED_BY_CNF";
                        tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.BookingDisplayNo + " is cancelled by CnF";
                        tblAlertInstanceTO.SourceDisplayId = "BOOKING_REJECTED_BY_CNF";

                        result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }
                    }

                    tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                    tblAlertInstanceTO.RaisedBy = tblBookingsTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblBookingsTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Generating Notification";
                        return resultMessage;
                    }
                }

                #endregion

                //Added By Gokul - Delete Already Scheduled bookings if change in booking Qty
                if (tblBookingsTO.IsResetScheduledQty)
                {
                    List<TblBookingScheduleTO> tempBookingScheduleTOList = new List<TblBookingScheduleTO>();

                    List<TblBookingScheduleTO> tblBookingScheduleTOList = TblBookingScheduleBL.SelectAllTblBookingScheduleList(tblBookingsTO.IdBooking, conn, tran);
                    if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                    {
                        
                        for (int l = 0; l < tblBookingScheduleTOList.Count; l++)
                        {
                            //Delete Ext
                            result = TblBookingExtDAO.DeleteTblBookingExtBySchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                            if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Sorry..Record Could not be saved";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                return resultMessage;
                            }

                            //Delete Address
                            result = TblBookingDelAddrDAO.DeleteTblBookingDelAddrByScheduleId(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                            if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Sorry..Record Could not be saved";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                return resultMessage;
                            }

                            //Delete Schedule
                            result = TblBookingScheduleDAO.DeleteTblBookingSchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                            if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Sorry..Record Could not be saved";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                return resultMessage;
                            }

                        }
                    }



                }


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

        public static ResultMessage UpdateBooking(TblBookingsTO tblBookingsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();


                #region 1. Update Booking Information

                TblBookingsTO existingTblBookingsTO = SelectTblBookingsTO(tblBookingsTO.IdBooking, conn, tran);

                Double diffQty = existingTblBookingsTO.BookingQty - tblBookingsTO.BookingQty;
                if (tblBookingsTO.IsCreateNewBooking == 1)
                {
                    diffQty = existingTblBookingsTO.BookingQty - tblBookingsTO.BulkBookingQty;
                    tblBookingsTO.BookingQty = tblBookingsTO.BulkBookingQty;
                    tblBookingsTO.PendingQty = tblBookingsTO.BulkBookingQty;
                }
                Double pendBookQtyToUpdate = 0;
                Double pendQuotaQtyToUpdate = 0;
                if (diffQty != 0)
                {
                    if (diffQty < 0)
                    {
                        pendBookQtyToUpdate = Math.Abs(diffQty);
                        pendQuotaQtyToUpdate = diffQty;
                    }
                    else
                    {
                        pendBookQtyToUpdate = -diffQty;
                        pendQuotaQtyToUpdate = diffQty;
                    }
                }

                existingTblBookingsTO.PendingQty = existingTblBookingsTO.PendingQty + pendBookQtyToUpdate;

                if (tblBookingsTO.IsCreateNewBooking != 1)
                {
                    existingTblBookingsTO.BookingQty = tblBookingsTO.BookingQty;
                    existingTblBookingsTO.BookingRate = tblBookingsTO.BookingRate;
                    existingTblBookingsTO.DeliveryDays = tblBookingsTO.DeliveryDays;
                    existingTblBookingsTO.StatusId = tblBookingsTO.StatusId;
                    existingTblBookingsTO.StatusDate = tblBookingsTO.StatusDate;
                    existingTblBookingsTO.UpdatedOn = tblBookingsTO.UpdatedOn;
                    existingTblBookingsTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                    existingTblBookingsTO.NoOfDeliveries = tblBookingsTO.NoOfDeliveries;
                    existingTblBookingsTO.IsConfirmed = tblBookingsTO.IsConfirmed;
                    existingTblBookingsTO.IsJointDelivery = tblBookingsTO.IsJointDelivery;
                    existingTblBookingsTO.IsSpecialRequirement = tblBookingsTO.IsSpecialRequirement;
                    existingTblBookingsTO.CdStructure = tblBookingsTO.CdStructure;
                    existingTblBookingsTO.CdStructureId = tblBookingsTO.CdStructureId;
                    existingTblBookingsTO.OrcAmt = tblBookingsTO.OrcAmt;
                    existingTblBookingsTO.OrcMeasure = tblBookingsTO.OrcMeasure;
                    existingTblBookingsTO.BillingName = tblBookingsTO.BillingName;
                    existingTblBookingsTO.Comments = tblBookingsTO.Comments;
                    existingTblBookingsTO.PoNo = tblBookingsTO.PoNo;

                    existingTblBookingsTO.SizesQty = tblBookingsTO.SizesQty;
                }
                TblUserTO tblUserTO = new TblUserTO();
                Boolean isFromUpdateQuotaAndSetStatus = false; 
                tblUserTO = BL.TblUserBL.SelectTblUserTO(tblBookingsTO.UpdatedBy, conn, tran);
                if (tblUserTO != null)
                {


                    if (tblUserTO.OrgTypeId == Convert.ToInt32(Constants.OrgTypeE.DEALER))
                    {
                        tblBookingsTO.StatusId = Convert.ToInt32(Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL);
                        tblBookingsTO.AuthReasons = "Enquiry from dealer";

                    }
                    else
                    {
                        if (tblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL))
                        {

                            //tblBookingsTO.StatusId = Convert.ToInt32(Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL);
                            resultMessage = UpdateQuotaAndSetStatus(tblBookingsTO, conn, tran, tblBookingsTO.UpdatedOn, resultMessage, ref isFromUpdateQuotaAndSetStatus);
                            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                resultMessage.Text = "Error While Update Quota & Set Status";
                                return resultMessage;
                            }
                        }
                        existingTblBookingsTO = tblBookingsTO;
                    }
                }

                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    var res = tblBookingsTO.BookingScheduleTOLst.GroupBy(x => x.ScheduleGroupId);
                    existingTblBookingsTO.NoOfDeliveries = res.Count();
                }
                //Reshma Added For Bulk Booking
                if (tblBookingsTO.IsCreateNewBooking==1)
                {
                    //existingTblBookingsTO.PendingQty =
                    tblBookingsTO.BookingRefId = existingTblBookingsTO.IdBooking;
                    tblBookingsTO.BookingTypeId =(int) Constants.BookingTypeE.Normal;
                    TblBookingsTO tblBookingsTONew = new TblBookingsTO();
                    tblBookingsTONew = tblBookingsTO;
                    tblBookingsTONew.CreatedOn = Constants.ServerDateTime;
                    resultMessage = SaveNewBooking(tblBookingsTONew, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While SaveNewBooking of Normal";
                        //resultMessage.Tag = tblBookingBeyondQuotaTO;
                        return resultMessage;
                    }
                    TblBookingsTO existingTblBookingsTOTemp = SelectTblBookingsTO(tblBookingsTO.BookingRefId, conn, tran);
                    existingTblBookingsTOTemp.PendingQty = existingTblBookingsTOTemp.PendingQty - tblBookingsTO.BookingQty;
                    existingTblBookingsTO = existingTblBookingsTOTemp;
                }
                existingTblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                result = UpdateTblBookings(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblBookings";
                    //resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }


                #region 2. Add Record in tblBookingBeyondQuota For History

                TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                tblBookingBeyondQuotaTO = tblBookingsTO.GetBookingBeyondQuotaTO();

                tblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                tblBookingBeyondQuotaTO.CreatedBy = tblBookingsTO.UpdatedBy;
                tblBookingBeyondQuotaTO.CreatedOn = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusDate = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusId = tblBookingsTO.StatusId;
                tblBookingBeyondQuotaTO.StatusRemark = tblBookingsTO.StatusRemark;
                result = BL.TblBookingBeyondQuotaBL.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingBeyondQuota";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                #endregion

                #region 2.1. Update Quota for Balance Qty

                TblQuotaDeclarationTO existingQuotaTO = TblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(tblBookingsTO.QuotaDeclarationId, conn, tran);
                if (existingQuotaTO == null)
                {
                    tran.Rollback();
                    resultMessage.Text = "Existing Quota Not Found In Function UpdateBookingConfirmations";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                Double availQuota = existingQuotaTO.BalanceQty;
                existingQuotaTO.BalanceQty = existingQuotaTO.BalanceQty + pendQuotaQtyToUpdate;
                existingQuotaTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                existingQuotaTO.UpdatedOn = Constants.ServerDateTime;

                result = TblQuotaDeclarationBL.UpdateTblQuotaDeclaration(existingQuotaTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While Updating Quota UpdateTblQuotaDeclaration in Function SaveNewBooking";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                #endregion

                #region 2.2. Manage Quota Consumption History

                TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO = new TblQuotaConsumHistoryTO();
                tblQuotaConsumHistoryTO.AvailableQuota = availQuota;
                tblQuotaConsumHistoryTO.BalanceQuota = existingQuotaTO.BalanceQty;
                tblQuotaConsumHistoryTO.BookingId = existingTblBookingsTO.IdBooking;
                tblQuotaConsumHistoryTO.CreatedBy = tblBookingsTO.UpdatedBy;
                tblQuotaConsumHistoryTO.CreatedOn = existingQuotaTO.UpdatedOn;
                tblQuotaConsumHistoryTO.QuotaDeclarationId = existingTblBookingsTO.QuotaDeclarationId;
                tblQuotaConsumHistoryTO.QuotaQty = pendQuotaQtyToUpdate;
                tblQuotaConsumHistoryTO.Remark = "Existing Booking Updated for Dealer :" + existingTblBookingsTO.DealerName;
                tblQuotaConsumHistoryTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.UPDATE;

                result = BL.TblQuotaConsumHistoryBL.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While Updating Quota InsertTblQuotaConsumHistory in Function SaveNewBooking";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                #endregion

                #endregion

                //Prajakta[2020-03-25] Commeneted and added size and address details with respect to schedule
                #region 3. Update Materialwise Qty and Rate
                //if (tblBookingsTO.OrderDetailsLst != null && tblBookingsTO.OrderDetailsLst.Count > 0)
                //{
                //    for (int i = 0; i < tblBookingsTO.OrderDetailsLst.Count; i++)
                //    {
                //        Models.TblBookingExtTO tblBookingExtTO = tblBookingsTO.OrderDetailsLst[i];

                //        //Insert New
                //        if (tblBookingExtTO.IdBookingExt <= 0)
                //        {
                //            tblBookingExtTO.BookingId = tblBookingsTO.IdBooking;
                //            tblBookingExtTO.Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                //            result = BL.TblBookingExtBL.InsertTblBookingExt(tblBookingsTO.OrderDetailsLst[i], conn, tran);
                //            if (result != 1)
                //            {
                //                tran.Rollback();
                //                resultMessage.Text = "Error While InsertTblBookingExt in Function UpdateBooking";
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                return resultMessage;
                //            }
                //        }
                //        //Update Existing
                //        else
                //        {
                //            result = BL.TblBookingExtBL.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
                //            if (result != 1)
                //            {
                //                tran.Rollback();
                //                resultMessage.Text = "Error While InsertTblBookingExt in Function UpdateBooking";
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                return resultMessage;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    //Sanjay [2017-03-06] These details are not compulsory while entry
                //    //tran.Rollback();
                //    //resultMessage.Text = "OrderDetailsLst Not Found  While SaveNewBooking";
                //    //resultMessage.MessageType = ResultMessageE.Error;
                //    //return resultMessage;
                //}

                #endregion
                #region 3.Save project schedule 

                List<TblBookingScheduleTO> tempBookingScheduleTOList = new List<TblBookingScheduleTO>();

                List<TblBookingScheduleTO> tblBookingScheduleTOList = TblBookingScheduleBL.SelectAllTblBookingScheduleList(tblBookingsTO.IdBooking, conn, tran);
                if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                {
                    ////Add the pending schedules entry
                    //for (int a = 0; a < tblBookingsTO.BookingScheduleTOLst.Count; a++)
                    //{
                    //    TblBookingScheduleTO temoBookingScheduleTO = tblBookingScheduleTOList.Where(p => p.IdSchedule == tblBookingsTO.BookingScheduleTOLst[a].IdSchedule).FirstOrDefault();
                    //    if (temoBookingScheduleTO != null)
                    //    {
                    //        tempBookingScheduleTOList.Add(tblBookingsTO.BookingScheduleTOLst[a]);
                    //    }
                    //}


                    //tblBookingsTO.BookingScheduleTOLst = tempBookingScheduleTOList;

                    for (int l = 0; l < tblBookingScheduleTOList.Count; l++)
                    {
                        //Delete Ext
                        result = TblBookingExtDAO.DeleteTblBookingExtBySchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                        if (result == -1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = -1;
                            return resultMessage;
                        }

                        //Delete Address
                        result = TblBookingDelAddrDAO.DeleteTblBookingDelAddrByScheduleId(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                        if (result == -1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = -1;
                            return resultMessage;
                        }

                        //Delete Schedule
                        result = TblBookingScheduleDAO.DeleteTblBookingSchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                        if (result == -1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = -1;
                            return resultMessage;
                        }

                    }
                }



                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    for (int i = 0; i < tblBookingsTO.BookingScheduleTOLst.Count; i++)
                    {
                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[i];
                        tblBookingScheduleTO.BookingId = tblBookingsTO.IdBooking;
                        if (tblBookingScheduleTO.CreatedBy != 0)
                        {
                            tblBookingScheduleTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                            tblBookingScheduleTO.UpdatedOn = tblBookingsTO.UpdatedOn;

                        }
                        else
                        {
                            tblBookingScheduleTO.CreatedBy = tblBookingsTO.UpdatedBy;
                            tblBookingScheduleTO.CreatedOn = tblBookingsTO.UpdatedOn;

                        }

                        //if (tblBookingScheduleTO.IdSchedule > 0)
                        //{
                        //    result = TblBookingScheduleDAO.UpdateTblBookingSchedule(tblBookingScheduleTO, conn, tran);
                        //    if (result != 1)
                        //    {
                        //        tran.Rollback();
                        //        resultMessage.DefaultBehaviour();
                        //        return resultMessage;
                        //    }

                        //    if (tblBookingScheduleTO.OrderDetailsLst != null && tblBookingScheduleTO.OrderDetailsLst.Count > 0)
                        //    {
                        //        for (int k = 0; k < tblBookingScheduleTO.OrderDetailsLst.Count; k++)
                        //        {
                        //            result = TblBookingExtDAO.UpdateTblBookingExt(tblBookingScheduleTO.OrderDetailsLst[k], conn, tran);
                        //            if (result != 1)
                        //            {
                        //                tran.Rollback();
                        //                resultMessage.DefaultBehaviour();
                        //                return resultMessage;
                        //            }
                        //        }
                        //    }
                        //    if (tblBookingScheduleTO.DeliveryAddressLst != null && tblBookingScheduleTO.DeliveryAddressLst.Count > 0)
                        //    {
                        //        for (int m = 0; m < tblBookingScheduleTO.DeliveryAddressLst.Count; m++)
                        //        {
                        //            result = TblBookingDelAddrDAO.UpdateTblBookingDelAddr(tblBookingScheduleTO.DeliveryAddressLst[m], conn, tran);
                        //            if (result != 1)
                        //            {
                        //                tran.Rollback();
                        //                resultMessage.DefaultBehaviour();
                        //                return resultMessage;
                        //            }
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //Insert schedule
                        result = TblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingSchedule in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }


                        #region 3.1. Save Materialwise Qty and Rate
                        //[05-09-2018] : Vijaymala added to get default brand for other booking

                        //List<DimBrandTO> brandList = DimBrandDAO.SelectAllDimBrand().Where(ele => ele.IsActive == 1).ToList();
                        //DimBrandTO dimBrandTO = new DimBrandTO();
                        //if (!isRegular)
                        //{
                        //    if (brandList != null && brandList.Count > 0)
                        //    {
                        //        dimBrandTO = brandList.Where(ele => ele.IsDefault == 1).FirstOrDefault();
                        //    }
                        //}

                        if (tblBookingScheduleTO.OrderDetailsLst != null && tblBookingScheduleTO.OrderDetailsLst.Count > 0)
                        {
                            for (int j = 0; j < tblBookingScheduleTO.OrderDetailsLst.Count; j++)
                            {
                                TblBookingExtTO tblBookingExtTO = tblBookingScheduleTO.OrderDetailsLst[j];
                                tblBookingExtTO.BookingId = tblBookingsTO.IdBooking;
                                tblBookingExtTO.Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                                tblBookingExtTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                //[05-09-2018] : Vijaymala added to get default brand for other booking
                                //if (!isRegular)
                                //{
                                //    if (dimBrandTO != null)
                                //    {
                                //        tblBookingExtTO.BrandId = dimBrandTO.IdBrand;
                                //    }
                                //}

                                result = TblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingExt in Function SaveNewBooking";
                                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                    resultMessage.Result = 0;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {

                        }
                        #endregion
                        #region 3.2. Save Order Delivery Addresses

                        if (tblBookingScheduleTO.DeliveryAddressLst != null && tblBookingScheduleTO.DeliveryAddressLst.Count > 0)
                        {
                            for (int k = 0; k < tblBookingScheduleTO.DeliveryAddressLst.Count; k++)
                            {
                                TblBookingDelAddrTO tblBookingDelAddrTO = tblBookingScheduleTO.DeliveryAddressLst[k];
                                if (string.IsNullOrEmpty(tblBookingDelAddrTO.Country))
                                    tblBookingDelAddrTO.Country = Constants.DefaultCountry;

                                tblBookingDelAddrTO.BookingId = tblBookingsTO.IdBooking;
                                tblBookingDelAddrTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                result = TblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Error While Inserting Booking Del Address in Function SaveNewBooking";
                                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            //Sanjay [2017-03-02] These details are not compulsory while entry
                            //tran.Rollback();
                            //resultMessage.Text = "Delivery Address Not Found - Function SaveNewBooking";
                            //resultMessage.MessageType = ResultMessageE.Error;
                            //return resultMessage;
                        }
                        #endregion
                    }

                   // }

                }
                else
                {

                }
                #endregion

                #region 4. Update Order Delivery Addresses

                //if (tblBookingsTO.DeliveryAddressLst != null && tblBookingsTO.DeliveryAddressLst.Count > 0)
                //{
                //    for (int i = 0; i < tblBookingsTO.DeliveryAddressLst.Count; i++)
                //    {
                //        TblBookingDelAddrTO tblBookingDelAddrTO = tblBookingsTO.DeliveryAddressLst[i];

                //        //Insert New
                //        if (tblBookingDelAddrTO.IdBookingDelAddr <= 0)
                //        {
                //            tblBookingDelAddrTO.BookingId = tblBookingsTO.IdBooking;
                //            result = BL.TblBookingDelAddrBL.InsertTblBookingDelAddr(tblBookingsTO.DeliveryAddressLst[i], conn, tran);
                //            if (result != 1)
                //            {
                //                tran.Rollback();
                //                resultMessage.Text = "Error While Inserting Booking Del Address in Function UpdateBooking";
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                return resultMessage;
                //            }
                //        }
                //        //Update Existing
                //        else
                //        {
                //            result = BL.TblBookingDelAddrBL.UpdateTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
                //            if (result != 1)
                //            {
                //                tran.Rollback();
                //                resultMessage.Text = "Error While Inserting Booking Del Address in Function UpdateBooking";
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                return resultMessage;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    //Sanjay [2017-03-06] These details are not compulsory while entry
                //    //tran.Rollback();
                //    //resultMessage.Text = "Delivery Address Not Found - Function SaveNewBooking";
                //    //resultMessage.MessageType = ResultMessageE.Error;
                //    //return resultMessage;
                //}
                #endregion



                //Priyanka [18-04-2019] : Added
                #region 5.Notification section
                if (tblUserTO.OrgTypeId == Convert.ToInt32(Constants.OrgTypeE.DEALER))
                {

                }
                else
                {
                    if (tblBookingsTO.IsWithinQuotaLimit == 0)
                    {
                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                        ResultMessage rMessage = new ResultMessage();
                        List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                        List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblBookingsTO.CnFOrgId, conn, tran);
                        if (cnfUserList != null && cnfUserList.Count > 0)
                        {
                            for (int a = 0; a < cnfUserList.Count; a++)
                            {
                                TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                                tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                                tblAlertUsersTOList.Add(tblAlertUsersTO);
                            }
                        }

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVAL_REQUIRED";
                        tblAlertInstanceTO.AlertComment = "Approval Required For Booking #" + existingTblBookingsTO.BookingDisplayNo;

                        tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.CreatedOn;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceDisplayId = "New Booking";
                        tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                        tblAlertInstanceTO.RaisedBy = tblBookingsTO.CreatedBy;
                        tblAlertInstanceTO.RaisedOn = tblBookingsTO.CreatedOn;
                        tblAlertInstanceTO.IsAutoReset = 1;
                        rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    }
                }
                 #endregion
                    tran.Commit();
                if(isFromUpdateQuotaAndSetStatus == true)
                {
                    resultMessage.Text = "Enquiry Updated Sucessfully, But Sent for Approval.";
                }
                else
                {
                    resultMessage.Text = "Enquiry Updated Sucessfully";
                }
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Tag = tblBookingsTO;
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
        #endregion

        #region Deletion
        public static int DeleteTblBookings(Int32 idBooking)
        {
            return TblBookingsDAO.DeleteTblBookings(idBooking);
        }

        public static int DeleteTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingsDAO.DeleteTblBookings(idBooking, conn, tran);
        }

        #endregion

        //Sudhir[30-APR-2018] Added for Set ParityId is NULL 
        public static int UpdateParityIdNull(SqlConnection conn, SqlTransaction tran)
        {
            int result;
            try
            {
                result = TblBookingsDAO.UpdateParityIdNull(conn, tran);
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


    }
}
