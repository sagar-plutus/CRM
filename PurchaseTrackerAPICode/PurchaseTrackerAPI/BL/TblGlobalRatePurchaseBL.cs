using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblGlobalRatePurchaseBL : ITblGlobalRatePurchaseBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblGlobalRatePurchaseDAO _itblGlobalRatePurchaseDAO;
        private readonly ITblPurchaseEnquiryDAO _iTblPurchaseEnquiryDAO;
        public TblGlobalRatePurchaseBL(ITblGlobalRatePurchaseDAO itblGlobalRatePurchaseDAO, Icommondao icommondao , ITblPurchaseEnquiryDAO iTblPurchaseEnquiryDAO)
        {
            _iCommonDAO = icommondao;
            _itblGlobalRatePurchaseDAO =itblGlobalRatePurchaseDAO;
            _iTblPurchaseEnquiryDAO = iTblPurchaseEnquiryDAO;
        }

        public Int32 noOfIterations = 4;
        #region Selection

        /// <summary>
        /// method added by swati pisal
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TblGlobalRatePurchaseTO> SelectLatestRateOfPurchaseDCT(DateTime sysDate,Boolean isGetLatest = true)
        {
            return _itblGlobalRatePurchaseDAO.SelectLatestRateOfPurchaseDCT(sysDate,isGetLatest);
        }
        public  List<TblGlobalRatePurchaseTO> GetPurchaseRateWithAvgDtls(DateTime fromDate,DateTime toDate)
        {
            return _itblGlobalRatePurchaseDAO.GetPurchaseRateWithAvgDtls(fromDate, toDate);
        }

        public  List<TblGlobalRatePurchaseTO> SelectLatestRateOfPurchaseDCT(Int32 forQuota, DateTime sysDate, ref Int32 Count)
        {
            List<TblGlobalRatePurchaseTO> globalRatePurchase = new List<TblGlobalRatePurchaseTO>();

            if (forQuota == 1)
            {
                sysDate =  _iCommonDAO.ServerDateTime;
                globalRatePurchase =SelectLatestRateOfPurchaseDCT(sysDate);
                if (globalRatePurchase == null || globalRatePurchase.Count <= 0)
                {
                    if (Count > 0)
                    {
                        sysDate = sysDate.AddDays(-1);
                        Count--;
                        globalRatePurchase = SelectLatestRateOfPurchaseDCT(forQuota, sysDate, ref Count);

                    }
                }
            }
            else
            {
                globalRatePurchase =SelectLatestRateOfPurchaseDCT(sysDate);
            }
            return globalRatePurchase;
        }
        public  List<TblGlobalRatePurchaseTO> GetGlobalPurchaseRateList(DateTime fromDate, DateTime toDate)
        {
            List < TblGlobalRatePurchaseTO > globalRatePurchaseList = _itblGlobalRatePurchaseDAO.GetGlobalPurchaseRateList(fromDate, toDate);
            if(globalRatePurchaseList!=null && globalRatePurchaseList.Count>0)
            {
                List<TblPurchaseEnquiryTO> purchaseEnquiryTOList = new List<TblPurchaseEnquiryTO>();
                string globalRatePurchaseIds = string.Join(", ", globalRatePurchaseList.Select(i => i.IdGlobalRatePurchase));
                if (globalRatePurchaseIds != null)
                {
                    purchaseEnquiryTOList = _iTblPurchaseEnquiryDAO.SelectPurchaseEnquiryRateWise(globalRatePurchaseIds);
                }

                if(purchaseEnquiryTOList!=null && purchaseEnquiryTOList.Count>0)
                {
                    for (int i = 0; i < globalRatePurchaseList.Count; i++)
                    {
                        TblGlobalRatePurchaseTO globalRateTO = globalRatePurchaseList[i];
                        List<TblPurchaseEnquiryTO> tempPurchaseEnquiryTOList = purchaseEnquiryTOList.Where(e => e.GlobalRatePurchaseId == globalRateTO.IdGlobalRatePurchase).ToList();

                        if (tempPurchaseEnquiryTOList != null && tempPurchaseEnquiryTOList.Count>0)
                        {
                            globalRateTO.TotalBookingQty = tempPurchaseEnquiryTOList[0].BookingQty;
                            globalRateTO.AvgBookingRate = tempPurchaseEnquiryTOList[0].BookingRate;
                        }

                    }
                }

            }
            return globalRatePurchaseList;
        }

        public  Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGlobalRatePurchaseDAO.IsRateAlreadyDeclaredForTheDate(date, conn, tran);

        }

        public  TblGlobalRatePurchaseTO SelectTblGlobalRatePurchaseTO(Int32 idGlobalRatePurchase, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGlobalRatePurchaseDAO.SelectTblGlobalRatePurchase(idGlobalRatePurchase, conn, tran);

        }
        #endregion

        #region Insertion

        public  int InsertTblGlobalRatePurchase(TblGlobalRatePurchaseTO TblGlobalRatePurchaseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGlobalRatePurchaseDAO.InsertTblGlobalRatePurchase(TblGlobalRatePurchaseTO, conn, tran);
        }

        #endregion

    }
}
