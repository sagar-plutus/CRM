using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.BL
{
    public class TblGlobalRateBL : ITblGlobalRateBL
    {

        private readonly IConnectionString _iConnectionString;
        private readonly ITblGlobalRateDAO _iTblGlobalRateDAO;
        public TblGlobalRateBL(ITblGlobalRateDAO iTblGlobalRateDAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iTblGlobalRateDAO = iTblGlobalRateDAO;
        }
        #region Selection


        public  TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate)
        {
            return _iTblGlobalRateDAO.SelectTblGlobalRate(idGlobalRate);
           
        }

        public  TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblGlobalRateDAO.SelectTblGlobalRate(idGlobalRate,conn,tran);

        }

        public  TblGlobalRateTO SelectLatestTblGlobalRateTO()
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.BeginTransaction();
                tran = conn.BeginTransaction();
                return _iTblGlobalRateDAO.SelectLatestTblGlobalRateTO(conn, tran);
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

        public  TblGlobalRateTO SelectLatestTblGlobalRateTO(SqlConnection conn,SqlTransaction tran)
        {
            return _iTblGlobalRateDAO.SelectLatestTblGlobalRateTO(conn,tran);
        }

        public  List<TblGlobalRateTO> SelectTblGlobalRateTOList(DateTime fromDate,DateTime toDate)
        {
            return _iTblGlobalRateDAO.SelectLatestTblGlobalRateTOList(fromDate,toDate);

        }

        public  Dictionary<Int32, Int32> SelectLatestBrandAndRateDCT()
        {
            return _iTblGlobalRateDAO.SelectLatestBrandAndRateDCT();
        }

        public  Dictionary<Int32, Int32> SelectLatestGroupAndRateDCT()
        {
            return _iTblGlobalRateDAO.SelectLatestGroupAndRateDCT();
        }

        public  Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGlobalRateDAO.IsRateAlreadyDeclaredForTheDate(date, conn,tran);

        }

        /// <summary>
        /// [19/01/2018] Vijaymala : Added To Get Rate Of Group Against Product Item
        /// </summary>
        /// <param name="tblGlobalRateTO"></param>
        /// <returns></returns>

        public  TblGlobalRateTO SelectProductGroupItemGlobalRate(Int32 prodItemId)
        {
            TblGlobalRateTO rateTO = new TblGlobalRateTO();
            // TblGroupItemTO tblGroupItemTO = BL.TblGroupItemBL.SelectTblGroupItemDetails(prodItemId);
            // if (tblGroupItemTO != null)
            // {
            //     Dictionary<Int32, Int32> groupRateDCT = BL.TblGlobalRateBL.SelectLatestGroupAndRateDCT();
            //     if (groupRateDCT != null)
            //     {
            //         if (groupRateDCT.ContainsKey(tblGroupItemTO.GroupId))
            //         {
            //             Int32 rateID = groupRateDCT[tblGroupItemTO.GroupId];
            //             rateTO = BL.TblGlobalRateBL.SelectTblGlobalRateTO(rateID);
            //         }
            //     }
            // }
            return rateTO;

        }
        #endregion

        #region Insertion
        public  int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            return _iTblGlobalRateDAO.InsertTblGlobalRate(tblGlobalRateTO);
        }

        public  int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGlobalRateDAO.InsertTblGlobalRate(tblGlobalRateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            return _iTblGlobalRateDAO.UpdateTblGlobalRate(tblGlobalRateTO);
        }

        public  int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGlobalRateDAO.UpdateTblGlobalRate(tblGlobalRateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblGlobalRate(Int32 idGlobalRate)
        {
            return _iTblGlobalRateDAO.DeleteTblGlobalRate(idGlobalRate);
        }

        public  int DeleteTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGlobalRateDAO.DeleteTblGlobalRate(idGlobalRate, conn, tran);
        }

        #endregion
        
    }
}
