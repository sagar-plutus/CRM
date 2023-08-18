using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

 
namespace SalesTrackerAPI.BL
{ 
    public class TblBookingScheduleBL
    {
        
        #region Selection

        public static List<TblBookingScheduleTO> SelectAllTblBookingScheduleList()
        {
            return TblBookingScheduleDAO.SelectAllTblBookingSchedule();
          
        }

        public static TblBookingScheduleTO SelectTblBookingScheduleTO(Int32 idSchedule)
        {
           return TblBookingScheduleDAO.SelectTblBookingSchedule(idSchedule);
        }

        public static List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId)
        {
            return TblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId);
        }

        public static List<TblBookingScheduleTO> GetAllBookingScheduleList(DateTime fromDate, DateTime toDate, Int32 cnfOrgId, Int32 dealerOrgId)
        {
            List<TblBookingScheduleTO> scheduleTOReturnList = new List<TblBookingScheduleTO>();

            List<TblBookingScheduleTO> scheduleTOList = TblBookingScheduleDAO.GetAllBookingScheduleList(fromDate, toDate, cnfOrgId, dealerOrgId);
            if (scheduleTOList != null && scheduleTOList.Count > 0)
            {

                List<TblBookingScheduleTO> distinctBookingIds = scheduleTOList.GroupBy(w => w.BookingId).Select(x => x.FirstOrDefault()).ToList();

                if (distinctBookingIds != null && distinctBookingIds.Count > 0)
                {
                    for (int i = 0; i < distinctBookingIds.Count; i++)
                    {

                        List<TblBookingScheduleTO> localList = scheduleTOList.Where(a => a.BookingId == distinctBookingIds[i].BookingId).ToList();

                        List<TblBookingScheduleTO> schedulelocalList = localList.GroupBy(w => w.ScheduleGroupId).Select(x => x.FirstOrDefault()).ToList();


                        if (schedulelocalList != null && schedulelocalList.Count > 0)
                        {
                            for (int k = 0; k < schedulelocalList.Count; k++)
                            {
                                List<TblBookingScheduleTO> tempList = localList.Where(a => a.ScheduleGroupId == schedulelocalList[k].ScheduleGroupId).ToList();
                                if (tempList != null && tempList.Count > 0)
                                {
                                    tempList = tempList.OrderByDescending(a => a.CreatedOn).ToList();
                                    tempList[0].IsDisplay = true;
                                }

                                scheduleTOReturnList.AddRange(tempList);
                            }
                        }


                    }
                }
            }

            if(scheduleTOReturnList != null && scheduleTOReturnList.Count > 0)
            {
                for (int k = 0; k < scheduleTOReturnList.Count; k++)
                {
                    scheduleTOReturnList[k].OrderDetailsLst = TblBookingExtDAO.SelectAllTblBookingExtListBySchedule(scheduleTOReturnList[k].IdSchedule);
                }
            }

            return scheduleTOReturnList;
        }

        public static List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId,SqlConnection conn,SqlTransaction tran)
        {
            List<TblBookingScheduleTO> list = TblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId, conn,tran);
            if (list != null && list.Count > 0)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    TblBookingScheduleTO tblBookingScheduleTO = list[k];

                    tblBookingScheduleTO.DeliveryAddressLst = TblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);

                    if (tblBookingScheduleTO.DeliveryAddressLst != null && tblBookingScheduleTO.DeliveryAddressLst.Count > 0)
                    {
                        for (int c = 0; c < tblBookingScheduleTO.DeliveryAddressLst.Count; c++)
                        {
                            tblBookingScheduleTO.DeliveryAddressLst[c].LoadingLayerId = tblBookingScheduleTO.LoadingLayerId;
                        }
                    }

                    tblBookingScheduleTO.OrderDetailsLst = TblBookingExtDAO.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
                }
            }
            return list;
        }


        //public List<TblBookingScheduleTO> SelectBookingScheduleByBookingId(Int32 bookingId)
        //{
        //    List<TblBookingScheduleTO> list = SelectAllTblBookingScheduleList(bookingId);

        //    if(list!=null && list.Count >0)
        //    {
        //        for (int k = 0; k < list.Count; k++)
        //        {
        //            TblBookingScheduleTO tblBookingScheduleTO = list[k];

        //            tblBookingScheduleTO.DeliveryAddressLst = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);

        //            tblBookingScheduleTO.OrderDetailsLst = BL.TblBookingExtBL.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
        //        }
        //    }

        //    return list;
        //}

        public static List<TblBookingScheduleTO> SelectBookingScheduleByBookingId(Int32 bookingId)
        {
            List<TblBookingScheduleTO> list = TblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId);
            if (list != null && list.Count > 0)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    TblBookingScheduleTO tblBookingScheduleTO = list[k];

                    tblBookingScheduleTO.DeliveryAddressLst = TblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);

                    if(tblBookingScheduleTO.DeliveryAddressLst!=null && tblBookingScheduleTO.DeliveryAddressLst.Count>0)
                    {
                        for (int c = 0; c < tblBookingScheduleTO.DeliveryAddressLst.Count; c++)
                        {
                            tblBookingScheduleTO.DeliveryAddressLst[c].LoadingLayerId = tblBookingScheduleTO.LoadingLayerId;
                        }
                    }

                    tblBookingScheduleTO.OrderDetailsLst = TblBookingExtDAO.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
                }
            }
            return list;
        }


        #endregion

        #region Insertion
        public static int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            return TblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO);
        }

        public static int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            return TblBookingScheduleDAO.UpdateTblBookingSchedule(tblBookingScheduleTO);
        }

        public static int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingScheduleDAO.UpdateTblBookingSchedule(tblBookingScheduleTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblBookingSchedule(Int32 idSchedule)
        {
            return TblBookingScheduleDAO.DeleteTblBookingSchedule(idSchedule);
        }

        public static int DeleteTblBookingSchedule(Int32 idSchedule, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingScheduleDAO.DeleteTblBookingSchedule(idSchedule, conn, tran);
        }

        #endregion
        
    }
}
