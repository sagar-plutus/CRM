using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblTaskModuleExtBL
    {

        #region Selection
        public static List<TblTaskModuleExtTO> SelectAllTblTaskModuleExt()
        {
            return TblTaskModuleExtDAO.SelectAllTblTaskModuleExt();
        }

        //public static List<TblTaskModuleExtTO> SelectAllTblTaskModuleExtList()
        //{
        //   return TblTaskModuleExtDAO.SelectAllTblTaskModuleExt();
        //}

        public static TblTaskModuleExtTO SelectTblTaskModuleExtTO(Int32 idTaskModuleExt)
        {
             TblTaskModuleExtTO tblTaskModuleExtTODT = TblTaskModuleExtDAO.SelectTblTaskModuleExt(idTaskModuleExt);
            if (tblTaskModuleExtTODT != null )
                return tblTaskModuleExtTODT;
            else
                return null;
        }

        public static List<TblTaskModuleExtTO> SelectTaskModuleDetailsByEntityId(Int32 EntityId)
        {
            return TblTaskModuleExtDAO.SelectTaskModuleDetailsByEntityId(EntityId);
        }

        #endregion

        #region Insertion
        public static int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO)
        {
            return TblTaskModuleExtDAO.InsertTblTaskModuleExt(tblTaskModuleExtTO);
        }

        public static int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblTaskModuleExtDAO.InsertTblTaskModuleExt(tblTaskModuleExtTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO)
        {
            return TblTaskModuleExtDAO.UpdateTblTaskModuleExt(tblTaskModuleExtTO);
        }

        public static int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblTaskModuleExtDAO.UpdateTblTaskModuleExt(tblTaskModuleExtTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblTaskModuleExt(Int32 idTaskModuleExt)
        {
            return TblTaskModuleExtDAO.DeleteTblTaskModuleExt(idTaskModuleExt);
        }

        public static int DeleteTblTaskModuleExt(Int32 idTaskModuleExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblTaskModuleExtDAO.DeleteTblTaskModuleExt(idTaskModuleExt, conn, tran);
        }

        #endregion

    }
}
