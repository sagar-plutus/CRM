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
    public class TblLoadingSlipExtBL
    {
        #region Selection
       
        public static List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList()
        {
            return TblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt();
            
        }

        public static List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList(int loadingSlipId)
        {
            return TblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(loadingSlipId);

        }


        /// <summary>
        /// Sanjay [2017-12-18] It ll return record count against each loading. For the status of Vehicle IN i.e Gate In
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Int32, string> SelectLoadingWiseExtRecordCountDCT()
        {
            return TblLoadingSlipExtDAO.SelectLoadingWiseExtRecordCountDCT();
        }

        public static List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList(int loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(loadingSlipId,conn,tran);
        }

        public static TblLoadingSlipExtTO SelectTblLoadingSlipExtTO(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
           return TblLoadingSlipExtDAO.SelectTblLoadingSlipExt(idLoadingSlipExt, conn, tran);
       
        }

        public static List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExtList(Int32 prodCatId,  int bookingId)
        {
            List<TblLoadingSlipExtTO> list = TblLoadingSlipExtDAO.SelectEmptyLoadingSlipExt(prodCatId);
            if (list != null)
            {
                List<TblBookingExtTO> bookingList = BL.TblBookingExtBL.SelectAllTblBookingExtList(bookingId);
                if (bookingList != null && bookingList.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var matchedTOList = bookingList.Where(b => b.MaterialId == list[i].MaterialId
                                                          && b.ProdCatId == list[i].ProdCatId
                                                          && b.ProdSpecId == list[i].ProdSpecId).ToList();
                        if (matchedTOList != null)
                        {
                            list[i].LoadingQty = matchedTOList.Sum(b => b.BookedQty);
                        }
                    }
                }
            }
            return list;

        }

        public static List<TblBookingScheduleTO> SelectEmptyLoadingSlipExtListAgainstSch(Int32 prodCatId, Int32 prodSpecId, int bookingId, int brandId)
        {
            List<TblBookingScheduleTO> list = TblBookingScheduleBL.SelectAllTblBookingScheduleList(bookingId);
            List<TblProductInfoTO> tblProductInfoTOs = TblProductInfoBL.SelectAllTblProductInfoListLatest();
            if (list != null && list.Count > 0)
            {
                list = list.OrderBy(o => o.ScheduleDate).ToList();

                for (int k = 0; k < list.Count; k++)
                {
                    TblBookingScheduleTO tblBookingScheduleTO = list[k];
                    List<TblProductInfoTO> tblProductInfoTOList = new List<TblProductInfoTO>();
                    tblBookingScheduleTO.DeliveryAddressLst = TblBookingDelAddrBL.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);

                    List<TblBookingExtTO> bookingList = TblBookingExtBL.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);


                    if (bookingList != null && bookingList.Count > 0)
                    {
                        for (int i = 0; i < bookingList.Count; i++)
                        {
                            var matchedAvgWt = tblProductInfoTOs.Where(b => b.MaterialId == bookingList[i].MaterialId
                                                           && b.ProdCatId == bookingList[i].ProdCatId
                                                           && b.ProdSpecId == bookingList[i].ProdSpecId
                                                         ).ToList();

                            TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO(bookingList[i]);
                            tblLoadingSlipExtTO.LoadingQty = bookingList[i].BalanceQty;
                            //tblLoadingSlipExtTO.ConversionFactor = bookingList[i].ConversionFactor;
                            //tblLoadingSlipExtTO.AvgBundleWt = bookingList[i].AvgBundleWt;
                            tblLoadingSlipExtTO.SchedulelayerId = tblBookingScheduleTO.LoadingLayerId;
                            //tblLoadingSlipExtTO.Bundles = bookingList[i].PendingUomQty;
                            //if (matchedAvgWt.Count > 0 && matchedAvgWt != null)
                            //    tblLoadingSlipExtTO.AvgBundleWt = matchedAvgWt[0].AvgBundleWt;
                            list[k].LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                        }

                        tblBookingScheduleTO.BalanceQty = bookingList.Sum(s => s.BalanceQty);

                    }

                }

                list = list.Where(w => w.BalanceQty > 0).ToList();

            }
            return list;
        }

        public static List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExtList(Int32 prodCatId, Int32 prodSpecId, int bookingId)
        {
            List<TblBookingExtTO> bookingList = new List<TblBookingExtTO>();
            if(bookingId > 0)
            {
                bookingList = BL.TblBookingExtBL.SelectAllTblBookingExtList(bookingId);
            }
            

            if (bookingList != null && bookingList.Count > 0)
            {
                prodCatId = 0;
                prodSpecId = 0;
            }
            List<TblLoadingSlipExtTO> list = TblLoadingSlipExtDAO.SelectEmptyLoadingSlipExt(prodCatId, prodSpecId);
            if (list != null)
            {
                List<TblLoadingSlipExtTO> finalList = new List<TblLoadingSlipExtTO>();
                if (bookingList != null && bookingList.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var matchedTOList = bookingList.Where(b => b.MaterialId == list[i].MaterialId
                                                          && b.ProdCatId == list[i].ProdCatId
                                                          && b.ProdSpecId == list[i].ProdSpecId
                                                          ).ToList();
                        if (matchedTOList != null && matchedTOList.Count  > 0)
                        {
                            list[i].LoadingQty = matchedTOList.Sum(b => b.BookedQty);
                            finalList.Add(list[i]);
                        }
                    }

                    //Sudhir[02-APR-2018] Added for Return LoadingSlipExtList Based on Other Item 
                    List<TblLoadingSlipExtTO> otherItemList = new List<TblLoadingSlipExtTO>();
                    List<TblBookingExtTO> filterBookingExtList = bookingList.Where(ele => ele.ProdItemId > 0).ToList();
                    
                    if (filterBookingExtList != null && filterBookingExtList.Count > 0)
                    {
                        foreach (TblBookingExtTO bookingExtTo in bookingList)
                        {
                            TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                            tblLoadingSlipExtTO.MaterialId = bookingExtTo.MaterialId;
                            tblLoadingSlipExtTO.ProdCatId = bookingExtTo.ProdCatId;
                            tblLoadingSlipExtTO.ProdSpecId = bookingExtTo.ProdSpecId;
                            tblLoadingSlipExtTO.ProdItemId = bookingExtTo.ProdItemId;
                            tblLoadingSlipExtTO.BookingId = bookingExtTo.BookingId;
                            tblLoadingSlipExtTO.BookingExtId = bookingExtTo.IdBookingExt;
                            tblLoadingSlipExtTO.DisplayName = bookingExtTo.DisplayName;
                            tblLoadingSlipExtTO.LoadingQty = bookingExtTo.BookedQty;
                            tblLoadingSlipExtTO.MaterialDesc = bookingExtTo.MaterialSubType;

                            otherItemList.Add(tblLoadingSlipExtTO);
                        }
                        finalList = otherItemList;
                    }

                    return finalList;
                }

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.LOADING_SLIP_DEFAULT_SIZES);
                String sizes = string.Empty;

                if (tblConfigParamsTO != null)
                    sizes = Convert.ToString(tblConfigParamsTO.ConfigParamVal);

                string[] sizeList = sizes.Split(',');

                for (int l = 0; l < list.Count; l++)
                {
                    int materialId = list[l].MaterialId;
                    if (Constants.IsNeedToRemoveFromList(sizeList, materialId))
                    {
                        list.RemoveAt(l);
                        l--;
                    }
                }
            }
            return list;

        }

        public static List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(Int32 loadingId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(loadingId.ToString(), conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        public static List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(Int32 loadingId,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(loadingId.ToString(),conn,tran);

        }

        public static Dictionary<Int32,Double> SelectLoadingQuotaWiseApprovedLoadingQtyDCT(String loadingQuotaIds,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingSlipExtDAO.SelectLoadingQuotaWiseApprovedLoadingQtyDCT(loadingQuotaIds, conn, tran);
        }

        public static List<TblLoadingSlipExtTO> SelectCnfWiseLoadingMaterialToPostPoneList(SqlConnection conn, SqlTransaction tran)
        {
            List<TblLoadingSlipExtTO> postponeList = null;
            TblConfigParamsTO postponeConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID, conn, tran);
            //Sanjay [2017-07-18] The vehicles which are gate in ,loading completed or postponed
            //List<TblLoadingTO> loadingTOToPostponeList = TblLoadingDAO.SelectAllLoadingListByStatus((int)Constants.TranStatusE.LOADING_POSTPONED + "", conn, tran);
            List<TblLoadingTO> loadingTOToPostponeList = TblLoadingDAO.SelectAllLoadingListByStatus(postponeConfigParamsTO.ConfigParamVal, conn, tran);
            if (loadingTOToPostponeList != null)
            {
                postponeList = new List<TblLoadingSlipExtTO>();
                var loadingIds = string.Join(",", loadingTOToPostponeList.Where(x=>x.LoadingTypeE==Constants.LoadingTypeE.REGULAR).Select(p => p.IdLoading.ToString()));
                List<TblLoadingSlipExtTO> extList = TblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(loadingIds, conn, tran);
                if (extList != null && extList.Count > 0)
                {
                    postponeList.AddRange(extList);
                }
            }

            return postponeList;
        }
        #endregion

        #region Insertion
        public static int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            return TblLoadingSlipExtDAO.InsertTblLoadingSlipExt(tblLoadingSlipExtTO);
        }

        public static int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtDAO.InsertTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            return TblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO);
        }

        public static int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
        }

        public static int UpdateFinalLoadingSlipExtCalTareWt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtDAO.UpdateFinalLoadingSlipExtCalTareWt(tblLoadingSlipExtTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt)
        {
            return TblLoadingSlipExtDAO.DeleteTblLoadingSlipExt(idLoadingSlipExt);
        }

        public static int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtDAO.DeleteTblLoadingSlipExt(idLoadingSlipExt, conn, tran);
        }

        #endregion
        
    }
}
