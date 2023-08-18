using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SalesTrackerAPI.BL
{
    public class TblSessionBL
    {
        #region Selection
        public static TblSessionTO SelectAllTblSession()
        {
            return TblSessionDAO.SelectAllTblSession();
        }

        public static List<TblSessionTO> SelectAllTblSessionList()
        {
            return TblSessionDAO.SelectAllTblSessionData();
        }

        public static TblSessionTO SelectTblSessionTO(int idsession)
        {
            return TblSessionDAO.SelectTblSession(idsession);
        }

        public static TblSessionTO getSessionAllreadyExist(Int32 idUser,Int32 ConversionUserId)
        {
            return TblSessionDAO.getSessionAllreadyExist(idUser, ConversionUserId);
        }

        #endregion

        #region Insertion
        public static int InsertTblSession(TblSessionTO tblSessionTO)
        {
            return TblSessionDAO.InsertTblSession(tblSessionTO);
        }

        public static int InsertTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSessionDAO.InsertTblSession(tblSessionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblSession(int idsession)
        {
            TblSessionTO SessionTO = new TblSessionTO();
            SessionTO =  TblSessionDAO.SelectTblSession(idsession);
            if (SessionTO != null)
            {
                SessionTO.EndTime = StaticStuff.Constants.ServerDateTime;
                SessionTO.IsEndSession = 1;
                return DAL.TblSessionDAO.UpdateTblSession(SessionTO);
            }
            else
            {
                return 0;
            }
        }

        public static int UpdateTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSessionDAO.UpdateTblSession(tblSessionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblSession(int idsession)
        {
            return TblSessionDAO.DeleteTblSession(idsession);
        }
        public static int deleteAllMsgData()
        {
            TblSessionHistoryBL.DeleteTblSessionHistory();
            return TblSessionDAO.DeleteTblSession();

        }

        public static int DeleteTblSession(int idsession, SqlConnection conn, SqlTransaction tran)
        {
            return TblSessionDAO.DeleteTblSession(idsession, conn, tran);
        }

        #endregion
        
    }
}
