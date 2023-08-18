using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SalesTrackerAPI.StaticStuff;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblModuleCommunicationBL
    {
        #region Selection
     
        public static List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationList()
        {
            return TblModuleCommunicationDAO.SelectAllTblModuleCommunication();
        }

        public static List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationListById(Int32 srcModuleId, Int32 srcTxnId)
        {
            return TblModuleCommunicationDAO.SelectAllTblModuleCommunicationById(srcModuleId, srcTxnId);
        }

        public static TblModuleCommunicationTO SelectTblModuleCommunicationTO(Int32 idModuleCommunication)
        {
            return TblModuleCommunicationDAO.SelectTblModuleCommunication(idModuleCommunication);
           
        }

        #endregion
        
        #region Insertion
        public static int InsertTblModuleCommunication(List<TblModuleCommunicationTO> tblModuleCommunicationList, string loginUserId)
        {
            foreach (var item in tblModuleCommunicationList)
            {
                item.CreatedBy = Convert.ToInt32(loginUserId);
                item.CreatedOn = Constants.ServerDateTime;
                int result = TblModuleCommunicationDAO.InsertTblModuleCommunication(item);
                if (result != 1)
                {
                    return -1;
                }
            }
            return 1;
        }

        public static int InsertTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblModuleCommunicationDAO.InsertTblModuleCommunication(tblModuleCommunicationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO)
        {
            return TblModuleCommunicationDAO.UpdateTblModuleCommunication(tblModuleCommunicationTO);
        }

        public static int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblModuleCommunicationDAO.UpdateTblModuleCommunication(tblModuleCommunicationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblModuleCommunication(Int32 idModuleCommunication)
        {
            return TblModuleCommunicationDAO.DeleteTblModuleCommunication(idModuleCommunication);
        }

        public static int DeleteTblModuleCommunication(Int32 idModuleCommunication, SqlConnection conn, SqlTransaction tran)
        {
            return TblModuleCommunicationDAO.DeleteTblModuleCommunication(idModuleCommunication, conn, tran);
        }

        #endregion
        
    }
}
