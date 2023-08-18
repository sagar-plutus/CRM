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
    public class TblBookingExtBL
    {
        #region Selection
       
        public static List<TblBookingExtTO> SelectAllTblBookingExtList()
        {
           return TblBookingExtDAO.SelectAllTblBookingExt();
      
        }

        public static List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId)
        {
            return TblBookingExtDAO.SelectAllTblBookingExt(bookingId);

        }

        public static List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.SelectAllTblBookingExt(bookingId, conn, tran);

        }
        public static List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(Int32 scheduleId)
        {
            return TblBookingExtDAO.SelectAllTblBookingExtListBySchedule(scheduleId);

        }

        /// <summary>
        /// Sanjay [2017-03-27] This list will return all material alongwith prod cat and spec
        /// Required while showing popup to take material wise ,spec wise qty while booking
        /// </summary>
        /// <param name="prodCatgId"></param>
        /// <returns></returns>
        public static List<TblBookingExtTO> SelectEmptyBookingExtList(int prodCatgId,int prodSpecId)
        {
            List<TblBookingExtTO> list= TblBookingExtDAO.SelectEmptyBookingExtList(prodCatgId,prodSpecId);

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

            return list;
        }



        public static List<TblBookingExtTO> SelectAllBookingDetailsWrtDealer(Int32 dealerId)
        {
            return TblBookingExtDAO.SelectBookingDetails(dealerId);

        }

        public static TblBookingExtTO SelectTblBookingExtTO(Int32 idBookingExt)
        {
            return TblBookingExtDAO.SelectTblBookingExt(idBookingExt);

        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            return TblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO);
        }

        public static int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            return TblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO);
        }

        public static int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblBookingExt(Int32 idBookingExt)
        {
            return TblBookingExtDAO.DeleteTblBookingExt(idBookingExt);
        }

        public static int DeleteTblBookingExt(Int32 idBookingExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.DeleteTblBookingExt(idBookingExt, conn, tran);
        }

        #endregion
        
    }
}
