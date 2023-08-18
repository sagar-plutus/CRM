using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.BL
{
    public class TblUnLoadingItemDetBL
    {
        #region Selection

        /// <summary>
        /// Vaibhav [13-Sep-2017] Get all unloading item details 
        /// </summary>
        /// <param name="unLoadingId"></param>
        /// <returns></returns>
        public static List<TblUnLoadingItemDetTO> SelectAllUnLoadingItemDetailsList(int unLoadingId = 0)
        {
            ResultMessage resultMessage = new ResultMessage();
           try
            {
                return TblUnLoadingItemDetDAO.SelectAllTblUnLoadingItemDetails(unLoadingId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnLoadingItemDetailsList");
                return null;
            }
        }


        public static TblUnLoadingItemDetTO SelectTblUnLoadingItemDetTO(Int32 idUnloadingItemDet)
        {
            return TblUnLoadingItemDetDAO.SelectTblUnLoadingItemDet(idUnloadingItemDet);
        }
        #endregion

        #region Insertion
        public static int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO)
        {
            return TblUnLoadingItemDetDAO.InsertTblUnLoadingItemDet(tblUnLoadingItemDetTO);
        }

        public static int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnLoadingItemDetDAO.InsertTblUnLoadingItemDet(tblUnLoadingItemDetTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO)
        {
            return TblUnLoadingItemDetDAO.UpdateTblUnLoadingItemDet(tblUnLoadingItemDetTO);
        }

        public static int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnLoadingItemDetDAO.UpdateTblUnLoadingItemDet(tblUnLoadingItemDetTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet)
        {
            return TblUnLoadingItemDetDAO.DeleteTblUnLoadingItemDet(idUnloadingItemDet);
        }

        public static int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnLoadingItemDetDAO.DeleteTblUnLoadingItemDet(idUnloadingItemDet, conn, tran);
        }

        #endregion

    }
}
